namespace AzureServiceBusExplorerCore.Models
{
    public class Queue
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