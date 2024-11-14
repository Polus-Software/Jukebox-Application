using Domain.Entities.JukeBox;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Repositories.JukeBox
{
    public interface IJukeBoxRepository
    {
        Task<bool> ChangeVolume(string soundZoneId);
        Task<TrackListResponse> PrepareSongListObject(string JsonResponse);
        Task<Track> PrepareNowPlayingSongObject(JObject JsonResponse);
    }
}
