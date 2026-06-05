namespace BasicChatbot.Models.Requests;

public class MessageContent
{
	public string? Role { get; set; } = "user";

	public string? Content { get; set; }
}
