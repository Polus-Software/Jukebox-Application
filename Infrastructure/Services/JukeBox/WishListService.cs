using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using Infrastructure.Repositories.JukeBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace Infrastructure.Services.JukeBox
{
    public class WishListService : IWishListService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IWishlistRepository _wishlistRepository;

        public WishListService(IHttpClientService httpClientService, IWishlistRepository wishlistRepository)
        {
            _httpClientService = httpClientService;
            _wishlistRepository = wishlistRepository;
        }

        public async Task<string> AddToWishList(string soundZoneId, string accountId, string songId)
        {
            string newTrackId = null;
            // check if song id already exist in the wishlist
            var trackId = _wishlistRepository.CheckSongAlreadyAdded(accountId, soundZoneId, songId);
            if (trackId == null)
            {
                // Add the track to wish list in DB
                newTrackId = await _wishlistRepository.AddToWishlist(accountId, soundZoneId, songId);
                if (newTrackId != null)
                {
                    bool status = await CheckHistoryandCurrentPlaying(accountId, soundZoneId, songId);
                }
                return newTrackId;
            }
            else
            {
                bool status = await UpvoteWishlistedSong(accountId, soundZoneId, songId);
                return newTrackId;
            }
        }

        private async Task<bool> UpvoteWishlistedSong(string accountId, string soundZoneId, string songId)
        {
            // Update upvote
            bool isUpvoted = await _wishlistRepository.UpvoteSongInWishlist(accountId, soundZoneId, songId);
            bool status = await CheckHistoryandCurrentPlaying(accountId, soundZoneId, songId);
            return true;
        }

        private async Task<bool> CheckHistoryandCurrentPlaying(string accountId, string soundZoneId, string songId)
        {
            // check if there is any queue already existing for that check DB has any track for the soundzone
           var trackIds = await _wishlistRepository.GetLatestQueue(accountId, soundZoneId);
            //  check if the track is currently playing. if its playing no need to update the queue.
            var filteredTrackList = await _wishlistRepository.CheckCurrentlyPlaying(soundZoneId, trackIds);
            // History checking
            var playBackHistoryIds = await _wishlistRepository.PlayBackHistoryList(soundZoneId, trackIds.Length);
            var comparedQueue = _wishlistRepository.CompareWithPlayBackHistory(playBackHistoryIds, filteredTrackList);
            //Creating a new Queue
            if (comparedQueue.Length != 0)
            {
                bool isQueueCreated = await _wishlistRepository.CreateQueue(soundZoneId, comparedQueue, trackIds.Length);
            }
            return true;
        }

        public async Task<bool> UpvoteSong(string accountId, string soundZoneId, string songId)
        {
            bool isUpvoted = await UpvoteWishlistedSong(accountId, soundZoneId, songId);
            return isUpvoted;
        }

        public async Task<TrackListResponse> GetSongsInWishList(string accountId, string soundZoneId)
        {
            var tracks = await _wishlistRepository.GetAllSongsInQueue(accountId, soundZoneId);
            var wishList = await _wishlistRepository.GetWishListSongsList(tracks);
            return wishList;
        }

        public async Task<bool> RemoveUpvote(string accountId, string soundZoneId, string songId)
        {
            bool isUpvotedRemoved = await _wishlistRepository.RemoveUpvoteForSong(accountId, soundZoneId, songId);
            return isUpvotedRemoved;
        }
    }
}
