namespace AzureServiceBusExplorerCore.Models.Interfaces
{
    public interface IMessage
    {
        public string MessageBody { get; set; }
        byte[] EncodeMessage();
    }
}