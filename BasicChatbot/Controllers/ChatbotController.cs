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

		#region [ Create Service Model ]

		var groqRequest = new MessageRequest
		{
			Model = "llama-3.3-70b-versatile",
			Messages = [
				new()
				{
					Role = "user",
					Content = input.Message
				}
			]
		};

		#endregion

		#region [ Service Call ]

		// Send Messagge
		var response = await _groqServiceClient.SendMessageAsync(groqRequest);

		#endregion

		#region [ Set Result ]

		var responseMessage = response.choices.FirstOrDefault()?.message?.content ?? "There is no content";

		;

		#endregion

		return Ok(new
		{
			responseMessage 
		});
	}
}
