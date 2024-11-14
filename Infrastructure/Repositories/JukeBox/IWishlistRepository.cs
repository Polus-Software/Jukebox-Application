using Domain.Entities.JukeBox;

namespace Infrastructure.Repositories.JukeBox
{
    public interface IWishlistRepository
    {
        string CheckSongAlreadyAdded(string accountId, string soundZoneId, string songId);
        Task<string> AddToWishlist(string accountId, string soundZoneId, string songId);
        Task<string[]> GetLatestQueue(string accountId, string soundZoneId);
        Task<bool> CreateQueue(string soundZoneId, Array trackIds, int actualQueueLength);
        Task<bool> UpvoteSongInWishlist(string accountId, string soundZoneId, string songId);
        Task<string[]> CheckCurrentlyPlaying(string soundZoneId, string[] newArray);
        Task<string[]> PlayBackHistoryList(string soundZoneId, int queueLength);
        string[] CompareWithPlayBackHistory(string[] playBackHistory, string[] latestQueue);
        Task<TrackListResponse> GetWishListSongsList(List<wishList> trackids);
        Task<List<wishList>> GetAllSongsInQueue(string accountId, string soundZoneId);
        Task<bool> RemoveUpvoteForSong(string accountId, string soundZoneId, string songId);
        Task<string> GetLastPlayedSong(string soundZoneId);
    }
}
