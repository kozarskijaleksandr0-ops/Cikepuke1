namespace WebApi2;

public class UpdateLocationRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? TimeZone { get; internal set; }

}
