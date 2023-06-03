using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Writing.Controllers; 

[ApiController]
public class RedisDemoController : Controller {

    private readonly IDistributedCache distributedCache;

    public RedisDemoController(IDistributedCache distributedCache) {
        this.distributedCache = distributedCache;
    }
    
    public class ResponseRedis {
        public string Key { get; set; }
        public string value { get; set; }
    }

    [HttpGet]
    [Route("/api/redis/demo")]
    public string getTime() {
        string key = "Caching time";
        string cachingTime = distributedCache.GetString(key);

        if (string.IsNullOrEmpty(cachingTime)) {
            cachingTime = DateTime.Now.ToString();
            distributedCache.SetString(key, cachingTime);
            return "Add to cache: " + cachingTime;
        }
        
        return "Caching time: " + cachingTime;
    }
}