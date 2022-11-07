using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static System.Net.Mime.MediaTypeNames;
using System;
using Microsoft.Extensions.DependencyInjection;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Backend.Controllers;

namespace Backend.IntegrationTest
{
    public class FindMembersTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private HomeController _controller;
        private Mock<IUsersRepository> _usersMockRepository;
        private readonly WebApplicationFactory<Program> _factory;

        public FindMembersTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IUsersRepository, UsersRepository>();
                });
            });

            _usersMockRepository = new Mock<IUsersRepository>();
            _controller = new HomeController(_usersMockRepository.Object);
        }

        [Theory]
        [InlineData("Test")]
        public void GetUserDataTest(string name)
        {
            //Arrange
            _usersMockRepository.Setup(repo => repo.GetUserData(name))
                .Returns("Test");

            //Act
            var result = _controller.GetUserData(name);

            //Assert
            Assert.IsType<string>(result);
            Assert.Equal(result, name);
        }


        private string GetUserData()
        {
            string name = "Test";
            return name;
        }
    }
}