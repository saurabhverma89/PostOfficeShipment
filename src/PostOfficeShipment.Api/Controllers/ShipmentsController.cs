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

}