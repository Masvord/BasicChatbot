using BasicChatbot.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
			return BadRequest("Message cannot be empty.");

		#endregion

		#region [ Session Context ]

		// You can replace this with a unique value from the user's ID or Token in the future
		string sessionKey = "ChatSession_FatihKara";
		List<MessageContent> messages = [];

		#endregion

		#region [ Take Chat History ]

		// Retrieve the previous chat history from Redis as a single JSON string
		var historyJson = await _redisServiceClient.GetStringAsync(sessionKey);

		if (!String.IsNullOrEmpty(historyJson))
		{
			// Deserialize the JSON back into a List<MessageContent> object to preserve the roles
			messages = JsonSerializer.Deserialize<List<MessageContent>>(historyJson)
				?? [];
		}

		#endregion

		#region [ Create Service Model ]

		// Add the user's NEW message to the list
		messages.Add(new MessageContent
		{
			Role = "user",
			Content = input.Message
		});

		var groqRequest = new MessageRequest
		{
			Model = "llama-3.3-70b-versatile",
			Messages = messages
		};

		#endregion

		#region [ Service Call ]

		// Send the message to Groq API
		var response = await _groqServiceClient.SendMessageAsync(groqRequest);

		if (!response.IsSuccess)
		{
			return StatusCode(500, new
			{
				error = response.ErrorMessage
			});
		}

		var responseMessage = response.Data!.choices.FirstOrDefault()?.message?.content ?? "There is no content";

		#endregion

		#region [ Update History ]

		// Add the AI's NEW response to the list to complete the history context
		messages.Add(new MessageContent
		{
			Role = "assistant",
			Content = responseMessage
		});

		// Serialize the entire updated list back to JSON and save it to Redis with a 1-hour expiration
		var updatedHistoryJson = JsonSerializer.Serialize(messages);
		await _redisServiceClient.SetStringAsync(sessionKey, updatedHistoryJson, TimeSpan.FromHours(1));

		#endregion

		return Ok(new
		{
			responseMessage
		});
	}
}