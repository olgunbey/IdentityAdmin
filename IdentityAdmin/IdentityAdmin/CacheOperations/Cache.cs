using ServiceStack.Redis;

namespace IdentityAdmin.CacheOperations
{
    public class Cache
    {
        private readonly IRedisClientAsync _redisClientAsync;
        public Cache(IRedisClientAsync redisClientAsync)
        {
            _redisClientAsync = redisClientAsync;
        }
        public Task<bool> SetCache(string key, string value)
        {
            return _redisClientAsync.SetAsync(key, value);
        }
        public async Task<string> GetCache(string key)
        {
            return await _redisClientAsync.GetAsync<string>(key);
        }
    }
}
