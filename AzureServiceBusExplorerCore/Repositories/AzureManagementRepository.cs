using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusExplorerCore.Clients;
using AzureServiceBusExplorerCore.Factories;
using AzureServiceBusExplorerCore.Models;
using AzureServiceBusExplorerCore.Models.Interfaces;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBusExplorerCore.Repositories
{
    public class AzureManagementRepository
    {
        private readonly IAzureManagementClient _azureManagementClient;

        public AzureManagementRepository(IManagementClientFactory managementClientFactory)
        {
            _azureManagementClient = managementClientFactory.GetManagementClient();
        }

        public async Task<IList<IQueue>> GetQueues()
        {
            var queueDescriptions = await _azureManagementClient.GetQueuesAsync();
            var queues = queueDescriptions.Select(description => new Queue(description.Path, description.UserMetadata))
                .ToList<IQueue>();
            return queues;
        }
    }
}