using Domain.Entities.JukeBox;
using MediatR;

namespace Application.Commands.JukeBox
{
    public class WishListCommand : IRequest<TrackListResponse>
    {
        public string AccountId { get; set; }
        public string SoundZone { get; set; }
    }
}
