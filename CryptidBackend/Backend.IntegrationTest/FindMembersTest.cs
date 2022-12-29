using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Cryptid.Backend;

namespace Backend.IntegrationTest
{
    public class FindMembersTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public FindMembersTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public static IEnumerable<object[]> GetPlayers()
        {
            yield return new object[] { "1", "2", "3" };
        }

        [Theory]
        [MemberData(nameof(GetPlayers))]
        public void TestMatchmakingService(string p1, string p2, string p3)
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                MatchmakingService mmService = scope.ServiceProvider.GetRequiredService<MatchmakingService>();
                mmService.AddPlayerToMatchmaking(p1);
                mmService.AddPlayerToMatchmaking(p2);
                mmService.AddPlayerToMatchmaking(p3);

                mmService.MatchPlayers();

                Assert.Equal(mmService.GetPlayersInQueue().Count, 1);
            }
        }
    }
}