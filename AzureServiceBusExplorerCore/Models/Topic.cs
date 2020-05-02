using System.Collections.Generic;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Models
{
    public class Topic : TopicDescription
    {
        public Topic(string name, string description) : base(name)
        {
            TopicName = name;
            TopicDescription = description;
            Subscribers = new List<Subscriber>();
        }

        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public IList<Subscriber> Subscribers { get; set; }
    }
}