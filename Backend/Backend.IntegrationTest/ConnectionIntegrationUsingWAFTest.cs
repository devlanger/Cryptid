using Microsoft.AspNetCore.Mvc.Testing;
using Backend;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace Backend.IntegrationTest
{
    public class ConnectionIntegrationUsingWAFTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DefaultWebApplicationFactory app;


        public ConnectionIntegrationUsingWAFTest()
        {
            app = new DefaultWebApplicationFactory();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Privacy")]
        public async Task EnpointsReturnCorrectContentType(string url)
        {
            //Arrange
            var client = app.CreateClient();

            //Act
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}