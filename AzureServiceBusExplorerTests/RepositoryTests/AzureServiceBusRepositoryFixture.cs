using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Repositories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Moq;
using NUnit.Framework;

namespace AzureServiceBusExplorerTests.RepositoryTests
{
    [TestFixture]
    public class AzureServiceBusRepositoryFixture
    {
        private AzureServiceBusRepository _repo;

        [Test]
        public void should_initialize_an_internal_queue()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            _repo.GetQueueClient(new QueueDescription("mock"));

            //Assert
            Assert.AreEqual(1, _repo.CountActiveQueueClients());
        }

        [Test]
        public void should_not_reuse_existing_queue_if_queue_name_differs()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            _repo.GetQueueClient(new QueueDescription("mock"));
            _repo.GetQueueClient(new QueueDescription("mock2"));

            //Assert
            Assert.AreEqual(2, _repo.CountActiveQueueClients());
        }

        [Test]
        public void should_reuse_existing_internal_queue()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            _repo.GetQueueClient(new QueueDescription("mock"));
            _repo.GetQueueClient(new QueueDescription("mock"));

            //Assert
            Assert.AreEqual(1, _repo.CountActiveQueueClients());
        }
    }
}