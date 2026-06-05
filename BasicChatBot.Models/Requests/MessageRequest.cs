namespace BasicChatbot.Models.Requests;

public class MessageRequest
{
	public string Model { get; set; } = "llama-3.3-70b-versatile";

	public List<MessageContent> Messages { get; set; } = [];
}
