using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cryptid.Backend.Repositories
{
    public class CacheRepository
    {
        public const string WAITING_LIST_KEY = "waiting_players";

        private readonly MemoryCacheEntryOptions cacheEntryOptions;
        private readonly IMemoryCache memoryCache;

        public List<string> WaitingPlayers
        {
            get
            {
                waitingPlayers = memoryCache.Get<List<string>>(WAITING_LIST_KEY);
                if (waitingPlayers == null)
                {
                    waitingPlayers = new List<string>();
                    memoryCache.Set(WAITING_LIST_KEY, waitingPlayers, cacheEntryOptions);
                }

                return waitingPlayers;
            }
            set
            {
                waitingPlayers = value;
            }
        }

        private List<string> waitingPlayers;

        public CacheRepository(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.waitingPlayers = new List<string>();
            this.cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(1024);
        }

        void SaveCache()
        {
            memoryCache.Set(WAITING_LIST_KEY, waitingPlayers, cacheEntryOptions);
        }

        internal void AddPlayerToMatchmaking(string id)
        {
            WaitingPlayers.Add(id);
            SaveCache();
        }

        internal void RemovePlayerMatchmaking(string id)
        {
            WaitingPlayers.Remove(id);
            SaveCache();
        }
    }
}
