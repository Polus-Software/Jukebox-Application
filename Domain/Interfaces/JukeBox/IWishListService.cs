using Domain.Entities.JukeBox;

namespace Domain.Interfaces.JukeBox
{
    public interface IWishListService
    {
        Task<string> AddToWishList(string soundZoneId, string accountId, string songId);
        Task<bool> UpvoteSong(string accountId, string soundZoneId, string songId);
        Task<TrackListResponse> GetSongsInWishList(string accountId, string soundZoneId);
        Task<bool> RemoveUpvote(string accountId, string soundZoneId, string songId);
    }
}
