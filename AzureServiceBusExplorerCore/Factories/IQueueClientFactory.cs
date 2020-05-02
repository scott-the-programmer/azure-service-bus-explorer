using Microsoft.Azure.ServiceBus;

namespace AzureServiceBusExplorerCore.Factories
{
    public interface IQueueClientFactory
    {
        IQueueClient GetQueueClient(string queueName);
    }
}