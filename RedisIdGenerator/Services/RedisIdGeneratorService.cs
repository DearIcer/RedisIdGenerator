using StackExchange.Redis;

namespace RedisIdGenerator.Services;


public interface IRedisIdGeneratorService
{
    Task<long> GetNextIdAsync(string key);
    Task<long> GetCurrentIdAsync(string key);
    Task ResetIdAsync(string key, long startValue = 0);
}

public class RedisIdGeneratorService : IRedisIdGeneratorService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisIdGeneratorService(IConnectionMultiplexer redis)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _db = _redis.GetDatabase();
    }

    /// <summary>
    /// 获取下一个ID（原子自增）
    /// </summary>
    /// <param name="key">ID键名</param>
    /// <returns>自增后的ID值</returns>
    public async Task<long> GetNextIdAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("键名不能为空", nameof(key));

        return await _db.StringIncrementAsync(key);
    }

    /// <summary>
    /// 获取当前ID值（不自增）
    /// </summary>
    /// <param name="key">ID键名</param>
    /// <returns>当前ID值，如果不存在则返回0</returns>
    public async Task<long> GetCurrentIdAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("键名不能为空", nameof(key));

        var value = await _db.StringGetAsync(key);
        return value.HasValue ? (long)value : 0;
    }

    /// <summary>
    /// 重置ID值
    /// </summary>
    /// <param name="key">ID键名</param>
    /// <param name="startValue">起始值，默认为0</param>
    /// <returns></returns>
    public async Task ResetIdAsync(string key, long startValue = 0)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("键名不能为空", nameof(key));

        await _db.StringSetAsync(key, startValue);
    }
}