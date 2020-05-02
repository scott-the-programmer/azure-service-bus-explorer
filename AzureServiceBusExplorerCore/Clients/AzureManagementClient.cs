using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
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

        public Task CreateQueueAsync(string name, string metadata)
        {
            var queueDescription = new QueueDescription(name) {UserMetadata = metadata};
            return CreateQueueAsync(queueDescription);
        }
        
        public Task CreateQueueAsync(QueueDescription queueDescription){
            return _managementClient.CreateQueueAsync(queueDescription);
        }

        public async Task DeleteQueueIfExistsAsync(string queueName)
        {
            try
            {
                await _managementClient.DeleteQueueAsync(queueName);
            }
            catch (MessagingEntityNotFoundException)
            {
                Console.WriteLine($"Topic {queueName} was not found");
            }
        }

        public Task CreateTopicAsync(string name, string metadata)
        {
            var topicDescription = new TopicDescription(name) {UserMetadata = metadata};
            return CreateTopicAsync(topicDescription);
        }
        
        public Task CreateTopicAsync(TopicDescription topicDescription){
            return _managementClient.CreateTopicAsync(topicDescription);
        }

        public async Task DeleteTopicIfExistsAsync(string topicName)
        {
            try
            {
                await _managementClient.DeleteTopicAsync(topicName);
            }
            catch (MessagingEntityNotFoundException)
            {
                Console.WriteLine($"Topic {topicName} was not found");
            }
        }
    }
}