using BasicChatbot.Service.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace BasicChatbot.Service.Services.Redis;

public class RedisService(IConnectionMultiplexer connectionMultiplexer, int databaseIndex = 0) : IRedisService
{
	private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
	private readonly IDatabase _database = connectionMultiplexer.GetDatabase(databaseIndex);
	private readonly int _databaseIndex = databaseIndex;

	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = false
	};

	/// <inheritdoc />
	public async Task<T?> GetAsync<T>(string key)
	{
		var value = await _database.StringGetAsync(key);

		if (value.IsNullOrEmpty)
			return default;

		return JsonSerializer.Deserialize<T>(value!, _jsonOptions);
	}

	/// <inheritdoc />
	public async Task<string?> GetStringAsync(string key)
	{
		var value = await _database.StringGetAsync(key);

		return value.IsNullOrEmpty ? null : value.ToString();
	}

	/// <inheritdoc />
	public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
	{
		var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);

		await _database.StringSetAsync(key, serializedValue, expiry);
	}

	/// <inheritdoc />
	public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
	{
		await _database.StringSetAsync(key, value, expiry);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteAsync(string key)
	{
		return await _database.KeyDeleteAsync(key);
	}

	/// <inheritdoc />
	public async Task<bool> ExistsAsync(string key)
	{
		return await _database.KeyExistsAsync(key);
	}

	/// <inheritdoc />
	public async Task<Dictionary<string, string?>> GetAllAsync()
	{
		var result = new Dictionary<string, string?>();
		var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());

		await foreach (var key in server.KeysAsync(database: _databaseIndex))
		{
			var value = await _database.StringGetAsync(key);
			result[key.ToString()] = value.IsNullOrEmpty ? null : value.ToString();
		}

		return result;
	}
}