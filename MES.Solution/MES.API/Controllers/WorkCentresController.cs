using MediatR;
using MES.Application.DTOs;
using MES.Application.WorkCentres.Queries;
using MES.Application.WorkOrders.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class WorkCentresController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = true, CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new GetWorkCentresQuery(), cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetWorkCentreByIdQuery(id), cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkCentreDto createWorkCentreDto,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new CreateWorkCentreCommand(createWorkCentreDto), cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById),new { id = result.Value }, result.Value)
                : BadRequest(result.Error);
        }
    }
}
