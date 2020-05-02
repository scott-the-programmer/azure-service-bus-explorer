using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Azure.ServiceBus.Management;
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
    }
}