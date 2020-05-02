using System.Collections.Generic;
using AzureServiceBusExplorerCore.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

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

        public IQueueClient GetQueueClient(QueueDescription queue)
        {
            if (_activeQueueClients.ContainsKey(queue.Path))
                return _activeQueueClients[queue.Path];
            var queueClient = _queueClientFactory.GetQueueClient(queue.Path);
            _activeQueueClients.Add(queue.Path, queueClient);
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