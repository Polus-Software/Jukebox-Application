using Application.Common.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using Infrastructure.Repositories.JukeBox;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services.JukeBox
{
    public class JukeBoxService : IJukeBoxService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IJukeBoxRepository _jukeBoxRepository;
        public JukeBoxService(IHttpClientService httpClientService, IJukeBoxRepository jukeBoxRepository)
        {
            _httpClientService = httpClientService;
            _jukeBoxRepository = jukeBoxRepository;
        }

        public async Task<TrackListResponse> GetSongs(string playListId)
        {
            var query = new StringContent(Query.GetPlayListSongsQuery(playListId), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var songsList = await _jukeBoxRepository.PrepareSongListObject(responseBody);

            return songsList;
        }

        public async Task<string> GetPlayListId(string soundZoneId)
        {
            var query = new StringContent(Query.GetPlayListQuery(soundZoneId), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var soundZoneArray = JsonDocument.Parse(responseBody).RootElement.GetProperty("data").GetProperty("soundZone").
                GetProperty("account").GetProperty("musicLibrary").GetProperty("playlists").GetProperty("edges");
            var playListId = soundZoneArray[0].GetProperty("node").GetProperty("id").ToString();

            return playListId;
        }

        public async Task<Track> GetNowPlayingSong(string soundZoneId)
        {
            var query = new StringContent(Query.GetNowPlayingQuery(soundZoneId), Encoding.UTF8, "application/json");
            var responseBody = await _httpClientService.GetData(query);
            var jsonResponse = JsonDocument.Parse(responseBody).RootElement;
            var jObject = JObject.Parse(responseBody);
            var song = await _jukeBoxRepository.PrepareNowPlayingSongObject(jObject);

            return song;
        }

        public async Task<bool> UpdateVolume(string soundZoneID)
        {
            bool isSuccess = await _jukeBoxRepository.ChangeVolume(soundZoneID);
            return isSuccess;
        }
    }
}
