using System.Net.WebSockets;
using System.Text;
using Application.Common.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using Infrastructure.Persistence;
using Infrastructure.Repositories.JukeBox;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services.JukeBox
{
    public class SoundtrackService : Hub, ISoundTrackService
    {
        private IHubContext<SoundtrackService> _jukeBoxHub;
        private readonly Uri _webSocketUri = new Uri("websocketUri");
        private ClientWebSocket _webSocket;
        private IConfiguration _configuration;
        private IGlobalListService _globalListService;
        private readonly JukeBoxDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWishlistRepository _iWishlistRepository;

        public SoundtrackService(IConfiguration configuration, IHubContext<SoundtrackService> jukeBoxHub, IGlobalListService globalListService, JukeBoxDbContext jukeBoxDbContext, IServiceScopeFactory scopeFactory, IWishlistRepository wishlistRepository)
        {
            _webSocket = new ClientWebSocket();
            _configuration = configuration;
            _jukeBoxHub = jukeBoxHub;
            _globalListService = globalListService;
            _dbContext = jukeBoxDbContext;
            _scopeFactory = scopeFactory;
            _iWishlistRepository = wishlistRepository;
        }

        public async Task ConnectAndSubscribeAsync(string zoneId)
        {
            await _webSocket.ConnectAsync(_webSocketUri, CancellationToken.None);
            await InitialConnection(); // Initalizing connection with soundtrack subscription
            Task nowPlayingSubscription = NowPlayingSubscription(zoneId);
        }

        private async Task InitialConnection()
        {
            var accessToken = _configuration["JukeBoxSettings:APIKey"];
            var initMessage = new
            {
                type = "connection_init",
                payload = new
                {
                    Authorization = $"Basic {accessToken}"
                }
            };
            await SendMessageAsync(initMessage);
            await ReceiveMessageAsync();
        }

        private async Task NowPlayingSubscription(string zoneId)
        {
            var subscriptionQuery = Query.GetNowPlayingSubscription(zoneId);
            var subscriptionMessage = new
            {
                id = zoneId,
                type = "subscribe",
                payload = new
                {
                    query = subscriptionQuery
                }
            };
            //Sending subscription query to get changes in the currently playing song
            await SendMessageAsync(subscriptionMessage);
            if (!_globalListService.ContainsString(zoneId)) // subscription created and added the id into list, keeping the subscription id list 
            {
                _globalListService.AddString(zoneId);
            }
            while (_webSocket.State == WebSocketState.Open)
            {
                await ReceiveMessageAsync();
            }
        }

        private async Task SendMessageAsync(object message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonMessage);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task ReceiveMessageAsync()
        {
            byte[] buffer = new byte[4096];
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            if (message != "")
            {
                var jsonMessage = JObject.Parse(message);
                if (jsonMessage["type"]?.ToString() == "next")
                {
                    var track = jsonMessage["payload"]["data"]["nowPlayingUpdate"]["nowPlaying"]["track"];
                    var playingTrack = new Track();
                    playingTrack.Id = track["id"].ToString();
                    playingTrack.Title = track["title"].ToString();
                    playingTrack.AlbumTitle = track["album"]["title"].ToString();
                    playingTrack.ThumbnailUrl = track["display"]["image"]["sizes"]["thumbnail"].ToString();
                    playingTrack.Artists = track["artists"].Select(artist => artist["name"].ToString()).ToList();
                    try
                    {
                        var zoneId = jsonMessage["id"]?.ToString();
                        var trackId = await _iWishlistRepository.GetLastPlayedSong(zoneId); // Removing last played song from Local. Becuase we do not want to keep the last played songs in the queue.
                        playingTrack.LastplayedSongId = trackId;
                        bool success = RemoveSongFromWishlist(trackId, zoneId);
                        //Transmitting the data to clients using the SignalR Hub.
                        await _jukeBoxHub.Clients.All.SendAsync("NowPlaying", playingTrack);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending message: {ex.Message}");
                    }
                }
                else if (jsonMessage["type"]?.ToString() == "connection_ack")
                {
                    Console.WriteLine("Connection acknowledged by server.");
                }
                else if (jsonMessage["type"]?.ToString() == "complete")
                {
                    Console.WriteLine("Subscription completed.");
                }
            }
        }
        private bool RemoveSongFromWishlist(string trackId, string zoneId)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope()) //While SignalR working as long Task. others will get disposed, so using scopefactory
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<JukeBoxDbContext>();
                    var entity = dbContext.WishListDtos.Where(e => e.TrackId == trackId && e.SoundZoneId == zoneId).FirstOrDefault();
                    if (entity != null)
                    {
                        dbContext.WishListDtos.Remove(entity);
                        dbContext.SaveChanges();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }       
    }
}

