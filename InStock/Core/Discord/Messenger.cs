using System.Linq;
using System.Threading;
using Discord;
using Discord.WebSocket;
using InStock.Core.Data;
using InStock.Core.Services;

namespace InStock.Core.Discord;

public class Messenger
{
    private static readonly ulong GUILD_ID = 0; // change this to your guild id (server id)
    private static readonly ulong AMAZON_ID = 0; // change this to the channel id

    private static readonly Color AMAZON_COLOR = new Color(255, 153, 0);
    
    private static SocketTextChannel AmazonTextChannel;
    private static DiscordSocketClient Client;
    
    public Messenger(DiscordSocketClient client)
    {
        Client = client;
        FetchTextChannels();

        ClearChannels();
    }

    private void ClearChannels()
    {
        async void Start()
        {
            var messages = AmazonTextChannel.GetMessagesAsync(int.MaxValue).Flatten();
            foreach (var h in await messages.ToArrayAsync())
            {
                await AmazonTextChannel.DeleteMessageAsync(h);
                Thread.Sleep(500);
            }
        }

        new Thread(Start).Start();
    }

    public void SendEmbed(string name, string fieldName, string fieldValue, string link)
    {
        EmbedBuilder embedBuilder = new()
        {
            Title = name,
            Description = $"[Buy Here]({link})"
        };
        embedBuilder.AddField(fieldName, fieldValue);
        embedBuilder.WithColor(AMAZON_COLOR);
        embedBuilder.WithAuthor(Client.CurrentUser);
        
        AmazonTextChannel.SendMessageAsync(default, default, embedBuilder.Build()).GetAwaiter().GetResult();
    }

    public void SendProductEmbed(ProductData productData, string link)
    {
        EmbedBuilder embedBuilder = new()
        {
            Title = "A new product has been pushed",
            Description = $"[Buy Here]({link})"
        };
        embedBuilder.AddField("Name", $"[{productData.Name}]({link})");
        embedBuilder.AddField("Price", productData.Price);
        embedBuilder.AddField("Stock", productData.Stock);
        embedBuilder.WithColor(AMAZON_COLOR);
        embedBuilder.WithAuthor(Client.CurrentUser);
        
        AmazonTextChannel.SendMessageAsync(default, default, embedBuilder.Build()).GetAwaiter().GetResult();
    }

    private void FetchTextChannels()
    {
        AmazonTextChannel = Client.GetGuild(GUILD_ID).GetTextChannel(AMAZON_ID);
    }
}