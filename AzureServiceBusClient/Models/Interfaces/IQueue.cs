namespace AzureServiceBusClient.Models.Interfaces
{
    public interface IQueue
    {
        public string QueueName { get; set; }
        public string QueueDescription { get; set; }
    }
}