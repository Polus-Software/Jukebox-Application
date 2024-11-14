using Application.Commands.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class PlaylistSongsCommandHandler : IRequestHandler<PlaylistSongsCommand, TrackListResponse>
    {
        private readonly IJukeBoxService _jukeboxservices;

        public PlaylistSongsCommandHandler(IJukeBoxService jukeBoxService)
        {
            _jukeboxservices = jukeBoxService;
        }

        public async Task<TrackListResponse> Handle(PlaylistSongsCommand request, CancellationToken cancellationToken)
        {
            var playListId = await _jukeboxservices.GetPlayListId(request.SoundZone);
            var songsList = await _jukeboxservices.GetSongs(playListId);
            return songsList;
        }
    }
}
