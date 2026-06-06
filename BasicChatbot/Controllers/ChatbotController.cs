using BasicChatbot.Models.Requests;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace BasicChatbot.API.Controllers;

[Route("api")]
[ApiController]
public class ChatbotController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
	[HttpPost]
	[Route("chat")]
	public async Task<IActionResult> PostMessage([FromBody] UserInput input)
	{
		#region [ Validation ]

		if (string.IsNullOrWhiteSpace(input.Message))
			return BadRequest("Mesaj boş olamaz.");

		#endregion

		#region [ Chat History ]

		var allChat = (await _redisServiceClient.GetAllAsync())
			.OrderBy(x => DateTime.ParseExact(x.Key, "d.MM.yyyy HH.mm.ss", null))
			.ToDictionary(x => x.Key, x => x.Value);

		#endregion

		#region [ Create Service Model ]

		// Take chat history from redis
		var messages = allChat
			.Where(x => x.Value is not null)
			.Select(x => new MessageContent
			{
				Role = "assistant",
				Content = x.Value!
			})
			.ToList();

		messages.Add(new MessageContent { Role = "user", Content = input.Message });

		// Add current message
		var groqRequest = new MessageRequest
		{
			Model = "llama-3.3-70b-versatile",
			Messages = messages
		};

		#endregion

		#region [ Service Call ]

		// Send Messagge
		var response = await _groqServiceClient.SendMessageAsync(groqRequest);

		#endregion

		#region [ Redis Call for Last Message ]

		// Set ket to current date time
		var key = DateTime.Now.ToString().Replace(':', '.');

		// Set value to redis with 1 hour expiration time
		var redisResponse = _redisServiceClient.SetStringAsync(key, input.Message, new TimeSpan(1, 0, 0));

		#endregion

		#region [ Set Result ]

		// Take chatbot message
		var responseMessage = response.choices.FirstOrDefault()?.message?.content ?? "There is no content";

		#endregion

		return Ok(new
		{
			responseMessage
		});
	}
}
