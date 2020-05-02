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
            
            AzureManagementRepository repo = new AzureManagementRepository(_managementClientFactory);
            await repo.DeleteQueueAsync("integration_test_queue");
            await repo.DeleteQueueAsync("integration_test_topic");
        }

        [Test]
        public async Task should_apply_crd_on_queue()
        {
            AzureManagementRepository repo = new AzureManagementRepository(_managementClientFactory);

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
            AzureManagementRepository repo = new AzureManagementRepository(_managementClientFactory);

            //Delete Queue
            await repo.DeleteQueueAsync("i_do_not_exist");
        }

        [Test]
        public async Task should_apply_crd_on_topic()
        {
            AzureManagementRepository repo = new AzureManagementRepository(_managementClientFactory);

            //Create Queue
            await repo.CreateTopicAsync(new TopicDescription("integration_test_topic"));

            //Read Queue
            var topics = await repo.GetTopicsAsync();
            var queue = topics.Where(x => x.TopicName == "integration_test_topic");
            Assert.AreEqual(1, queue.Count());

            //Delete Queue
            await repo.DeleteTopicAsync("integration_test_topic");
        }
    }
}