namespace BasicChatbot.Models.Responses.Groq;

public class Usage
{
#pragma warning disable IDE1006 // Naming Styles
	public double queue_time { get; set; }

	public int prompt_tokens { get; set; }

	public double prompt_time { get; set; }

	public int completion_tokens { get; set; }

	public double completion_time { get; set; }

	public int total_tokens { get; set; }

	public double total_time { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
