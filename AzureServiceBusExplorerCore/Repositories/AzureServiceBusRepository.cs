using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Repositories
{
    public class AzureServiceBusRepository
    {
        private readonly Dictionary<string, IQueueClient> _activeQueueClients = new Dictionary<string, IQueueClient>();

        private readonly MessageHandlerOptions _messageOptions = new MessageHandlerOptions(MessagePumpExceptionHandler)
        {
            MaxConcurrentCalls = 1,
            AutoComplete = false
        };

        private readonly IQueueClientFactory _queueClientFactory;
        private MessageState _messageState;

        public AzureServiceBusRepository(IQueueClientFactory queueClientFactory)
        {
            _queueClientFactory = queueClientFactory;
        }

        public IQueueClient GetQueueClient(QueueDescription queue)
        {
            if (_activeQueueClients.ContainsKey(queue.Path))
                return _activeQueueClients[queue.Path];
            var queueClient = _queueClientFactory.GetQueueClient(queue.Path);
            _activeQueueClients.Add(queue.Path, queueClient);
            return queueClient;
        }

        public async Task<IList<string>> GetMessagesAsync(QueueDescription queue, int n, int timeoutInSeconds = 30)
        {
            var queueClient = GetQueueClient(queue);

            _messageState = new MessageState(n);

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(
                (message, _) => MessagePumpReadHandler(message, _messageState), _messageOptions);

            await WaitForServicePumpActionAsync(() => _messageState.IsFull(), timeoutInSeconds);
            _activeQueueClients.Remove(queueClient.Path); //TODO: Gross way of handling queue disposal
            await queueClient.CloseAsync();
            return _messageState.GetMessages();
        }

        public Task SendMessagesAsync(QueueDescription queue, IList<Message> messages)
        {
            var queueClient = GetQueueClient(queue);
            return queueClient.SendAsync(messages);
        }

        public async Task DeleteMessageAsync(QueueDescription queue, Message message, int timeoutInSeconds = 30)
        {
            var queueClient = GetQueueClient(queue);

            bool completed = false;
            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(
                (incomingMessage, _) => MessagePumpDeleteHandler(queueClient, incomingMessage, message, ref completed),
                _messageOptions);

            await WaitForServicePumpActionAsync(() => completed, timeoutInSeconds);
        }

        internal static Task MessagePumpExceptionHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        }

        internal static Task MessagePumpReadHandler(Message message, MessageState state)
        {
            state.AddMessage(Encoding.UTF8.GetString(message.Body));
            return Task.CompletedTask;
        }

        internal Task MessagePumpDeleteHandler(IQueueClient queueClient, Message incomingMessage,
            Message messageToDelete, ref bool notifier)
        {
            if (incomingMessage.MessageId == messageToDelete.MessageId)
            {
                queueClient.CompleteAsync(incomingMessage.SystemProperties.IsLockTokenSet
                    ? incomingMessage.SystemProperties.LockToken
                    : null);
                notifier = true;
                _activeQueueClients.Remove(queueClient.Path); //TODO: Gross way of handling queue disposal
                queueClient.CloseAsync();

            }

            return Task.CompletedTask;
        }

        private static Task WaitForServicePumpActionAsync(Func<bool> action, int timeoutInSeconds)
        {
            return Task.Run(() =>
            {
                var time = 0;
                while (!action() && time < timeoutInSeconds)
                {
                    Thread.Sleep(1000);
                    time++;
                }
            });
        }

        internal MessageState GetMessageState()
        {
            return _messageState;
        }

        public int CountActiveQueueClients()
        {
            return _activeQueueClients.Count;
        }

        internal class MessageState
        {
            private readonly int _maxSize;
            private readonly IList<string> _messages;

            public MessageState(int n)
            {
                _maxSize = n;
                _messages = new List<string>(n);
            }

            public void AddMessage(string message)
            {
                if (!IsFull()) _messages.Add(message);
            }

            public IList<string> GetMessages()
            {
                return _messages;
            }

            public bool IsFull()
            {
                return _messages.Count == _maxSize;
            }
        }
    }
}