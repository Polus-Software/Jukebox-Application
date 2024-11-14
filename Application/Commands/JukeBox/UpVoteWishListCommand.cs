using MediatR;

namespace Application.Commands.JukeBox
{
    public class UpVoteWishListCommand : IRequest<bool>
    {
        public string SoundZone { get; set; }
        public string SongId { get; set; }
        public string AccountId { get; set; }
    }
}
