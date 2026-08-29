namespace PostOfficeShipment.Application.DTOs.PostOffices;

public class CreatePostOfficeRequest
{
    public required string ZipCode { get; set; }

    public required string Name { get; set; }

    public string? Address { get; set; }

}
