using BasicChatbot.Models.Responses.Groq;

namespace BasicChatbot.Models.Responses;

public class MessageResponse
{
#pragma warning disable IDE1006 // Naming Styles

	public string? id { get; set; }

	public string? @object { get; set; }

	public long created { get; set; }

	public string? model { get; set; }

	public List<Choice> choices { get; set; } = [];

	public Usage usage { get; set; } = new();

	public string? system_fingerprint { get; set; }

#pragma warning restore IDE1006 // Naming Styles
}
