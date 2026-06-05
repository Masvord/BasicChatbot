namespace BasicChatbot.Models.Responses.Groq;

#pragma warning disable IDE1006 // Naming Styles

public class Choice
{
	public int index { get; set; }

	public GroqMessage? message { get; set; }

	public string? finish_reason { get; set; }
}

public class GroqMessage
{
	public string? role { get; set; }

	public string? content { get; set; }
}

#pragma warning restore IDE1006 // Naming Styles