using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Clients
{
    [ExcludeFromCodeCoverage] //Real interactions with Azure
    public class AzureManagementClient : IAzureManagementClient
    {
        private readonly ManagementClient _managementClient;

        public AzureManagementClient(string connection)
        {
            _managementClient = new ManagementClient(connection);
        }

        public Task<IList<QueueDescription>> GetQueuesAsync()
        {
            return _managementClient.GetQueuesAsync();
        }

        public Task<IList<TopicDescription>> GetTopicsAsync()
        {
            return _managementClient.GetTopicsAsync();
        }

        public Task CreateQueueAsync(QueueDescription queueDescription)
        {
            return _managementClient.CreateQueueAsync(queueDescription);
        }

        public async Task DeleteQueueAsync(string queueName)
        {
            await _managementClient.DeleteQueueAsync(queueName);
        }

        public Task CreateTopicAsync(TopicDescription topicDescription)
        {
            return _managementClient.CreateTopicAsync(topicDescription);
        }

        public Task CreateTopicSubscription(SubscriptionDescription subscriptionDescription)
        {
            return _managementClient.CreateSubscriptionAsync(subscriptionDescription);
        }

        public async Task DeleteTopicAsync(string topicName)
        {
            await _managementClient.DeleteTopicAsync(topicName);
        }
    }
}