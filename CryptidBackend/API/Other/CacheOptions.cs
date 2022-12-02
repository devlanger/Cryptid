using Microsoft.Extensions.Caching.Memory;

namespace Cryptid.Backend
{
    public class CacheOptions
    {
        public static MemoryCacheEntryOptions MemoryCacheOptions
        {
            get

            {
                if (options == null)
                {
                    options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
                }
                return options;
            }
        }

        private static MemoryCacheEntryOptions options;
    }
}
