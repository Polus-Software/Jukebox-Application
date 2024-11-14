using Application.Common;
using Application.Common.JukeBox;
using Application.Commands.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace umbGastroOn2024.Controllers
{
    [ApiController]
    [Route("api/jukebox")]
    public class JukeboxController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISoundTrackService _soundtrackService;
        private IGlobalListService _globalListService;

        public JukeboxController(IMediator mediator, ISoundTrackService soundTrackService, IGlobalListService globalListService)
        {
            _mediator = mediator;
            _soundtrackService = soundTrackService;
            _globalListService = globalListService;
        }

        [HttpGet("getSongs")]
        public async Task<JukeBoxResult<TrackListResponse>> GetSongs(string zoneId)
        {
            try
            {
                var command = new PlaylistSongsCommand { SoundZone = zoneId };
                var songs = await _mediator.Send(command);
                var response = new JukeBoxResult<TrackListResponse>(songs, "successful");
                // Start websocket
                if (!_globalListService.ContainsString(zoneId))  // Check if subscription with zone id running
                {
                    await _soundtrackService.ConnectAndSubscribeAsync(zoneId);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new JukeBoxResult<TrackListResponse>(ex.Message);
                return errorResponse;
            }
        }

        [HttpGet("getNowPlayingSong")]
        public async Task<JukeBoxResult<Track>> GetNowPlayingSong(string zoneId)
        {
            try
            {
                var command = new NowPlayingSongCommand { SoundZone = zoneId };
                var songDetails = await _mediator.Send(command);
                var response = new JukeBoxResult<Track>(songDetails, "successful");
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new JukeBoxResult<Track>(ex.Message);
                return errorResponse;
            }
        }

        [HttpPost("volumeChange")]
        public async Task<OperationResult> VolumeChange(string zoneId)
        {
            try
            {
                var command = new VolumeChangeCommand { SoundZone = zoneId };
                bool result = await _mediator.Send(command);
                return new OperationResult(true, "successful");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        [HttpPost("addToWishList")]
        public async Task<JukeBoxResult<string>> AddToWishList(AccountDetails details)
        {
            try
            {
                var command = new CreateWishListCommand { SoundZone = details.SoundZone, AccountId = details.AccountId, SongID = details.SongID };
                string trackId = await _mediator.Send(command);
                return new JukeBoxResult<string>(trackId, "successful");
            }
            catch (Exception ex)
            {
                return new JukeBoxResult<string>(ex.Message);
            }
        }

        [HttpPost("upVoteSong")]
        public async Task<OperationResult> UpVoteSong(AccountDetails details)
        {
            try
            {
                var command = new UpVoteWishListCommand { SoundZone = details.SoundZone, AccountId = details.AccountId, SongId = details.SongID };
                bool result = await _mediator.Send(command);
                return new OperationResult(true, "successful");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        [HttpGet("getWishList")]
        public async Task<JukeBoxResult<TrackListResponse>> GetWishList(string accountId, string zoneId)
        {
            try
            {
                var command = new WishListCommand { AccountId = accountId, SoundZone = zoneId };
                var songDetails = await _mediator.Send(command);
                var response = new JukeBoxResult<TrackListResponse>(songDetails, "successful");
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new JukeBoxResult<TrackListResponse>(ex.Message);
                return errorResponse;
            }
        }

        [HttpPost("removeUpvote")]
        public async Task<JukeBoxResult<bool>> RemoveUpvote(AccountDetails details)
        {
            try
            {
                var command = new RemoveUpvoteCommand { SoundZone = details.SoundZone, AccountId = details.AccountId, SongId = details.SongID };
                bool result = await _mediator.Send(command);
                var response = new JukeBoxResult<bool>(true, "successful");
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new JukeBoxResult<bool>(ex.Message);
                return errorResponse;
            }
        }
    }
}
