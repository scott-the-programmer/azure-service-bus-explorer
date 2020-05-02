using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Clients
{
    public class AzureManagementClient : IAzureManagementClient
    {
        private readonly ManagementClient _managementClient;
        
        public AzureManagementClient(string connection)
        {
            _managementClient = new ManagementClient(connection);
        }
        
        public Task<IList<QueueDescription>> GetQueuesAsync()
        {
            return _managementClient.GetQueuesAsync();
        }
        
        public Task<IList<TopicDescription>> GetTopicsAsync()
        {
            return _managementClient.GetTopicsAsync();
        }
    }
}