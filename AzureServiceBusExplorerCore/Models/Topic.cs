namespace AzureServiceBusExplorerCore.Models
{
    public class Topic 
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