using MediatR;
using MES.Application.DTOs;
using MES.Application.WorkOrders.Commands;
using MES.Application.WorkOrders.Queries;
using MES.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MES.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class WorkOrdersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WorkOrderStatus? status, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetWorkOrdersQuery(status), cancellationToken);

            return result.IsSuccess 
                ? Ok(result.Value) 
                : BadRequest(result.Error);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetWorkOrderByIdQuery(id), cancellationToken);

            return result.IsSuccess 
                ? Ok(result.Value) 
                : NotFound(result.Error);
        }

        [HttpPost]
        [Authorize(Roles = "Planner,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateWorkOrderDto dto,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new CreateWorkOrderCommand(dto), cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
                : BadRequest(result.Error);
        }

        [HttpPost("{id:guid}/release")]
        [Authorize(Roles = "Planner,Admin")]
        public async Task<IActionResult> Release(Guid id,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new ReleaseWorkOrderCommand(id), cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> Start(Guid id,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new StartWorkOrderCommand(id), cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new CompleteWorkOrderCommand(id), cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpPost("production")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> RecordProduction([FromBody] RecordProductionDto dto,CancellationToken cancellationToken)
        {
            var operatorId = User.Identity?.Name ?? "unknown";

            var result = await mediator.Send(
                new RecordProductionCommand(dto, operatorId), cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }

    
}
