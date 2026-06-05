namespace BasicChatbot.Integrations.Groq.Models.Responses;

#pragma warning disable IDE1006 // Naming Styles

public class GroqResponse
{

	public string? id { get; set; }

	public string? @object { get; set; }

	public long created { get; set; }

	public string? model { get; set; }

	public List<Choice> choices { get; set; } = [];

	public Usage usage { get; set; } = new();

	public string? system_fingerprint { get; set; }
}

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

public class Usage
{
	public double queue_time { get; set; }

	public int prompt_tokens { get; set; }

	public double prompt_time { get; set; }

	public int completion_tokens { get; set; }

	public double completion_time { get; set; }

	public int total_tokens { get; set; }

	public double total_time { get; set; }
}

#pragma warning restore IDE1006 // Naming Styles