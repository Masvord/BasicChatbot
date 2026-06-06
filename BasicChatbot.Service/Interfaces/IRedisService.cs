namespace BasicChatbot.Service.Interfaces;

public interface IRedisService
{
	/// <summary>
	/// Deserializes and returns the value stored in Redis for the specified key.
	/// </summary>
	Task<T?> GetAsync<T>(string key);

	/// <summary>
	/// Returns the string value associated with the specified key from Redis.
	/// </summary>
	Task<string?> GetStringAsync(string key);

	/// <summary>
	/// Serializes and stores the specified object as JSON for the specified key in Redis.
	/// </summary>
	Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

	/// <summary>
	/// Sets the specified string value for the specified key in Redis.
	/// </summary>
	Task SetStringAsync(string key, string value, TimeSpan? expiry = null);

	/// <summary>
	/// Remove the specified key and its associated value from Redis.
	/// </summary>
	Task<bool> DeleteAsync(string key);

	/// <summary>
	/// Control whether the specified key exists in Redis and return true if it does, otherwise false.
	/// </summary>
	Task<bool> ExistsAsync(string key);

	/// <summary>
	/// Returns a dictionary containing all key-value pairs stored in the Redis database, where keys are strings and values are nullable strings.
	/// </summary>
	Task<Dictionary<string, string?>> GetAllAsync();
}
