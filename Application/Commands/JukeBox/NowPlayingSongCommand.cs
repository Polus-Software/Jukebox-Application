using Domain.Entities.JukeBox;
using MediatR;

namespace Application.Commands.JukeBox
{
    public class NowPlayingSongCommand : IRequest<Track>
    {
        public string SoundZone {  get; set; }
    }

}
