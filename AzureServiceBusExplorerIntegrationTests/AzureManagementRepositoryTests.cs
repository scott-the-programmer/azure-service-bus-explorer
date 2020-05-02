using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace AzureServiceBusExplorerIntegrationTests
{
    [Category("Integration")]
    public class AzureManagementRepositoryTests
    {
        private IManagementClientFactory _managementClientFactory;
        
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("integration_settings.json")
                .Build();

            _managementClientFactory = new ManagementClientFactory(config["ServiceBusConnection"]);
        }

        [Test]
        public async Task should_return_list_of_queues()
        {
            //Setup
            AzureManagementRepository repo = new AzureManagementRepository(_managementClientFactory);
            
            //Act
            var queues = await repo.GetQueues();

            //Assert
            Assert.IsNotNull(queues);
        }
    }
}