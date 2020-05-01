
using System.Collections.Generic;
using AzureServiceBusClient.Models;
using AzureServiceBusClient.Models.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.

namespace AzureServiceBusClient
 {
     public class AzureServiceBusClient
     {

         private readonly string _serviceBusConnectionString;
         private Dictionary<string,IQueueClient> _activeQueueClients = new Dictionary<string, IQueueClient>();
         
         public AzureServiceBusClient(string serviceBusConnectionString)
         {
             _serviceBusConnectionString = serviceBusConnectionString;
         }

         public IQueueClient GetQueueClient(IQueue queue)
         {
             if (_activeQueueClients.ContainsKey(queue.QueueName))
                 return _activeQueueClients[queue.QueueName];
             IQueueClient queueClient = new QueueClient(_serviceBusConnectionString, queue.QueueName);
             _activeQueueClients.Add(queue.QueueName,queueClient);
             return queueClient;
         }

         public IList<IMessage> GetMessages(IQueue queue)
         {
             IQueueClient queueClient = GetQueueClient(queue);
             
             queueClient.
         }
     }
 }