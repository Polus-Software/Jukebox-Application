using Application.Commands.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class UpVoteWishListCommandHandler : IRequestHandler<UpVoteWishListCommand, bool>
    {
        private readonly IWishListService _wishListService;

        public UpVoteWishListCommandHandler(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<bool> Handle(UpVoteWishListCommand request, CancellationToken cancellationToken)
        {
            bool result = await _wishListService.UpvoteSong(request.AccountId, request.SoundZone, request.SongId);
            return result;
        }
    }
}
