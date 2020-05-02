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
    [TestFixture]
    public class AzureManagementRepositoryFixture
    {
        [Test]
        public async Task should_return_a_queue_object()
        {
            //Setup
            IList<QueueDescription> mockResult = new List<QueueDescription>();
            mockResult.Add(new QueueDescription("path") {UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetQueuesAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            var queues = await repo.GetQueuesAsync();

            //Assert
            Assert.AreEqual(1, queues.Count);
        }

        [Test]
        public async Task should_return_10_queue_objects()
        {
            //Setup
            IList<QueueDescription> mockResult = new List<QueueDescription>();
            for (int i = 0; i < 10; i++)
                mockResult.Add(new QueueDescription("path") {UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetQueuesAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            var queues = await repo.GetQueuesAsync();

            //Assert
            Assert.AreEqual(10, queues.Count);
        }

        [Test]
        public async Task should_have_correct_queue_details()
        {
            //Setup
            IList<QueueDescription> mockResult = new List<QueueDescription>();
            mockResult.Add(new QueueDescription("path") {UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetQueuesAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            var queues = await repo.GetQueuesAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, queues.Count);
                Assert.AreEqual("path", queues[0].QueueName);
                Assert.AreEqual("metadata", queues[0].QueueDescription);
            });
        }

        [Test]
        public async Task should_return_a_topic_object()
        {
            //Setup
            IList<TopicDescription> mockResult = new List<TopicDescription>();
            mockResult.Add(new TopicDescription("path") {UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetTopicsAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            var topics = await repo.GetTopicsAsync();

            //Assert
            Assert.AreEqual(1, topics.Count);
        }

        [Test]
        public async Task should_return_10_topic_objects()
        {
            //Setup
            IList<TopicDescription> mockResult = new List<TopicDescription>();
            for (int i = 0; i < 10; i++)
                mockResult.Add(new TopicDescription("path") {UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetTopicsAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            var topics = await repo.GetTopicsAsync();

            //Assert
            Assert.AreEqual(10, topics.Count);
        }

        [Test]
        public async Task should_have_correct_topic_details()
        {
            //Setup
            IList<TopicDescription> mockResult = new List<TopicDescription>();
            mockResult.Add(new TopicDescription("path") {UserMetadata = "metadata"});
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.GetTopicsAsync()).ReturnsAsync(mockResult);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            var topics = await repo.GetTopicsAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, topics.Count);
                Assert.AreEqual("path", topics[0].TopicName);
                Assert.AreEqual("metadata", topics[0].TopicDescription);
            });
        }

        [Test]
        public async Task should_create_new_topic()
        {
            //Setup
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.CreateTopicAsync(It.IsAny<TopicDescription>()))
                .Returns(Task.CompletedTask);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            await repo.CreateTopicAsync(new TopicDescription("mock"));

            //Assert
            managementClientMock.Verify(mock => mock.CreateTopicAsync(It.IsAny<TopicDescription>()),
                Times.Once());
        }

        [Test]
        public async Task should_delete_topic()
        {
            //Setup
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.DeleteTopicIfExistsAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            await repo.DeleteTopicAsync("mock");

            //Assert
            managementClientMock.Verify(mock => mock.DeleteTopicIfExistsAsync(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public async Task should_create_new_queue()
        {
            //Setup
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.CreateQueueAsync(It.IsAny<QueueDescription>()))
                .Returns(Task.CompletedTask);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            await repo.CreateQueueAsync(new QueueDescription("mock"));

            //Assert
            managementClientMock.Verify(mock => mock.CreateQueueAsync(It.IsAny<QueueDescription>()),
                Times.Once());
        }

        [Test]
        public async Task should_delete_queue()
        {
            //Setup
            Mock<IAzureManagementClient> managementClientMock = new Mock<IAzureManagementClient>();
            Mock<IManagementClientFactory> managementClientFactoryMock = new Mock<IManagementClientFactory>();
            managementClientMock.Setup(mock => mock.DeleteQueueIfExistsAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            managementClientFactoryMock.Setup(mock => mock.GetManagementClient()).Returns(managementClientMock.Object);

            AzureManagementRepository repo = new AzureManagementRepository(managementClientFactoryMock.Object);

            //Act
            await repo.DeleteQueueAsync("mock");

            //Assert
            managementClientMock.Verify(mock => mock.DeleteQueueIfExistsAsync(It.IsAny<string>()), Times.Once());
        }
    }
}