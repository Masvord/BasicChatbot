using BasicChatbot.Integrations.Groq;
using BasicChatbot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicChatbot.API.Controllers;

public class BaseController(IServiceProvider serviceProvider) : ControllerBase
{
	#region [ Service Clients ]

	protected readonly GroqServiceClient _groqServiceClient = serviceProvider.GetRequiredService<GroqServiceClient>();

	protected readonly IRedisService _redisServiceClient = serviceProvider.GetRequiredService<IRedisService>();

	#endregion
}
