using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Clients
{
    public interface IAzureManagementClient
    {
        Task<IList<QueueDescription>> GetQueuesAsync();
        Task<IList<TopicDescription>> GetTopicsAsync();
        Task CreateQueueAsync(QueueDescription queueDescription);
        Task DeleteQueueIfExistsAsync(string queueName);
        Task CreateTopicAsync(TopicDescription topicDescriptionDescription);
        Task CreateTopicSubscription(SubscriptionDescription subscriptionDescription);
        Task DeleteTopicIfExistsAsync(string topicName);
    }
}