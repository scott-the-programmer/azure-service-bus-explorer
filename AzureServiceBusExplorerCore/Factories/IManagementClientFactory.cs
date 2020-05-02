using AzureServiceBusExplorerCore.Clients;

namespace AzureServiceBusExplorerCore.Factories
{
    public interface IManagementClientFactory
    {
        IAzureManagementClient GetManagementClient();
    }
}