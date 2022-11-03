using Microsoft.AspNetCore.Mvc.Testing;
using Backend;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Backend.IntegrationTest
{
    public class SomeTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SomeTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                   
                });
            });
        }

        [Fact]
        public void Test1()
        {

        }
    }
}