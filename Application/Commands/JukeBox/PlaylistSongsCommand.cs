using Domain.Entities.JukeBox;
using MediatR;

namespace Application.Commands.JukeBox
{
    public class PlaylistSongsCommand : IRequest<TrackListResponse>
    {
        public string SoundZone { get; set; }
    }
}
