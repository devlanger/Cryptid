using Microsoft.AspNetCore.Mvc.Testing;
using Backend;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static System.Net.Mime.MediaTypeNames;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Backend.IntegrationTest
{
    public class ConnectionIntegrationUsingTestServerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private DefaultWebApplicationFactory app;

        public ConnectionIntegrationUsingTestServerTest(WebApplicationFactory<Program> factory)
        {
            app = new DefaultWebApplicationFactory();
        }

        [Theory]
        [InlineData("/Home/GetUserData?name=Tesx")]
        public async Task EnpointsReturnCorrectContentType(string url)
        {
            //Arrange
            var client = app.CreateClient();

            //Act
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string r = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal("Test", r);
        }
    }
}