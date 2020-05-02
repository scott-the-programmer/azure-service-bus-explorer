using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBusExplorerCore.Factories
{
    [ExcludeFromCodeCoverage] //Real interactions with Azure
    public class QueueClientFactory : IQueueClientFactory
    {
        private readonly string _connection;
        
        public QueueClientFactory(string connection)
        {
            _connection = connection;
        }
        
        
        public IQueueClient GetQueueClient(string queueName)
        {
            return new QueueClient(_connection, queueName);
        }
    }
}