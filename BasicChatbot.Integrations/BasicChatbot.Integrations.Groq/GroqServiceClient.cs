using BasicChatbot.Integrations.Groq.Models.Requests;
using BasicChatbot.Integrations.Groq.Models.Responses;
using BasicChatbot.Models.Requests;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BasicChatbot.Integrations.Groq
{
	public class GroqServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
	{
		private readonly string _apiKey = configuration["Groq:ApiKey"]
	   ?? throw new ArgumentNullException("[ Chatbot.Integrations.Groq ] - [ GroqServiceClient ] - [ ApiKey ] -  ApiKey configuration is missing!");

		private readonly string _apiUrl = configuration["Groq:ApiUrl"]
			?? throw new ArgumentNullException("[ Chatbot.Integrations.Groq ] - [ GroqServiceClient ] - [ ApiUrl ] -  ApiUrl configuration is missing!");

		public async Task<GroqResponse> SendMessageAsync(MessageRequest request)
		{
			var client = httpClientFactory.CreateClient();
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

			var requestModel = new GroqRequest
			{
				Model = request.Model,
				Messages = [.. request.Messages.Select(m => new GroqRequestContent
				{
					Role = m.Role ?? "user",
					Content = m.Content
				})]
			};

			var response = await client.PostAsJsonAsync(_apiUrl, requestModel);

			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException($"[ Chatbot.Integrations.Groq ] - [ GroqServiceClient ] -  Response isn't success!: {response.StatusCode}");

			var groqResponse = await response.Content.ReadFromJsonAsync<GroqResponse>();

			return groqResponse
				?? throw new Exception("[ Chatbot.Integrations.Groq ] - [ GroqServiceClient ] -  There is no answer in API!");
		}
	}
}
