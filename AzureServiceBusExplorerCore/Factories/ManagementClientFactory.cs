using System.Diagnostics.CodeAnalysis;
using AzureServiceBusExplorerCore.Clients;

namespace AzureServiceBusExplorerCore.Factories
{
    [ExcludeFromCodeCoverage] //Real interactions with Azure
    public class ManagementClientFactory : IManagementClientFactory
    {
        private readonly string _connection;
        
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