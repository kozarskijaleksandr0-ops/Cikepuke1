using Microsoft.AspNetCore.Mvc;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;

namespace WebApi2;

[ApiController]
[Route("api/positions")]
public class PositionController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            IEnumerable<Position> positions = PositionStorage.GetAll();
            return Ok(positions);
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        try
        {
            PositionId positionId = PositionId.Create(id);
            Position position = PositionStorage.GetById(positionId);
            
            if (position == null || !position.LifeTime.IsActive)
            {

                return NotFound($"Должность с ID {id} не найдена");
            }


            return Ok(position);
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreatePositionRequest request)
    {
        DateTime Date = DateTime.UtcNow;
        try
        {
            if (request == null)
            {

                return BadRequest("Тело запроса не может быть пустым");
            }


            if (string.IsNullOrWhiteSpace(request.Name))
            {

                return BadRequest("Название не может быть пустым");
            }


            if (string.IsNullOrWhiteSpace(request.Description))
            {

                return BadRequest("Описание не может быть пустым");
            }


            PositionId positionId = PositionId.Create(Guid.NewGuid());
            PositionName name = PositionName.Create(request.Name);
            PositionDescription description = PositionDescription.Create(request.Description);
            EntityLifeTime lifeTime = EntityLifeTime.Create(Date, Date);

            Position position = new Position(positionId, name, description, lifeTime);
            
            PositionStorage.Add(position);
            
            return CreatedAtAction(nameof(GetById), new { id = positionId.Value }, position);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Ошибка в формате данных: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict($"Конфликт при добавлении: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Problem($"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpPatch("{id:guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdatePositionRequest request)
    {
        try
        {
            if (request == null)
            {

                return BadRequest("Тело запроса не может быть пустым");
            }


            PositionId positionId = PositionId.Create(id);
            Position existingPosition = PositionStorage.GetById(positionId);

            if (existingPosition == null || !existingPosition.LifeTime.IsActive)
            {

                return NotFound($"Должность с ID {id} не найдена");
            }


            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                existingPosition.ChangePositionName(PositionName.Create(request.Name));
            }


            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                existingPosition.ChangeDescription(PositionDescription.Create(request.Description));
            }


            PositionStorage.Update(existingPosition);
            return Ok(existingPosition);
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
    public IActionResult Delete([FromRoute] Guid id)
    {
        try
        {
            PositionId positionId = PositionId.Create(id);
            Position existingPosition = PositionStorage.GetById(positionId);
            
            if (existingPosition == null)
            {

                return NotFound($"Должность с ID {id} не найдена");
            }


            existingPosition.Delete();
            PositionStorage.Update(existingPosition);
            
            return Ok(new { message = "Должность успешно архивирована" });
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
    public IActionResult HardDelete([FromRoute] Guid id)
    {
        try
        {
            PositionId positionId = PositionId.Create(id);
            Position existingPosition = PositionStorage.GetById(positionId);
            
            if (existingPosition == null)
            {

                return NotFound($"Должность с ID {id} не найдена");
            }


            PositionStorage.HardRemove(positionId);
            
            return Ok(new { message = "Должность полностью удалена из системы" });
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
}