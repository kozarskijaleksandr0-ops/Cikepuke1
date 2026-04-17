using Microsoft.AspNetCore.Mvc;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using WebApi2;
using DirectoryService.Domain.Shared;
using static WebApi2.LocationStorage;
using DirectoryService.Application.CreateLocation;


namespace WebApi2;

[ApiController]
[Route("api/locations")]
public class LocationController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            IEnumerable<Location> locations = LocationStorage.GetAll();
            return Ok(locations);
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            LocationId locationId = LocationId.Create(id);
            Location location = LocationStorage.GetById(locationId);
            
            if (location == null || !location.LifeTime.IsActive)
            {

                return NotFound($"Локация с ID {id} не найдена");
            }


            return Ok(location);
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task <IActionResult> Create([FromBody] CreateLocationRequest request,[FromServices] CreateLocationHandler handler, CancellationToken ct = default)
    {
         var command = new CreateLocationCommand(request.Name, request.Address, request.TimeZone);
         Guid result = await handler.Handle(command, ct);
         return Ok(result);
        
    }

    [HttpPatch("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateLocationRequest request)
    {
        try
        {
            if (request == null)
            {

                return BadRequest("Тело запроса не может быть пустым");
            }


            LocationId locationId = LocationId.Create(id);
            Location existingLocation = LocationStorage.GetById(locationId);

            if (existingLocation == null || !existingLocation.LifeTime.IsActive)
            {

                return NotFound($"Локация с ID {id} не найдена");
            }


            if (!string.IsNullOrWhiteSpace(request.NewName))
            {
                existingLocation.ChangeName(LocationName.Create(request.NewName));
            }


            if (!string.IsNullOrWhiteSpace(request.NewAddress))
            {
                existingLocation.ChangeAddress(LocationAddress.Create(request.NewAddress));
            }


            if (!string.IsNullOrWhiteSpace(request.TimeZone))
            {
                existingLocation.ChangeIanaTimeZone(IanaTimeZone.Create(request.TimeZone));
            }


            LocationStorage.Update(existingLocation);
            return Ok(existingLocation);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Ошибка в формате данных: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict($"Конфликт при обновлении: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            LocationId locationId = LocationId.Create(id);
            Location existingLocation = LocationStorage.GetById(locationId);
            
            if (existingLocation == null)
            {

                return NotFound($"Локация с ID {id} не найдена");
            }


            if (!existingLocation.LifeTime.IsActive)
            {

                return NotFound($"Локация с ID {id} не найдена");
            }


            existingLocation.Delete();
            LocationStorage.Update(existingLocation);
            
            return Ok(new { message = "Локация успешно архивирована" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}/hard")]
    public IActionResult HardDelete(Guid id)
    {
        try
        {
            LocationId locationId = LocationId.Create(id);
            Location existingLocation = LocationStorage.GetById(locationId);
            
            if (existingLocation == null)
            {

                return NotFound($"Локация с ID {id} не найдена");
            }


            LocationStorage.HardRemove(locationId);
            
            return Ok(new { message = "Локация полностью удалена из системы" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

     [HttpPut("{id:guid}")]
    public async Task<IResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateLocationRequest request,
        [FromServices] UpdateLocationHandler handler,
        CancellationToken ct)
    {
        try
        {
            var command = new UpdateLocationCommand
            {
                Id = id,
                NewName = request.NewName,
                NewAddress = request.NewAddress
            };

            var result = await handler.Handle(command, ct);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Внутренняя ошибка: {ex.Message}");
        }
    }
    public record UpdateLocationRequest
{
    public string? NewName { get; set; }
    public string? NewAddress { get; set; }
    public string? TimeZone { get; set; }
}
}