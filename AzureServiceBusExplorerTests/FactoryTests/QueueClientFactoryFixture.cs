using AzureServiceBusExplorerCore.Factories;
using NUnit.Framework;

namespace AzureServiceBusExplorerTests.FactoryTests
{
    [TestFixture]
    public class QueueClientFactoryFixture
    {
        [Test]
        public void should_initiate_queue_factory()
        {
            Assert.DoesNotThrow(()=>
            {
                var queueClientFactory = new QueueClientFactory("mock");
                Assert.IsNotNull(queueClientFactory);
            });                
        }
    }
}