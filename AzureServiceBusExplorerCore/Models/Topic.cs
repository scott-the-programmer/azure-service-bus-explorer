using AzureServiceBusExplorerCore.Models.Interfaces;

namespace AzureServiceBusExplorerCore.Models
{
    public class Topic : ITopic
    {
        public Topic(string name, string description)
        {
            TopicName = name;
            TopicDescription = description;
        }

        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
    }
}