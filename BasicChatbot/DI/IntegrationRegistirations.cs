using BasicChatbot.Integrations.Groq;

namespace BasicChatbot.API.DI;

public static class IntegrationRegistirations
{
	public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
	{
		services.AddHttpClient();
		services.AddScoped<GroqServiceClient>();

		return services;
	}
}
