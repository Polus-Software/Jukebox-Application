using Application.Common;
using Application.Commands.POS;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.POS
{
  public class ImportProductsCommandHandler : IRequestHandler<ImportProductsCommand, OperationResult>
  {
    private readonly IPOSService _posService;

    public ImportProductsCommandHandler(IPOSService posService)
    {
      _posService = posService;
    }

    public async Task<OperationResult> Handle(ImportProductsCommand request, CancellationToken cancellationToken)
    {
      var products = await _posService.FetchProductsFromPOSAsync();

      if (products == null || products.Count == 0)
      {
        return new OperationResult(false, "No products were returned from the POS API.");
      }

      await _posService.UpsertProductsAsync(products);

      return new OperationResult(true, "Products imported successfully.");
    }
  }
}
