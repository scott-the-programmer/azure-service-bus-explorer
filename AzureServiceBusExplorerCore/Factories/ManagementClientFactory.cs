using AzureServiceBusExplorerCore.Clients;

namespace AzureServiceBusExplorerCore.Factories
{
    public class ManagementClientFactory : IManagementClientFactory
    {
        private string _connection;
        
        public ManagementClientFactory(string connection)
        {
            _connection = connection;
        }
        
        public IAzureManagementClient GetManagementClient()
        {
            return new AzureManagementClient(_connection);
        }
    }
}