using System.Net;
using BackendBlocks.Core.Features.Hashs.Commands.CreateHash;
using BackendBlocks.Core.Features.Hashs.Queries.GetHashList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackendBlocks.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class HashController(IMediator _mediator) : ControllerBase   
{

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HashViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<HashViewModel>>> GetAll()
    {
        var result = await _mediator.Send(new GetHashListQuery());
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CreateOrder([FromBody] CreateHashCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

