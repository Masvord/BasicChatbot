using BasicChatbot.Common.Models.Results;
using BasicChatbot.Integrations.Groq.Models.Requests;
using BasicChatbot.Integrations.Groq.Models.Responses;
using BasicChatbot.Models.Requests;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace BasicChatbot.Integrations.Groq
{
	public class GroqServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
	{
		private readonly string _apiKey = configuration["Groq:ApiKey"]
		   ?? throw new ArgumentNullException("[ Chatbot.Integrations.Groq ] - [ GroqServiceClient ] - [ ApiKey ] -  ApiKey configuration is missing!");

		private readonly string _apiUrl = configuration["Groq:ApiUrl"]
			?? throw new ArgumentNullException("[ Chatbot.Integrations.Groq ] - [ GroqServiceClient ] - [ ApiUrl ] -  ApiUrl configuration is missing!");

		public async Task<Result<GroqResponse>> SendMessageAsync(MessageRequest request)
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
			{
				return Result<GroqResponse>.Failure(new FailureModel()
				{
					ErrorMessage = $"Groq API request failed with status code: {response.StatusCode}",
				});
			}

			var groqResponse = await response.Content.ReadFromJsonAsync<GroqResponse>();

			if (groqResponse is null)
			{
				return Result<GroqResponse>.Failure(new FailureModel()
				{
					ErrorMessage = "Groq API response deserialization returned null",
				});
			}

			return Result<GroqResponse>.Success(groqResponse);
		}
	}
}
