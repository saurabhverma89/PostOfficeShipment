using Microsoft.AspNetCore.Mvc;
using PostOfficeShipment.Application.DTOs.Common;
using PostOfficeShipment.Application.DTOs.Shipments;
using PostOfficeShipment.Application.Interfaces;

namespace PostOfficeShipment.Api.Controllers;

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
        try
        {
            var shipment = await _shipmentService.UpdateAsync(id, request, cancellationToken);

            if (shipment is null)
            {
                return NotFound();
            }

            return Ok(shipment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }

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
        try
        {
            var shipment = await _shipmentService.MoveAsync(id, request, cancellationToken);

            if (shipment is null)
            {
                return NotFound();
            }

            return Ok(shipment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }

    }

    [HttpPost("{id:int}/receive-destination")]
    public async Task<ActionResult<ShipmentResponse>> ReceiveAtDestination(int id, CancellationToken cancellationToken)
    {
        try
        {
            var shipment =
            await _shipmentService.ReceiveAtDestinationAsync(id, cancellationToken);

            if (shipment is null)
            {
                return NotFound();
            }

            return Ok(shipment);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }

    }

    [HttpPost("{id:int}/deliver")]
    public async Task<ActionResult<ShipmentResponse>> Deliver(int id, CancellationToken cancellationToken)
    {
        try
        {
            var shipment = await _shipmentService.DeliverAsync(id, cancellationToken);

            if (shipment is null)
            {
                return NotFound();
            }

            return Ok(shipment);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }

    }


}