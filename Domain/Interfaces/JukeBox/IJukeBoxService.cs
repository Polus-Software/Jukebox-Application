using Domain.Entities.JukeBox;

namespace Domain.Interfaces.JukeBox
{
    public interface IJukeBoxService
    {
        Task<TrackListResponse> GetSongs(string playListId);
        Task<string> GetPlayListId(string soundZoneId);
        Task<Track> GetNowPlayingSong(string soundZoneId);
        Task<bool> UpdateVolume(string soundZoneID);
    }
}
