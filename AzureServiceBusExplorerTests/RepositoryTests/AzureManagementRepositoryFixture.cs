using System.Collections.Generic;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Clients;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Azure.ServiceBus.Management;
using Moq;
using NUnit.Framework;

namespace AzureServiceBusExplorerTests.RepositoryTests
{
    public class AzureManagementRepositoryFixture
    {
        [Test]
        public async Task should_return_a_queue_object()
        {
            //Setup
            IList<QueueDescription> mockResult = new List<QueueDescription>();
            mockResult.Add(new QueueDescription("path"){UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetQueuesAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);
            
            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);
            
            //Act
            var queues = await repo.GetQueues();
            
            //Assert
            Assert.AreEqual(1,queues.Count);

        }
        
        [Test]
        public async Task should_return_10_queue_objects()
        {
            //Setup
            IList<QueueDescription> mockResult = new List<QueueDescription>();
            for(int i = 0; i < 10; i++)
                mockResult.Add(new QueueDescription("path"){UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetQueuesAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);
            
            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);
            
            //Act
            var queues = await repo.GetQueues();
            
            //Assert
            Assert.AreEqual(10,queues.Count);

        }
        
        [Test]
        public async Task should_have_correct_queue_details()
        {
            //Setup
            IList<QueueDescription> mockResult = new List<QueueDescription>();
            mockResult.Add(new QueueDescription("path"){UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetQueuesAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);
            
            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);
            
            //Act
            var queues = await repo.GetQueues();
            
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1,queues.Count);
                Assert.AreEqual("path", queues[0].QueueName);
                Assert.AreEqual("metadata", queues[0].QueueDescription);
            });
        }
    }
}