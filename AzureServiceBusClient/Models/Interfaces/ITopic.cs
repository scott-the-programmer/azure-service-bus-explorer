namespace AzureServiceBusClient.Models.Interfaces
{
    public interface ITopic
    {
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
    }
}