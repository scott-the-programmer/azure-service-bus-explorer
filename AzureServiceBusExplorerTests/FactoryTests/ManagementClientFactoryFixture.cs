using AzureServiceBusExplorerCore.Factories;
using NUnit.Framework;

namespace AzureServiceBusExplorerTests.FactoryTests
{
    [TestFixture]
    public class ManagementClientFactoryFixture
    {
        [Test]
        public void should_initiate_management_factory()
        {
            Assert.DoesNotThrow(()=>
            {
                var managementClientFactory = new ManagementClientFactory("mock");
                Assert.IsNotNull(managementClientFactory);
            });                
        }
    }
}