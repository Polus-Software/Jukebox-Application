using MediatR;

namespace Application.Commands.JukeBox
{
    public class CreateWishListCommand : IRequest<string>
    {
        public string SoundZone { get; set; }
        public string SongID {  get; set; }
        public string AccountId { get; set; }
    }
}
