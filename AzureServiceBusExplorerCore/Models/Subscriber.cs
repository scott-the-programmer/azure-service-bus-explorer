
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Models
{
    public class Subscriber : SubscriptionDescription
    {
        public Subscriber(string topicName, string subscriptionName) : base(topicName, subscriptionName)
        {
        }

    }
}