// NovaPAI C# SDK Example
// Install: dotnet add package Azure.AI.OpenAI
// Or:      dotnet add package OpenAI
// Docs: https://api.novapai.ai

using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

// ── Setup Client ────────────────────────────────────────────
var options = new OpenAIClientOptions
{
    Endpoint = new Uri("https://api.novapai.ai/router/v1")
};
var client = new ChatClient(
    model: "deepseek-v4-pro",
    credential: new ApiKeyCredential("your-api-key"),
    options: options
);

// ── Basic Chat ──────────────────────────────────────────────
async Task BasicChat()
{
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Hello!")
    };

    ChatCompletion response = await client.CompleteChatAsync(messages);
    Console.WriteLine(response.Content[0].Text);
}

// ── Streaming ───────────────────────────────────────────────
async Task StreamChat()
{
    var messages = new List<ChatMessage>
    {
        new UserChatMessage("Tell me a joke")
    };

    await foreach (StreamingChatCompletionUpdate chunk in
        client.CompleteChatStreamingAsync(messages))
    {
        foreach (var part in chunk.ContentUpdate)
            Console.Write(part.Text);
    }
    Console.WriteLine();
}

// ── Multi-turn Conversation ─────────────────────────────────
async Task MultiTurnChat()
{
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage("You are a helpful assistant.")
    };

    async Task<string> Chat(string userInput)
    {
        messages.Add(new UserChatMessage(userInput));
        var response = await client.CompleteChatAsync(messages);
        var reply = response.Content[0].Text;
        messages.Add(new AssistantChatMessage(reply));
        return reply;
    }

    Console.WriteLine(await Chat("What is 1+1?"));
    Console.WriteLine(await Chat("Multiply that by 10"));
}

await BasicChat();
await StreamChat();
await MultiTurnChat();
