using AzureServiceBusExplorerCore.Models.Interfaces;

namespace AzureServiceBusExplorerCore.Models
{
    public class Message : IMessage
    {
        public string MessageBody { get; set; }
        public byte[] EncodeMessage()
        {
            throw new System.NotImplementedException();
        }
    }
}