using Application.Common.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories.JukeBox
{
    public class JukeBoxRepository : IJukeBoxRepository
    {
        private readonly JukeBoxDbContext _dbContext;
        private readonly AppSettings _appSettings;
        private readonly IHttpClientService _httpClientService;

        public JukeBoxRepository(JukeBoxDbContext jukeBoxDbContext, IOptions<AppSettings> appSettings, IHttpClientService httpClientService)
        {
            _dbContext = jukeBoxDbContext;
            _appSettings = appSettings.Value;
            _httpClientService = httpClientService;
        }

        public async Task<bool> ChangeVolume(string soundZoneId)
        {
            try
            {
                var volumeData = await _dbContext.VolumeDtos.FirstOrDefaultAsync(p => p.SoundZoneId == soundZoneId);
                if (volumeData == null)
                {
                    // Adding new soundzone data to DB
                    var data = new VolumeDto();
                    data.SoundZoneId = soundZoneId;
                    data.VolumeCount = 1;
                    await _dbContext.VolumeDtos.AddAsync(data);
                }
                else
                {
                    if (_appSettings.VolumeLimit == volumeData.VolumeCount) // Check if volume vote limit is reached
                    {
                        int volume = await GetCurrentVolume(soundZoneId); // Get current volume level
                        if (volume != 0)
                        {
                            int newVolume = await UpdateVolume(soundZoneId, volume); // Decrease the volume level 1
                        }
                        // Update the voting to Zero.
                        _dbContext.VolumeDtos.Where(b => b.SoundZoneId == soundZoneId).ExecuteUpdate(s => s.SetProperty(e => e.VolumeCount, e => 0));
                    }
                    else
                    {
                        // Increase the volume vote count
                        _dbContext.VolumeDtos.Where(b => b.SoundZoneId == soundZoneId).ExecuteUpdate(s => s.SetProperty(e => e.VolumeCount, e => e.VolumeCount + 1));
                    }
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<int> GetCurrentVolume(string soundZoneId)
        {
            var query = new StringContent(Query.GetCurrentVolumeQuery(soundZoneId), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;
            int volume = jsonResponse.GetProperty("data").GetProperty("soundZone").GetProperty("playback").GetProperty("volume").GetInt32();
            
            return volume;
        }

        private async Task<int> UpdateVolume(string soundZoneId, int oldVolume)
        {
            int newVolume = oldVolume - 1;
            var query = new StringContent(Query.UpdateVolumeQuery(soundZoneId, newVolume), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;
            int volume = jsonResponse.GetProperty("data").GetProperty("setVolume").GetProperty("volume").GetInt32();
            return volume;
        }

        public async Task<TrackListResponse> PrepareSongListObject(string JsonResponse)
        {
            var jsonResponse = JsonDocument.Parse(JsonResponse).RootElement.ToString();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var tempResponse = System.Text.Json.JsonSerializer.Deserialize<TempResponse>(jsonResponse, options);
            if (tempResponse != null && tempResponse.Data?.Playlist?.Tracks?.Edges != null)
            {
                var trackListResponse = new TrackListResponse
                {
                    Tracks = tempResponse.Data.Playlist.Tracks.Edges.Select(edge => new Track
                    {
                        Title = edge.Node.Title?.ToString(),
                        Id = edge.Node.Id?.ToString(),
                        AlbumTitle = edge.Node.Album.Title?.ToString(),
                        ThumbnailUrl = edge.Node.Album.Display.Image.Sizes.Thumbnail?.ToString(),
                        Artists = edge.Node.Artists.ConvertAll(a => a.Name),
                        durationMs = (int)edge.Node.durationMs
                    }).ToList()
                };

                return trackListResponse;
            }
            else
            {
                return new TrackListResponse { Tracks = new List<Track>() };
            }
        }

        public async Task<Track> PrepareNowPlayingSongObject(JObject JsonResponse)
        {
            var track = JsonResponse["data"]["nowPlaying"]["track"];
            if (track != null && track.HasValues)
            {
                var playingTrack = new Track
                {
                    Id = track["id"]?.ToString(),
                    Title = track["title"]?.ToString(),
                    AlbumTitle = track["album"]?["title"]?.ToString(),
                    ThumbnailUrl = track["display"]?["image"]?["sizes"]?["thumbnail"]?.ToString(),
                    Artists = track["artists"]?.Select(artist => artist["name"]?.ToString()).ToList() ?? new List<string>() 
                };
                return playingTrack;
            }
            else
            {
                return new Track(); 
            }
        }
    }
}
