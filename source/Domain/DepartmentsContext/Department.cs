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

		private readonly List<DepartmentPosition> _departmentPositions = [];
		private readonly List<Department> departments = [];
		private readonly List<DepartmentLocation> _departmentLocations = [];

		public Department(
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

		public Department(
			DepartmentId id,
			DepartmentName name,
			DepartmentIdentifier identifier,
			EntityLifeTime lifeTime
		)
		{
			Id = id;
			ParentId = null;
			Name = name;
			Identifier = identifier;
			LifeTime = lifeTime;
			Depth = DepartmentDepth.Root();
			Path = DepartmentPath.Create(identifier.Value);
		}


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

		// Метод для добавления дочернего подразделения
		public void AddChild(Department child)
		{
			ArgumentNullException.ThrowIfNull(child);

        if (IsChildOf(child) || child == this)	
        {
        throw new InvalidOperationException("Подразделение не может быть дочерним.");
        }
    
        if (child.ParentId != null && child.ParentId != this.Id)
        {
        throw new InvalidOperationException("Подразделение уже имеет родителя.");
        }
    
       departments.Add(child);
		}
		
		public void AddLocation(Location location)
		{
			if (_departmentLocations.Any(l=>l.LocationId == location.Id))
			{
				throw new InvalidOperationException("Локация уже присутствует в подразделении.");
			}
			_departmentLocations.Add(new DepartmentLocation(Id,location.Id,location,this));
		}
	}
}
