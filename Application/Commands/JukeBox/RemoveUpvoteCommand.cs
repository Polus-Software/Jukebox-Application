using MediatR;

namespace Application.Commands.JukeBox
{
   public  class RemoveUpvoteCommand : IRequest<bool>
    {
        public string SoundZone { get; set; }
        public string SongId { get; set; }
        public string AccountId { get; set; }
    }
}
