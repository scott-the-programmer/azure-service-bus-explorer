using AzureServiceBusExplorerCore.Models.Interfaces;

namespace AzureServiceBusExplorerCore.Models
{
    public class Queue : IQueue
    {
        public Queue(string name, string description)
        {
            QueueName = name;
            QueueDescription = description;
        }

        public string QueueName { get; set; }
        public string QueueDescription { get; set; }
    }
}