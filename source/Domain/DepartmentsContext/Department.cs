using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.DepartmentsContext
{
	public class Department
	{
		public DepartmentId Id { get; }
		public DepartmentName Name { get; }
		public DepartmentIdentifier Identifier { get; }
		public DepartmentId? ParentId { get; }
		public DepartmentPath Path { get; }
		public DepartmentDepth Depth { get; }
		public bool IsActive { get; }
		public EntityLifeTime LifeTime { get; set; }

		// Фабричный метод для создания корневого подразделения
		public static Department CreateRoot(DepartmentName name, DepartmentIdentifier identifier, bool isActive = true)
		{
			DepartmentId id = DepartmentId.Create();
			DepartmentPath path = DepartmentPath.CreateForRoot(identifier.Value);
			DepartmentDepth depth = DepartmentDepth.CalculateFromPath(path);
			EntityLifeTime lifeTime = EntityLifeTime.Create(createdAt: DateTime.UtcNow, updatedAt: DateTime.UtcNow);

			return new Department(id, name, identifier, null, path, depth, isActive, lifeTime);
		}

		// Фабричный метод для создания дочернего подразделения
		public static Department CreateChild(
			DepartmentName name,
			DepartmentIdentifier identifier,
			Department parent,
			bool isActive = true
		)
		{
			DepartmentId id = DepartmentId.Create();
			DepartmentPath path = DepartmentPath.CreateForChild(parent.Path, identifier.Value);
			DepartmentDepth depth = DepartmentDepth.CalculateFromPath(path);
			EntityLifeTime lifeTime = EntityLifeTime.Create(createdAt: DateTime.UtcNow, updatedAt: DateTime.UtcNow);

			return new Department(id, name, identifier, parent.Id, path, depth, isActive, lifeTime);
		}

		// Закрытый конструктор
		private Department(
			DepartmentId id,
			DepartmentName name,
			DepartmentIdentifier identifier,
			DepartmentId? parentId,
			DepartmentPath path,
			DepartmentDepth depth,
			bool isActive,
			EntityLifeTime lifeTime
		)
		{
			Id = id;
			Name = name;
			Identifier = identifier;
			ParentId = parentId;
			Path = path;
			Depth = depth;
			IsActive = isActive;
			LifeTime = lifeTime;
		}

		// Метод для изменения активности
		public Department ChangeActivity(bool isActive)
		{
			EntityLifeTime updatedLifeTime = EntityLifeTime.Create(
				createdAt: LifeTime.CreatedAt,
				updatedAt: DateTime.UtcNow,
				isActive: isActive
			);

			return new Department(Id, Name, Identifier, ParentId, Path, Depth, isActive, updatedLifeTime);
		}

		// Метод для проверки, является ли подразделение корневым
		public bool IsRoot()
		{
			return ParentId == null;
		}

		// Метод для проверки, является ли подразделение дочерним относительно другого
		public bool IsChildOf(Department parent)
		{
			if (parent == null)
			{
				return false;
			}

			return Path.Value.StartsWith(parent.Path.Value + ".", StringComparison.InvariantCultureIgnoreCase);
		}

		private readonly List<Position> positions = [];

		public void AddPosition(Position position)
		{
			foreach (Position p in positions)
			{
				if (p.Name == position.Name)
				{
					throw new ArgumentException("Внтури подразделения уже есть такая должность");
				}
			}

			positions.Add(position);

			LifeTime = LifeTime.Update();
		}

		private readonly List<Location> offices = [];

		public void Addoffice(Location office)
		{
			foreach (Location l in offices)
			{
				if (l.Name == office.Name)
				{
					throw new ArgumentException("Уже есть такое название");
				}
				if (l.Address == office.Address)
				{
					throw new ArgumentException("Есть уже с таким адресом");
				}
				if (l.Id == office.Id)
				{
					throw new ArgumentException("Такой оффис уже есть");
				}
			}
			offices.Add(office);

			LifeTime = LifeTime.Update();
		}
	}
}
