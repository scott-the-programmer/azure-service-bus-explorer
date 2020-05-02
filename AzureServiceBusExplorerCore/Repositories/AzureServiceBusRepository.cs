using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        public Task<IList<string>> GetMessages(QueueDescription queue, int n, int timeoutInSeconds = 30)
        {
            var queueClient = GetQueueClient(queue);

            _messageState = new MessageState(n);
            var messageHandlerOptions = new MessageHandlerOptions(args =>
            {
                Console.WriteLine(args.Exception);
                return Task.CompletedTask;
            })
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler((message, token) =>
            {
                _messageState.AddMessage(Encoding.UTF8.GetString(message.Body));
                return Task.CompletedTask;
            }, messageHandlerOptions);

            return Task.Run(async () =>
            {
                var time = 0;
                while (!_messageState.IsFull() && time < timeoutInSeconds)
                {
                    Thread.Sleep(1000);
                    time++;
                }
                await queueClient.CloseAsync();
                return _messageState.GetMessages();
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
            private readonly IList<string> _messages;
            private readonly int _maxSize;

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