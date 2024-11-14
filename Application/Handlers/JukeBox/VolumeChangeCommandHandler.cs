using Application.Commands.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class VolumeChangeCommandHandler : IRequestHandler<VolumeChangeCommand, bool>
    {
        private readonly IJukeBoxService _jukeboxservices;

        public VolumeChangeCommandHandler(IJukeBoxService jukeBoxService)
        {
            _jukeboxservices = jukeBoxService;
        }

        public async Task<bool> Handle(VolumeChangeCommand request, CancellationToken cancellationToken)
        {
            bool result = await _jukeboxservices.UpdateVolume(request.SoundZone);
            return result;
        }
    }
}
