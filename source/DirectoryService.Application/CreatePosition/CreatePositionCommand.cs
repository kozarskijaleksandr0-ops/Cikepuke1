namespace DirectoryService.Application.CreatePosition;

public class CreatePositionCommand
{
    public string Name { get;  } 
    public string Description { get;  } 

    public CreatePositionCommand(string name, string address)
    {
        Name = name;
        
        Description = address;
    }
}


