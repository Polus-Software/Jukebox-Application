using Domain.Entities;

namespace Domain.Interfaces
{
  public interface IPOSService
  {
    Task<IReadOnlyList<KoronaProductDto>> FetchProductsFromPOSAsync();

    Task UpsertProductsAsync(IReadOnlyList<KoronaProductDto> products);
  }
}
