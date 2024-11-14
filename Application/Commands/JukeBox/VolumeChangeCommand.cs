using MediatR;

namespace Application.Commands.JukeBox
{
    public class VolumeChangeCommand : IRequest<bool>
    {
        public string SoundZone { get; set; }
    }
}
