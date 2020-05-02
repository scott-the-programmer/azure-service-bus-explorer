using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Clients;
using AzureServiceBusExplorerCore.Factories;
using Microsoft.Azure.ServiceBus;
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

        public async Task DeleteQueueIfExistsAsync(string queueName)
        {
            try
            {
                await _azureManagementClient.DeleteQueueAsync(queueName);
            }
            catch (MessagingEntityNotFoundException)
            {
                Console.WriteLine($"Queue {queueName} was not found");
            }
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

        public async Task DeleteTopicIfExistsAsync(string topicName)
        {
            try
            {
                await _azureManagementClient.DeleteTopicAsync(topicName);
            }
            catch (MessagingEntityNotFoundException)
            {
                Console.WriteLine($"Topic {topicName} was not found");
            }
        }
    }
}