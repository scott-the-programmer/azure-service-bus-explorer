using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Clients;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Models;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Repositories
{
    public class AzureManagementRepository
    {
        private readonly IAzureManagementClient _azureManagementClient;

        public AzureManagementRepository(IManagementClientFactory managementClientFactory)
        {
            _azureManagementClient = managementClientFactory.GetManagementClient();
        }

        public async Task<IList<Queue>> GetQueuesAsync()
        {
            var queueDescriptions = await _azureManagementClient.GetQueuesAsync();
            var queues = queueDescriptions.Select(_ => new Queue(_.Path, _.UserMetadata))
                .ToList<Queue>();
            return queues;
        }
        public Task CreateQueueAsync(QueueDescription queueDescription)
        {
            return _azureManagementClient.CreateQueueAsync(queueDescription);
        }
        
        public Task DeleteQueueAsync(string queueName)
        {
            return _azureManagementClient.DeleteQueueIfExistsAsync(queueName);
        }
        
        public async Task<IList<Topic>> GetTopicsAsync()
        {
            var topicDescriptions = await _azureManagementClient.GetTopicsAsync();
            var topics = topicDescriptions.Select(_ => new Topic(_.Path, _.UserMetadata))
                .ToList<Topic>();
            return topics;
        }
        
        public Task CreateTopicAsync(TopicDescription topicDescription)
        {
            return _azureManagementClient.CreateTopicAsync(topicDescription);
        }

        public Task DeleteTopicAsync(string topicName)
        {
            return _azureManagementClient.DeleteTopicIfExistsAsync(topicName);
        }
    }
}