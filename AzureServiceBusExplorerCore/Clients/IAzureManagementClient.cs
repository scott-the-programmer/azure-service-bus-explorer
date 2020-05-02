using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Clients
{
    public interface IAzureManagementClient
    {
        Task<IList<QueueDescription>> GetQueuesAsync();
        Task<IList<TopicDescription>> GetTopicsAsync();
        Task CreateQueueAsync(string name, string metadata);
        Task CreateQueueAsync(QueueDescription queueDescription);
        Task DeleteQueueIfExistsAsync(string queueName);
        Task CreateTopicAsync(string name, string metadata);
        Task CreateTopicAsync(TopicDescription topicDescription);
        Task DeleteTopicIfExistsAsync(string topicName);
        
    }
}