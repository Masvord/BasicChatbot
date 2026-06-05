using BasicChatbot.Models.Requests;

namespace BasicChatbot.Integrations.Groq.Models.Requests;

public class GroqRequest
{
	public string Model { get; set; } = "llama-3.3-70b-versatile";

	public List<GroqRequestContent> Messages { get; set; } = [];
}

public class GroqRequestContent
{
	public string Role { get; set; } = "user";

	public string? Content { get; set; } 
}
