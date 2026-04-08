using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using Infrastructure.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using WebApi2;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<LocationRepository, LocationRepository>();
builder.Services.AddScoped<CreatePositionHandler>();
builder.Services.AddScoped<CreateLocationHandler>();
builder.Services.AddEndpointsApiExplorer();


WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
}

LocationStorage.InitializeStorage();
PositionStorage.InitializeStorage();

app.UseSwagger();
app.UseSwaggerUI();
app.MapSwagger();

app.MapControllers();

app.MapGet("api/locations", () =>
{
    try
    {
        IEnumerable<Location> locations = LocationStorage.GetAll();
        return Results.Ok(locations);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

app.MapGet("api/locations/{id}", (string id) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        LocationId locationId = LocationId.Create(guid);
        Location location = LocationStorage.GetById(locationId);

        if (location == null || location.LifeTime.IsActive == false)
        {
            return Results.NotFound($"Локация с ID {id} не найдена");
        }

        return Results.Ok(location);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

app.MapPost("api/locations", (CreateLocationRequest request) =>
{
    DateTime Date = DateTime.UtcNow;
    try
    {
        if (string.IsNullOrWhiteSpace(request.Address))
        {
            return Results.BadRequest("Адрес не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest("Название не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(request.TimeZone))
        {
            return Results.BadRequest("Часовой пояс не может быть пустым");
        }

        LocationId locationId;
        LocationAddress address;
        LocationName name;
        IanaTimeZone timeZone;

        try
        {
            locationId = LocationId.Create(Guid.NewGuid());
            address = LocationAddress.Create(request.Address);
            name = LocationName.Create(request.Name);
            timeZone = IanaTimeZone.Create(request.TimeZone);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest($"Ошибка в формате данных: {ex.Message}");
        }

        EntityLifeTime lifeTime = EntityLifeTime.Create(Date, Date);
        Location location = new Location(locationId, address, name, timeZone, lifeTime);

        try
        {
            LocationStorage.Add(location);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict($"Конфликт при добавлении: {ex.Message}");
        }

        return Results.Created($"/api/locations/{locationId.Value}", location);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

app.MapPatch("api/locations/{id:guid}", (
    [FromRoute(Name = "id")] Guid id,
    [FromQuery(Name = "name")] string name,
    [FromQuery(Name = "addres")] string address,
    [FromQuery(Name = "timeZone")] string timeZone) =>
{
    try
    {
        LocationId locationId = LocationId.Create(id);
        Location existingLocation = LocationStorage.GetById(locationId);

        if (existingLocation == null || existingLocation.LifeTime.IsActive == false)
        {
            return Results.NotFound($"Локация с ID {id} не найдена");
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                // изменить название
                existingLocation.ChangeName(LocationName .Create(name));
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                // изменить адрес
                existingLocation.ChangeAddress(LocationAddress.Create(address));
            }

            if (!string.IsNullOrWhiteSpace(timeZone))
            {
                // изменить тайм зону
                existingLocation.ChangeIanaTimeZone(IanaTimeZone.Create(timeZone));
            }

            return Results.Ok(existingLocation);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest($"Ошибка в формате данных: {ex.Message}");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

app.MapDelete("api/locations/{id}", (string id) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        LocationId locationId = LocationId.Create(guid);
        Location existingLocation = LocationStorage.GetById(locationId);

        if (existingLocation == null)
        {
            return Results.NotFound($"Локация с ID {id} не найдена");
        }

        if (existingLocation.LifeTime.IsActive == false)
        {
            return Results.NotFound($"Локация с ID {id} не найдена");
        }

        // Мягкое удаление - устанавливаем DeletedAt
        Location archivedLocation = new Location(
            existingLocation.Id,
            existingLocation.Address,
            existingLocation.Name,
            existingLocation.TimeZone,
            EntityLifeTime.Create(existingLocation.LifeTime.CreatedAt, DateTime.UtcNow)
        );

        LocationStorage.Update(archivedLocation);

        return Results.Ok(new { message = "Локация успешно архивирована" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// DELETE - жесткое удаление (полное удаление из хранилища)
app.MapDelete("api/locations/{id}/hard", (string id) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        LocationId locationId = LocationId.Create(guid);
        Location existingLocation = LocationStorage.GetById(locationId);

        if (existingLocation == null)
        {
            return Results.NotFound($"Локация с ID {id} не найдена");
        }

        // Жесткое удаление
        LocationStorage.HardRemove(locationId);

        return Results.Ok(new { message = "Локация полностью удалена" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// ========== ENDPOINTS ДЛЯ POSITION ==========

// GET все должности (только активные)
app.MapGet("api/positions", () =>
{
    try
    {
        IEnumerable<Position> positions = PositionStorage.GetAll();
        return Results.Ok(positions);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// GET должность по ID (только активные)
app.MapGet("api/positions/{id}", (string id) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        PositionId positionId = PositionId.Create(guid);
        Position position = PositionStorage.GetById(positionId);

        return position == null || position.LifeTime.IsActive  == false
            ? Results.NotFound($"Должность с ID {id} не найдена")
            : Results.Ok(position);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// POST создание новой должности
app.MapPost("api/positions", (CreatePositionRequest request) =>
{
     DateTime Date = DateTime.UtcNow;
    try
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest("Название не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return Results.BadRequest("Описание не может быть пустым");
        }

        PositionId positionId;
        PositionName name;
        PositionDescription description;

        try
        {
            positionId = PositionId.Create(Guid.NewGuid());
            name = PositionName.Create(request.Name);
            description = PositionDescription.Create(request.Description);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest($"Ошибка в формате данных: {ex.Message}");
        }

        EntityLifeTime lifeTime = EntityLifeTime.Create(Date, Date);
        Position position = new Position(positionId, name, description, lifeTime);

        try
        {
            PositionStorage.Add(position);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict($"Конфликт при добавлении: {ex.Message}");
        }

        return Results.Created($"/api/positions/{positionId.Value}", position);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// PATCH обновление должности
app.MapPatch("api/positions/{id}", (string id, UpdatePositionRequest request) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        PositionId positionId = PositionId.Create(guid);
        Position existingPosition = PositionStorage.GetById(positionId);

        if (existingPosition == null || existingPosition.LifeTime.IsActive == false)
        {
            return Results.NotFound($"Должность с ID {id} не найдена");
        }

        PositionName? newName = null;
        PositionDescription? newDescription = null;

        try
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                newName = PositionName.Create(request.Name);
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                newDescription = PositionDescription.Create(request.Description);
            }
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest($"Ошибка в формате данных: {ex.Message}");
        }

        Position updatedPosition = new Position(
            existingPosition.Id,
            newName ?? existingPosition.Name,
            newDescription ?? existingPosition.Description,
            existingPosition.LifeTime
        );

        try
        {
            PositionStorage.Update(updatedPosition);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict($"Конфликт при обновлении: {ex.Message}");
        }

        return Results.Ok(updatedPosition);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// DELETE - мягкое удаление (архивация)
app.MapDelete("api/positions/{id}", (string id) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        PositionId positionId = PositionId.Create(guid);
        Position existingPosition = PositionStorage.GetById(positionId);

        if (existingPosition == null)
        {
            return Results.NotFound($"Должность с ID {id} не найдена");
        }

        if (existingPosition.LifeTime.IsActive == false)
        {
            return Results.NotFound($"Должность с ID {id} не найдена");
        }

        // Мягкое удаление - устанавливаем DeletedAt
        Position archivedPosition = new Position(
            existingPosition.Id,
            existingPosition.Name,
            existingPosition.Description,
            EntityLifeTime.Create(existingPosition.LifeTime.CreatedAt, DateTime.UtcNow)
        );

        PositionStorage.Update(archivedPosition);

        return Results.Ok(new { message = "Должность успешно архивирована" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

// DELETE - жесткое удаление (полное удаление из хранилища)
app.MapDelete("api/positions/{id}/hard", (string id) =>
{
    try
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return Results.BadRequest("Некорректный формат ID. Ожидается GUID.");
        }

        PositionId positionId = PositionId.Create(guid);
        Position existingPosition = PositionStorage.GetById(positionId);

        if (existingPosition == null)
        {
            return Results.NotFound($"Должность с ID {id} не найдена");
        }

        // Жесткое удаление
        PositionStorage.HardRemove(positionId);

        return Results.Ok(new { message = "Должность полностью удалена" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Внутренняя ошибка сервера: {ex.Message}");
    }
});

app.Run();
