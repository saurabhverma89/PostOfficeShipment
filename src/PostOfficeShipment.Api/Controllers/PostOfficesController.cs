using Microsoft.AspNetCore.Mvc;
using PostOfficeShipment.Application.DTOs.PostOffices;
using PostOfficeShipment.Application.Interfaces;

namespace PostOfficeShipment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostOfficesController : ControllerBase
{
    private readonly IPostOfficeService _postOfficeService;

    public PostOfficesController(IPostOfficeService postOfficeService)
    {
        _postOfficeService = postOfficeService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostOfficeResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _postOfficeService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostOfficeResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _postOfficeService.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PostOfficeResponse>> Create(CreatePostOfficeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postOfficeService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
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

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostOfficeResponse>> Update(int id, UpdatePostOfficeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postOfficeService.UpdateAsync(id, request, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
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
        var deleted = await _postOfficeService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

}
