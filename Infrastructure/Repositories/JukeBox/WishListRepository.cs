using Application.Common.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories.JukeBox
{
    public class WishListRepository : IWishlistRepository
    {
        private readonly JukeBoxDbContext _dbContext;
        private readonly IHttpClientService _httpClientService;

        public WishListRepository(JukeBoxDbContext jukeBoxDbContext, IHttpClientService httpClientService)
        {
            _dbContext = jukeBoxDbContext;
            _httpClientService = httpClientService;
        }

        public string CheckSongAlreadyAdded(string accountId, string soundZoneId, string songId)
        {
            var trackId = _dbContext.WishListDtos.Where(p => p.SoundZoneId == soundZoneId && p.AccountId == accountId && p.TrackId == songId).Select(x => x.TrackId).FirstOrDefault();
            return trackId;
        }

        public async Task<string> AddToWishlist(string accountId, string soundZoneId, string songId)
        {
            var wishlist = new WishListDto();
            wishlist.AccountId = accountId;
            wishlist.SoundZoneId = soundZoneId;
            wishlist.TrackId = songId;
            wishlist.UpVoteCount = 0;
            await _dbContext.WishListDtos.AddAsync(wishlist);
            await _dbContext.SaveChangesAsync();

            return songId;
        }

        public async Task<string[]> GetLatestQueue(string accountId, string soundZoneId)
        {
            string[] trackIds = new string[] { };
            trackIds = _dbContext.WishListDtos.Where(p => p.SoundZoneId == soundZoneId && p.AccountId == accountId).OrderByDescending(x => x.UpVoteCount).Select(x => x.TrackId).ToArray();
            return trackIds;
        }

        public async Task<bool> CreateQueue(string soundZoneId, Array trackIds, int actualQueueLength)
        {
            var query = new StringContent(Query.CreateQueueQuery(soundZoneId, trackIds, actualQueueLength), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;

            return true;
        }

        public async Task<bool> UpvoteSongInWishlist(string accountId, string soundZoneId, string songId)
        {
            _dbContext.WishListDtos.Where(b => b.SoundZoneId == soundZoneId && b.AccountId == accountId && b.TrackId == songId).ExecuteUpdate(s => s.SetProperty(e => e.UpVoteCount, e => e.UpVoteCount + 1));
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<string[]> CheckCurrentlyPlaying(string soundZoneId, string[] newArray)
        {
            var query = new StringContent(Query.GetNowPlayingQuery(soundZoneId), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;
            var Playlist = jsonResponse.GetProperty("data").ToString();
            var jsonObject = JObject.Parse(Playlist);
            if (jsonObject != null)
            {
                string currentlyPlaying = jsonObject.SelectToken("nowPlaying.track.id")?.ToString();
                var updatedIds = newArray.Where(id => id != currentlyPlaying).ToArray();
                return updatedIds;
            }
            else
            {
                return newArray;
            }
        }

        public async Task<string[]> PlayBackHistoryList(string soundZoneId, int queueLength)
        {
            var tracks = new PlayBackTracks();
            var query = new StringContent(Query.GetPlayBackHistory(soundZoneId, queueLength), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;
            var playBackHistory = JsonDocument.Parse(responseBody).RootElement.GetProperty("data").GetProperty("soundZone").
                GetProperty("playbackHistory").GetProperty("edges").ToString();
            var nodeList = JsonConvert.DeserializeObject<List<PlayBackTracks>>(playBackHistory);
            var idArray = new string[] { };
            idArray = nodeList.Select(item => item.Node.Track.Id).ToArray();

            return idArray;
        }

        public string[] CompareWithPlayBackHistory(string[] playBackHistory, string[] latestQueue)
        {
            string[] filteredUpVoted = latestQueue.Except(playBackHistory).ToArray();
            return filteredUpVoted;
        }

        public async Task<bool> RemoveUpvoteForSong(string accountId, string soundZoneId, string songId)
        {
            _dbContext.WishListDtos.Where(b => b.SoundZoneId == soundZoneId && b.AccountId == accountId && b.TrackId == songId).ExecuteUpdate(s => s.SetProperty(e => e.UpVoteCount, e => e.UpVoteCount - 1));
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TrackListResponse> GetWishListSongsList(List<wishList> trackids)
        {
            var idsArray = trackids.Select(item => item.SongId.ToString()).ToArray();
            var query = new StringContent(Query.GetSongsListQuery(idsArray), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonObject = JObject.Parse(responseBody);
            var tracks = jsonObject["data"]["tracks"]
                .Select(track => new Track
                {
                    Title = track["title"].ToString(),
                    Id = track["id"].ToString(),
                    AlbumTitle = track["album"]["title"].ToString(),
                    ThumbnailUrl = track["display"]["image"]["sizes"]["thumbnail"].ToString(),
                    Artists = track["artists"]
                        .Select(artist => artist["name"].ToString())
                        .ToList(),
                    durationMs = (int)track["durationMs"],
                    Count = trackids.Where(item => item.SongId == track["id"]?.ToString()).Select(item => item.Count).FirstOrDefault()
                }).OrderByDescending(track => track.Count)
                .ToList();
            var trackList = new TrackListResponse
            {
                Tracks = tracks
            };
            return trackList;
        }

        public async Task<List<wishList>> GetAllSongsInQueue(string accountId, string soundZoneId)
        {
            var tracks = _dbContext.WishListDtos.Where(p => p.SoundZoneId == soundZoneId && p.AccountId == accountId).OrderByDescending(x => x.UpVoteCount).Select(p => new wishList
            {
                Count = p.UpVoteCount,
                SongId = p.TrackId
            })
     .ToList();
            return tracks;
        }

        public async Task<string> GetLastPlayedSong(string soundZoneId)
        {
            var query = new StringContent(Query.GetPlayBackHistory(soundZoneId, 1), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;
            var trackId = JsonDocument.Parse(responseBody).RootElement.GetProperty("data").GetProperty("soundZone").GetProperty("playbackHistory").GetProperty("edges")
            .EnumerateArray().Select(edge => edge.GetProperty("node").GetProperty("track").GetProperty("id").GetString()).FirstOrDefault();

            return trackId;
        }
    }
}
