# BasicChatbot API

A simple and extensible chatbot API built with ASP.NET Core that integrates with multiple AI providers including Groq and Gemini to provide intelligent conversational responses.

## Features

- RESTful API for chat interactions
- Support for multiple AI providers (Groq, Gemini)
- Clean architecture with separated models and integrations
- Swagger/OpenAPI documentation
- Configurable AI models and endpoints

## Architecture

The project follows a modular architecture with the following components:

- **[BasicChatbot.API](./BasicChatbot/)** - Main API project with controllers and configuration
- **[BasicChatBot.Models](./BasicChatBot.Models/)** - Request/response models
- **[BasicChatbot.Common.Models](./BasicChatbot.Common.Models/)** - Shared models
- **[BasicChatbot.Service](./BasicChatbot.Service/)** - Business logic services
- **[BasicChatbot.Integrations](./BasicChatbot.Integrations/)** - External service integrations

## Prerequisites

- .NET 8.0 or later
- Valid API keys for AI providers (Groq, Gemini)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd BasicChatbot
```

### 2. Configure API Keys

Update the API keys in [`appsettings.json`](./BasicChatbot/appsettings.json):

```json
{
  "Groq": {
    "ApiKey": "your-groq-api-key",
    "ApiUrl": "https://api.groq.com/openai/v1/chat/completions"
  },
  "Gemini": {
    "ApiKey": "your-gemini-api-key",
    "ApiUrl": "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"
  }
}
```

### 3. Build and Run

```bash
dotnet build
cd BasicChatbot
dotnet run
```

The API will be available at `http://localhost:5274` (or the port specified in your configuration).

## API Endpoints

### POST `/api/chat`

Send a message to the chatbot and receive an AI-generated response.

**Request Body:**
```json
{
  "message": "Hello, how are you?"
}
```

**Response:**
```json
{
  "responseMessage": "Hello! I'm doing well, thank you for asking. How can I help you today?"
}
```

### Example Usage

Using curl:
```bash
curl -X POST http://localhost:5274/api/chat \
  -H "Content-Type: application/json" \
  -d '{"message": "What is artificial intelligence?"}'
```

You can also test the API using the provided HTTP file [`BasicChatbot.http`](./BasicChatbot/BasicChatbot.http) with tools like REST Client in VS Code.

## Development

### Project Structure

```
BasicChatbot/
├── BasicChatbot/                    # Main API project
│   ├── Controllers/                 # API controllers
│   ├── DI/                         # Dependency injection configuration
│   ├── Program.cs                  # Application entry point
│   └── appsettings.json            # Configuration settings
├── BasicChatBot.Models/            # Data models
│   ├── Requests/                   # Request models
│   └── Responses/                  # Response models
├── BasicChatbot.Integrations/      # External service integrations
├── BasicChatbot.Service/           # Business logic
└── BasicChatbot.Common.Models/     # Shared models
```

### Adding New AI Providers

1. Create a new integration project under `BasicChatbot.Integrations/`
2. Implement the service client interface
3. Register the service in the DI container
4. Update configuration settings

### API Documentation

When running in development mode, Swagger UI is available at:
- `http://localhost:5274/swagger`

## Configuration

The application can be configured through [`appsettings.json`](./BasicChatbot/appsettings.json) and [`appsettings.Development.json`](./BasicChatbot/appsettings.Development.json).

### Supported AI Models

Currently configured with:
- **Groq**: llama-3.3-70b-versatile
- **Gemini**: gemini-1.5-flash

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is open source. Please see the license file for more details.

## Support

If you encounter any issues or have questions, please open an issue in the repository.