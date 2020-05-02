using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace AzureServiceBusExplorerIntegrationTests
{
    [Category("Integration")]
    public class AzureServiceBusRepositoryTests
    {
        private IQueueClientFactory _queueFactory;
        
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("integration_settings.json")
                .Build();
            
            _queueFactory = new QueueClientFactory(config["ServiceBusConnection"]);
        }
        //
        // [Test]
        // public void should_create_and_retrieve_queue_client()
        // {
        //     //Setup
        //     AzureServiceBusRepository repo = new AzureServiceBusRepository(_queueFactory);
        //
        //     repo.GetQueueClient("");
        // }
    }
}