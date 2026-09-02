using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostOfficeShipment.Application.DTOs.Common;
using PostOfficeShipment.Application.DTOs.Shipments;
using PostOfficeShipment.Application.Interfaces;

namespace PostOfficeShipment.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IShipmentService _shipmentService;

    public ShipmentsController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpPost]
    public async Task<ActionResult<ShipmentResponse>> Create(CreateShipmentRequest request, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = shipment.Id },
            shipment);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShipmentResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.GetByIdAsync(id, cancellationToken);

        if (shipment is null)
        {
            return NotFound();
        }

        return Ok(shipment);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<ShipmentResponse>>> GetAll([FromQuery] ShipmentQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _shipmentService.GetPagedAsync(request, cancellationToken);

        return Ok(result);

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShipmentResponse>> Update(int id, UpdateShipmentRequest request, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.UpdateAsync(id, request, cancellationToken);

        if (shipment is null)
        {
            return NotFound();
        }

        return Ok(shipment);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _shipmentService.DeleteAsync(
        id,
        cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();

    }

    [HttpPost("{id:int}/move")]
    public async Task<ActionResult<ShipmentResponse>> Move(int id, MoveShipmentRequest request, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.MoveAsync(id, request, cancellationToken);

        if (shipment is null)
        {
            return NotFound();
        }

        return Ok(shipment);
    }

    [HttpPost("{id:int}/receive-destination")]
    public async Task<ActionResult<ShipmentResponse>> ReceiveAtDestination(int id, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.ReceiveAtDestinationAsync(id, cancellationToken);

        if (shipment is null)
        {
            return NotFound();
        }

        return Ok(shipment);
    }

    [HttpPost("{id:int}/deliver")]
    public async Task<ActionResult<ShipmentResponse>> Deliver(int id, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.DeliverAsync(id, cancellationToken);

        if (shipment is null)
        {
            return NotFound();
        }

        return Ok(shipment);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ShipmentSummaryResponse>> GetSummary(CancellationToken cancellationToken)
    {
        var result = await _shipmentService.GetSummaryAsync(cancellationToken);

        return Ok(result);
    }
}