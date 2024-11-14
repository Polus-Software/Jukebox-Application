using Application.Commands.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class NowPlayingSongCommandHandler : IRequestHandler<NowPlayingSongCommand, Track>
    {
        private readonly IJukeBoxService _jukeboxservices;

        public NowPlayingSongCommandHandler(IJukeBoxService jukeBoxService)
        {
            _jukeboxservices = jukeBoxService;
        }

        public async Task<Track> Handle(NowPlayingSongCommand request, CancellationToken cancellationToken)
        {
            var nowPlayingSong = await _jukeboxservices.GetNowPlayingSong(request.SoundZone);
            return nowPlayingSong;
        }
    }
}
