using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InStock.Core.Discord;

public class DiscordClient
{
    public static DiscordSocketClient _client;
    private readonly DiscordSocketConfig _config;
    private readonly string _token;
    
    
    public static Messenger Messenger;
    
    public DiscordClient(string token)
    {
        _token = token;
        _config = new DiscordSocketConfig()
        {

        };
        _client = new DiscordSocketClient(_config);
        MainAsync().GetAwaiter().GetResult();
    }
    
    private async Task MainAsync()
    {
        _client.Log += Log; 

        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();
        
        _client.Ready += ClientOnReady;

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task<Task> ClientOnReady()
    {
        Messenger = new Messenger(_client);
        
        SetActivity();
        
        SetupServices.SetupAll();

        return Task.CompletedTask;
    }

    private void SetActivity()
    {
        _client.SetActivityAsync(new Game("product stocks.", ActivityType.Watching, ActivityProperties.Embedded)).GetAwaiter().GetResult();
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}