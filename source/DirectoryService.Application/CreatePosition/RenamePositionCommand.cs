namespace DirectoryService.Application.CreatePosition;

public class RenamePositionCommand
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}
