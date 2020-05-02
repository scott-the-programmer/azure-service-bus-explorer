using System.Collections.Generic;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Models;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Clients
{
    public interface IAzureManagementClient
    {
        Task<IList<QueueDescription>> GetQueuesAsync();
        Task<IList<TopicDescription>> GetTopicsAsync();
        Task CreateQueueAsync(QueueDescription queueDescription);
        Task DeleteQueueIfExistsAsync(string queueName);
        Task CreateTopicAsync(Topic topicDescription);
        Task CreateTopicSubscription(Subscriber subscriber);
        Task DeleteTopicIfExistsAsync(string topicName);
    }
}