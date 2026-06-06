using BasicChatbot.Service.Interfaces;
using BasicChatbot.Service.Services.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BasicChatbot.Service.DI;

public static class ServiceRegistrations
{
	public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
	{
		#region [ Redis ]

		var redisHost = configuration["Redis:Host"]
			?? throw new InvalidOperationException("[ BasicChatbot.Service ] - [ ServiceRegistrations ] - Redis:Host configuration is missing!");

		var redisPort = configuration["Redis:Port"] ?? "6379";
		var redisPassword = configuration["Redis:Password"];

		var redisConnectionString = string.IsNullOrWhiteSpace(redisPassword)
			? $"{redisHost}:{redisPort}"
			: $"{redisHost}:{redisPort},password={redisPassword}";

		services.AddSingleton<IConnectionMultiplexer>(
			ConnectionMultiplexer.Connect(redisConnectionString));

		var redisDatabaseIndex = int.TryParse(configuration["Redis:DatabaseIndex"], out var dbIndex)
			? dbIndex
			: 1;

		services.AddScoped<IRedisService>(sp =>
			new RedisService(sp.GetRequiredService<IConnectionMultiplexer>(), redisDatabaseIndex));

		#endregion

		return services;
	}
}
