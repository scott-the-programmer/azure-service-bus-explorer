using System;
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

        public Task CreateTopicAsync(Topic topic)
        {
            return _azureManagementClient.CreateTopicAsync(topic);
        }

        public async Task CreateTopicSubscriptionAsync(Topic topic, Subscriber subscriber)
        {
            if (topic.Subscribers.Contains(subscriber))
            {
                throw new ArgumentException("This subscriber is already apart of the topic");
            }

            if (subscriber.TopicPath != topic.TopicName)
            {
                throw new ArgumentException(
                    $"The subscriber topic {subscriber.TopicPath} does not match the given topic ${topic.TopicName}");
            }

            await _azureManagementClient.CreateTopicSubscription(subscriber);

            topic.Subscribers.Add(subscriber);
        }

        public Task DeleteTopicAsync(string topicName)
        {
            return _azureManagementClient.DeleteTopicIfExistsAsync(topicName);
        }
    }
}