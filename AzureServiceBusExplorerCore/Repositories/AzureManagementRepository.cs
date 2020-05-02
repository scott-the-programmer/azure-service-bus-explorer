using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Models;
using AzureServiceBusExplorerCore.Models.Interfaces;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Repositories
{
    public class AzureManagementClient
    {
        private readonly ManagementClient _managementClient;

        public AzureManagementClient(string connectionString)
        {
            _managementClient = new ManagementClient(connectionString);
        }

        public async Task<IList<IQueue>> GetQueues()
        {
            var queueDescriptions = await _managementClient.GetQueuesAsync();
            var queues = queueDescriptions.Select(description => new Queue(description.Path, description.UserMetadata))
                .ToList<IQueue>();
            return queues;
        }

        public async Task<IList<ITopic>> GetTopics()
        {
            var topicDescriptions = await _managementClient.GetTopicsAsync();
            var topics = topicDescriptions.Select(description => new Topic(description.Path, description.UserMetadata))
                .ToList<ITopic>();
            return topics;
        }
    }
}