using System.ComponentModel.DataAnnotations;

namespace PostOfficeShipment.Application.DTOs.PostOffices;

public class CreatePostOfficeRequest
{
    [Required]
    public required string ZipCode { get; set; }
    
    [Required]
    public required string Name { get; set; }

    public string? Address { get; set; }

}
