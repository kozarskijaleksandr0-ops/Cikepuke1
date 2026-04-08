namespace DirectoryService.Application.CreateLocation;

public class CreateLocationCommand
{
     public string Name { get;  }  
    public string Address { get;  }  
    public string TimeZone { get;  }  


    public CreateLocationCommand(string name, string address, string timeZone)
    {
        Name = name;
        Address = address;
        TimeZone = timeZone;
    }
}
