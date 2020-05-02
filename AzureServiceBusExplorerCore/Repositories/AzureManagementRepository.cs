using System.Collections.Generic;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Clients;
using AzureServiceBusExplorerCore.Factories;
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

        public Task<IList<QueueDescription>> GetQueuesAsync()
        {
            return _azureManagementClient.GetQueuesAsync();
        }

        public Task CreateQueueAsync(QueueDescription queueDescription)
        {
            return _azureManagementClient.CreateQueueAsync(queueDescription);
        }

        public Task DeleteQueueAsync(string queueName)
        {
            return _azureManagementClient.DeleteQueueIfExistsAsync(queueName);
        }

        public Task<IList<TopicDescription>> GetTopicsAsync()
        {
            return _azureManagementClient.GetTopicsAsync();
        }

        public Task CreateTopicAsync(TopicDescription topicDescription)
        {
            return _azureManagementClient.CreateTopicAsync(topicDescription);
        }

        public async Task CreateTopicSubscriptionAsync(SubscriptionDescription subscriptionDescription)
        {
            await _azureManagementClient.CreateTopicSubscription(subscriptionDescription);
        }

        public Task DeleteTopicAsync(string topicName)
        {
            return _azureManagementClient.DeleteTopicIfExistsAsync(topicName);
        }
    }
}