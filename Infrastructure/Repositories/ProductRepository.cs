using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
  public class ProductRepository : IProductRepository
  {
    //private readonly ApplicationDbContext _context;

    //public ProductRepository(ApplicationDbContext context)
    //{
    //  _context = context;
    //}

    public async Task UpsertAsync(IReadOnlyList<KoronaProductDto> products)
    {
      //foreach (var product in products)
      //{
      //  var existingProduct = await _context.Products
      //      .FirstOrDefaultAsync(p => p.Id == product.Id);

      //  if (existingProduct == null)
      //  {
      //    // Add new product
      //    await _context.Products.AddAsync(product);
      //  }
      //  else
      //  {
      //    // Update existing product
      //    _context.Entry(existingProduct).CurrentValues.SetValues(product);
      //  }
      //}

      //await _context.SaveChangesAsync();
    }
  }
}
