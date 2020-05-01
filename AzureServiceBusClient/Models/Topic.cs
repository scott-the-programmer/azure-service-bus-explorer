using AzureServiceBusClient.Models.Interfaces;

namespace AzureServiceBusClient.Models
{
    public class Topic : ITopic
    {
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }

        public Topic(string name, string description)
        {
            TopicName = name;
            TopicDescription = description;
        }
    }
}