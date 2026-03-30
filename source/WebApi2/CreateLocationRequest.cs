namespace WebApi2;

public record CreateLocationRequest
{
    public string Address { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TimeZone { get; set; } = "Europe/Moscow";
}

