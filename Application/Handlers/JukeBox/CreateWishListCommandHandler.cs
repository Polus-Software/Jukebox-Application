using Application.Commands.JukeBox;
using Domain.Interfaces.JukeBox;
using MediatR;

namespace Application.Handlers.JukeBox
{
    public class CreateWishListCommandHandler : IRequestHandler<CreateWishListCommand, string>
    {
        private readonly IWishListService _wishListService;

        public CreateWishListCommandHandler(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<string> Handle(CreateWishListCommand request, CancellationToken cancellationToken)
        {
            var trackId = await _wishListService.AddToWishList(request.SoundZone, request.AccountId, request.SongID);
            return trackId;
        }
    }
}
