using Application.Commands.POS;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace umbGastroOn2024.Controllers
{
  [ApiController]
  [Route("api/POS")]
  public class POSController : UmbracoApiController
  {
    private readonly IMediator _mediator;
    public POSController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("import-products")]
    public async Task<IActionResult> ImportProducts()
    {
      var result = await _mediator.Send(new ImportProductsCommand());

      if (result.Success)
      {
        return Ok(result.Message);
      }
      else
      {
        return BadRequest(result.Message);
      }
    }
    }
}
