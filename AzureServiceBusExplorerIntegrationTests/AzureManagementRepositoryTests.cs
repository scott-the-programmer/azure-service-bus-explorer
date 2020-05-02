using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Models;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace AzureServiceBusExplorerIntegrationTests
{
    [Category("Integration")]
    public class AzureManagementRepositoryTests
    {
        private IManagementClientFactory _managementClientFactory;

        [SetUp]
        public async Task Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("integration_settings.json")
                .Build();

            _managementClientFactory = new ManagementClientFactory(config["ServiceBusConnection"]);

            var repo = new AzureManagementRepository(_managementClientFactory);
            await repo.DeleteQueueAsync("integration_test_queue");
            await repo.DeleteQueueAsync("integration_test_topic");
        }

        [Test]
        public async Task should_apply_crd_on_queue()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Create Queue
            await repo.CreateQueueAsync(new QueueDescription("integration_test_queue"));

            //Read Queue
            var queues = await repo.GetQueuesAsync();
            var queue = queues.Where(x => x.QueueName == "integration_test_queue");
            Assert.AreEqual(1, queue.Count());

            //Delete Queue
            await repo.DeleteQueueAsync("integration_test_queue");
        }

        [Test]
        public async Task can_delete_queue_that_does_not_exist()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Delete Queue
            await repo.DeleteQueueAsync("i_do_not_exist");
        }

        [Test]
        public async Task can_delete_topic_that_does_not_exist()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Delete Queue
            await repo.DeleteTopicAsync("i_do_not_exist");
        }


        [Test]
        public async Task should_apply_crd_on_topic()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Create Topic
            await repo.CreateTopicAsync(new Topic("integration_test_topic", "mock"));

            //Read Topic
            var topics = await repo.GetTopicsAsync();
            var queue = topics.Where(x => x.TopicName == "integration_test_topic");
            Assert.AreEqual(1, queue.Count());

            //Delete Topic
            await repo.DeleteTopicAsync("integration_test_topic");
        }


        [Test]
        public async Task can_create_subscriber_to_topic()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Create Topic
            var topic = new Topic("can_create_subscriber_to_topic", "mock");
            await repo.CreateTopicAsync(topic);

            //Create Subscriber
            var subscriber = new Subscriber("can_create_subscriber_to_topic", "can_create_subscriber_to_topic");
            await repo.CreateTopicSubscriptionAsync(topic, subscriber);

            //Delete Topic
            await repo.DeleteTopicAsync("can_create_subscriber_to_topic");
        }
    }
}