using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        [Test]
        public async Task should_return_single_message()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock =>
                mock.RegisterMessageHandler(It.IsAny<Func<Message, CancellationToken, Task>>(),
                    It.IsAny<MessageHandlerOptions>()));
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var taskHandle = _repo.GetMessages(new QueueDescription("mock"), 1);

            //Mock Messages
            var messageState = _repo.GetMessageState();
            messageState.AddMessage("abc");

            //Act
            var messages = await taskHandle;

            //Assert
            Assert.Multiple(
                () =>
                {
                    Assert.AreEqual(1, messages.Count);
                    Assert.AreEqual("abc", messages[0]);
                });
        }
        
        [Test]
        public async Task should_close_client_after_processing()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock =>
                mock.RegisterMessageHandler(It.IsAny<Func<Message, CancellationToken, Task>>(),
                    It.IsAny<MessageHandlerOptions>()));
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var taskHandle = _repo.GetMessages(new QueueDescription("mock"), 1);

            //Mock Messages
            var messageState = _repo.GetMessageState();
            messageState.AddMessage("abc");

            //Act
            await taskHandle;

            //Assert
            queueClientMock.Verify(mock => mock.CloseAsync(), Times.Once);
        }

        [Test]
        public void should_return_1000_messages()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock =>
                mock.RegisterMessageHandler(It.IsAny<Func<Message, CancellationToken, Task>>(),
                    It.IsAny<MessageHandlerOptions>()));
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var taskHandle = _repo.GetMessages(new QueueDescription("mock"), 1000);

            //Mock Messages
            var messageState = _repo.GetMessageState();

            for (int i = 0; i < 1000; i++)
                messageState.AddMessage("abc");

            //Act
            var messages = taskHandle.Result;

            //Assert
            Assert.AreEqual(1000, messages.Count);
        }

        [Test]
        public async Task should_time_out_trying_to_get_messages()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock =>
                mock.RegisterMessageHandler(It.IsAny<Func<Message, CancellationToken, Task>>(),
                    It.IsAny<MessageHandlerOptions>()));
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var messages = await _repo.GetMessages(new QueueDescription("mock"), 1000, 2);

            //Assert
            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public void should_dev_null_exception()
        {
            //Setup
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act & Assert
            Assert.DoesNotThrow(() =>
            {
                AzureServiceBusRepository.MessagePumpExceptionHandler(new ExceptionReceivedEventArgs(new Exception("mock"), "mock",
                    "mock", "mock", "mock"));
            });
        }
        
        
        [Test]
        public async Task should_add_message_to_state()
        {
            //Setup
            var mockMessage = new Message(Encoding.UTF8.GetBytes("test"));
            var messageState = new AzureServiceBusRepository.MessageState(1);
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            await AzureServiceBusRepository.MessagePumpMessageHandler(mockMessage, messageState, new CancellationToken());
            
            //Assert
            Assert.AreEqual("test",messageState.GetMessages()[0]);
        }
    }
}