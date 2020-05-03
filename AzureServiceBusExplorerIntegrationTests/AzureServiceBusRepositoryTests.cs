using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace AzureServiceBusExplorerIntegrationTests
{
    [Category("Integration")]
    public class AzureServiceBusRepositoryTests
    {
        private IQueueClientFactory _queueFactory;
        private AzureManagementRepository _managementRepository;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("integration_settings.json")
                .Build();

            _queueFactory = new QueueClientFactory(config["ServiceBusConnection"]);
            _managementRepository =
                new AzureManagementRepository(new ManagementClientFactory(config["ServiceBusConnection"]));
        }

        [Test]
        public async Task should_be_able_to_apply_crd_operations_on_queue_message()
        {
            
            var testIdentifier = "should_be_able_to_apply_crd_operations_on_queue_message";
            AzureServiceBusRepository repo = new AzureServiceBusRepository(_queueFactory);
            QueueDescription queue = new QueueDescription(testIdentifier);
            await _managementRepository.DeleteQueueIfExistsAsync(testIdentifier);
            await _managementRepository.CreateQueueAsync(queue);

            //Send Message
            var message = new Message(Encoding.UTF8.GetBytes(testIdentifier)) {MessageId = "1"};
            var messagesToSend = new List<Message> {message};
            await repo.SendMessagesAsync(queue, messagesToSend);

            //Read Message
            var messages = await repo.GetMessagesAsync(queue, 1);
            Assert.AreEqual(testIdentifier, messages[0]);

            //Delete Message
            await repo.DeleteMessageAsync(queue, message);

            //Clean up
            await _managementRepository.DeleteQueueIfExistsAsync(testIdentifier);
        }
    }
}