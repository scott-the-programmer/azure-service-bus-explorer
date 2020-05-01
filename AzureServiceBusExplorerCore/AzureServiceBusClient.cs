using System.Collections.Generic;
using AzureServiceBusExplorerCore.Models.Interfaces;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBusExplorerCore
{
    public class AzureServiceBusClient
    {
        private readonly string _serviceBusConnectionString;
        private readonly Dictionary<string, IQueueClient> _activeQueueClients = new Dictionary<string, IQueueClient>();

        public AzureServiceBusClient(string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
        }

        public IQueueClient GetQueueClient(IQueue queue)
        {
            if (_activeQueueClients.ContainsKey(queue.QueueName))
                return _activeQueueClients[queue.QueueName];
            IQueueClient queueClient = new QueueClient(_serviceBusConnectionString, queue.QueueName);
            _activeQueueClients.Add(queue.QueueName, queueClient);
            return queueClient;
        }

        public IList<IMessage> GetMessages(IQueue queue)
        {
            var queueClient = GetQueueClient(queue);

            return null;
            //queueClient.
        }
    }
}