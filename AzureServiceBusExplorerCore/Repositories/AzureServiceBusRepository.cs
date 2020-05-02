using System.Collections.Generic;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Models;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBusExplorerCore.Repositories
{
    public class AzureServiceBusRepository
    {
        private readonly Dictionary<string, IQueueClient> _activeQueueClients = new Dictionary<string, IQueueClient>();
        private readonly IQueueClientFactory _queueClientFactory;

        public AzureServiceBusRepository(IQueueClientFactory queueClientFactory)
        {
            _queueClientFactory = queueClientFactory;
        }

        public IQueueClient GetQueueClient(Queue queue)
        {
            if (_activeQueueClients.ContainsKey(queue.QueueName))
                return _activeQueueClients[queue.QueueName];
            var queueClient = _queueClientFactory.GetQueueClient(queue.QueueName);
            _activeQueueClients.Add(queue.QueueName, queueClient);
            return queueClient;
        }

        // public IList<string> GetMessages(Queue queue)
        // {
        //     var queueClient = GetQueueClient(queue);

        //     return null;
        //     //queueClient.
        // }

        public int CountActiveQueueClients()
        {
            return _activeQueueClients.Count;
        }
    }
}