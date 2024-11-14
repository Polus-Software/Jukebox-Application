using Application.Common;
using MediatR;

namespace Application.Commands.POS
{
  public class ImportProductsCommand : IRequest<OperationResult>
  {
  }
}