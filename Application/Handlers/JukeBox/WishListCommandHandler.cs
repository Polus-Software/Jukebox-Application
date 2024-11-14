using Application.Commands.JukeBox;
using Domain.Entities.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class WishListCommandHandler : IRequestHandler<WishListCommand, TrackListResponse>
    {
        private readonly IWishListService _wishlistservices;

        public WishListCommandHandler(IWishListService wishListService)
        {
            _wishlistservices = wishListService;
        }

        public async Task<TrackListResponse> Handle(WishListCommand request, CancellationToken cancellationToken)
        {
            var wishList = await _wishlistservices.GetSongsInWishList(request.AccountId, request.SoundZone);
            return wishList;
        }
    }
}
