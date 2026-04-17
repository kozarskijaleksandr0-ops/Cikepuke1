namespace DirectoryService.Application.CreateLocation;

public class UpdateLocationCommand
{
    public Guid Id { get; set; }
    public string? NewName { get; set; }
    public string? NewAddress { get; set; }
    public string? TimeZone { get; set; }
}
