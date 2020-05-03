using System;
using System.Collections.Generic;
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
        public async Task should_add_message_to_state()
        {
            //Setup
            var mockMessage = new Message(Encoding.UTF8.GetBytes("test"));
            var messageState = new AzureServiceBusRepository.MessageState(1);
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            await AzureServiceBusRepository.MessagePumpReadHandler(mockMessage, messageState);

            //Assert
            Assert.AreEqual("test", messageState.GetMessages()[0]);
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
            queueClientMock.Setup(mock => mock.Path).Returns("Mock");
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var taskHandle = _repo.GetMessagesAsync(new QueueDescription("mock"), 1);

            //Mock Messages
            var messageState = _repo.GetMessageState();
            messageState.AddMessage("abc");

            //Act
            await taskHandle;

            //Assert
            queueClientMock.Verify(mock => mock.CloseAsync(), Times.Once);
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
                AzureServiceBusRepository.MessagePumpExceptionHandler(new ExceptionReceivedEventArgs(
                    new Exception("mock"), "mock",
                    "mock", "mock", "mock"));
            });
        }

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
        public void should_return_1000_messages()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock =>
                mock.RegisterMessageHandler(It.IsAny<Func<Message, CancellationToken, Task>>(),
                    It.IsAny<MessageHandlerOptions>()));
            queueClientMock.Setup(mock => mock.Path).Returns("Mock");
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var taskHandle = _repo.GetMessagesAsync(new QueueDescription("mock"), 1000);

            //Mock Messages
            var messageState = _repo.GetMessageState();

            for (var i = 0; i < 1000; i++)
                messageState.AddMessage("abc");

            //Act
            var messages = taskHandle.Result;

            //Assert
            Assert.AreEqual(1000, messages.Count);
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
            queueClientMock.Setup(mock => mock.Path).Returns("Mock");
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);


            //Act
            var taskHandle = _repo.GetMessagesAsync(new QueueDescription("mock"), 1);

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
        public async Task should_send_message()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock =>
                mock.SendAsync(It.IsAny<IList<Message>>())).Returns(Task.CompletedTask);
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            var mockMessages = new List<Message> {new Message(Encoding.UTF8.GetBytes("test"))};

            //Act
            await _repo.SendMessagesAsync(new QueueDescription("mock"), mockMessages);

            //Assert
            queueClientMock.Verify(mock => mock.SendAsync(It.IsAny<IList<Message>>()), Times.Once);
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
            queueClientMock.Setup(mock => mock.Path).Returns("Mock");
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            _repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            var messages = await _repo.GetMessagesAsync(new QueueDescription("mock"), 1000, 2);

            //Assert
            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public async Task should_delete_message_in_service_pump()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock => mock.CompleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            queueClientMock.Setup(mock => mock.CloseAsync()).Returns(Task.CompletedTask);
            queueClientMock.Setup(mock => mock.Path).Returns("Mock");
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            var messageToDelete = new Message {MessageId = "1"};
            var incomingMessage = new Message {MessageId = "1"};
            var completed = true;
            
            var repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            await repo.MessagePumpDeleteHandler(queueClientMock.Object, incomingMessage,
                messageToDelete,
                ref completed);

            //Assert
            queueClientMock.Verify(mock => mock.CompleteAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task should_not_delete_message_if_not_found_in_service_pump()
        {
            //Setup
            var queueClientMock = new Mock<IQueueClient>();
            var queueClientFactoryMock = new Mock<IQueueClientFactory>();
            queueClientMock.Setup(mock => mock.CompleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            queueClientMock.Setup(mock => mock.CloseAsync()).Returns(Task.CompletedTask);
            queueClientFactoryMock.Setup(mock => mock.GetQueueClient(It.IsAny<string>()))
                .Returns(queueClientMock.Object);
            
            var messageToDelete = new Message {MessageId = "1"};
            var incomingMessage = new Message {MessageId = "2"};
            var completed = true;
            var repo = new AzureServiceBusRepository(queueClientFactoryMock.Object);

            //Act
            await repo.MessagePumpDeleteHandler(queueClientMock.Object, incomingMessage,
                messageToDelete,
                ref completed);

            //Assert
            queueClientMock.Verify(mock => mock.CompleteAsync(It.IsAny<string>()), Times.Never);
        }
    }
}