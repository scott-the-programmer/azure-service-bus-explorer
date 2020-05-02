using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Factories;
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
            await repo.DeleteQueueIfExistsAsync("integration_test_queue");
            await repo.DeleteQueueIfExistsAsync("integration_test_topic");
        }

        [Test]
        public async Task should_apply_crd_on_queue()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Create Queue
            await repo.CreateQueueAsync(new QueueDescription("integration_test_queue"));

            //Read Queue
            var queues = await repo.GetQueuesAsync();
            var queue = queues.Where(x => x.Path == "integration_test_queue");
            Assert.AreEqual(1, queue.Count());

            //Delete Queue
            await repo.DeleteQueueIfExistsAsync("integration_test_queue");
        }

        [Test]
        public async Task can_delete_queue_that_does_not_exist()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Delete Queue
            await repo.DeleteQueueIfExistsAsync("i_do_not_exist");
        }

        [Test]
        public async Task can_delete_topic_that_does_not_exist()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Delete Queue
            await repo.DeleteTopicIfExistsAsync("i_do_not_exist");
        }


        [Test]
        public async Task should_apply_crd_on_topic()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Create Topic
            await repo.CreateTopicAsync(new TopicDescription("integration_test_topic"));

            //Read Topic
            var topics = await repo.GetTopicsAsync();
            var queue = topics.Where(x => x.Path == "integration_test_topic");
            Assert.AreEqual(1, queue.Count());

            //Delete Topic
            await repo.DeleteTopicIfExistsAsync("integration_test_topic");
        }


        [Test]
        public async Task can_create_subscriber_to_topic()
        {
            var repo = new AzureManagementRepository(_managementClientFactory);

            //Create Topic
            var topic = new TopicDescription("can_create_subscriber_to_topic");
            await repo.CreateTopicAsync(topic);

            //Create Subscriber
            var subscriber =
                new SubscriptionDescription("can_create_subscriber_to_topic", "can_create_subscriber_to_topic");
            await repo.CreateTopicSubscriptionAsync(subscriber);

            //Delete Topic
            await repo.DeleteTopicIfExistsAsync("can_create_subscriber_to_topic");
        }
    }
}