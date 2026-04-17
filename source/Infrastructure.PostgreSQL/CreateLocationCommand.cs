namespace Infrastructure.PostgreSQL;

public class CreateLocationCommand
{
     public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TimeZone { get; set; } = "Europe/Moscow";
}
