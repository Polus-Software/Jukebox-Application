using Application.Commands.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class RemoveUpvoteCommandHandler : IRequestHandler<RemoveUpvoteCommand, bool>
    {
        private readonly IWishListService _wishListService;

        public RemoveUpvoteCommandHandler(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<bool> Handle(RemoveUpvoteCommand request, CancellationToken cancellationToken)
        {
            bool result = await _wishListService.RemoveUpvote(request.AccountId, request.SoundZone, request.SongId);
            return result;
        }
    }
}
