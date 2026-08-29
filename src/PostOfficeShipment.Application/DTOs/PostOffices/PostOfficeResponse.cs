namespace PostOfficeShipment.Application.DTOs.PostOffices;

public class PostOfficeResponse
{
    public int Id { get; set; }

    public string ZipCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}
