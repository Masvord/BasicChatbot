using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BasicChatbot.Gemini.Integrations;

public class ChatbotServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
{
	private readonly string _apiKey = configuration["Gemini:ApiKey"]
		?? throw new ArgumentNullException("Api Key is null!");

	private readonly string GeminiUrl = configuration["Gemini:ApiUrl"]
		?? throw new ArgumentNullException("Api URL is null!");

	public async Task<string> SendMessageAsync(string message)
	{
		var client = httpClientFactory.CreateClient();

		var body = new
		{
			contents = new[]
			{
				new
				{
					parts = new[]
					{
						new { text = message }
					}
				}
			}
		};

		var response = await client.PostAsJsonAsync($"{GeminiUrl}?key={_apiKey}", body);

		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException($"Gemini API hatası: {response.StatusCode}");

		var json = await response.Content.ReadFromJsonAsync<JsonElement>();

		return json
			.GetProperty("candidates")[0]
			.GetProperty("content")
			.GetProperty("parts")[0]
			.GetProperty("text")
			.GetString() ?? "Yanıt alınamadı.";
	}
}
