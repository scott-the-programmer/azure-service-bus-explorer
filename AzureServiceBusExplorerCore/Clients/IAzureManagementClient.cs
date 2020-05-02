using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Clients
{
    public interface IAzureManagementClient
    {
        Task<IList<QueueDescription>> GetQueuesAsync();
    }
}