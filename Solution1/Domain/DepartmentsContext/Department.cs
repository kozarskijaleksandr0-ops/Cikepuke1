using DirectoryService.Domain.DepartmentsContext.ValueObjects;
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
        public EntityLifeTime LifeTime { get; }

        // Фабричный метод для создания корневого подразделения
        public static Department CreateRoot(
            DepartmentName name,
            DepartmentIdentifier identifier,
            bool isActive = true)
        {
            var id = DepartmentId.Create();
            var path = DepartmentPath.CreateForRoot(identifier.Value);
            var depth = DepartmentDepth.CalculateFromPath(path);
            var lifeTime = EntityLifeTime.Create(
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow
            );

            return new Department(
                id,
                name,
                identifier,
                null,
                path,
                depth,
                isActive,
                lifeTime
            );
        }

        // Фабричный метод для создания дочернего подразделения
        public static Department CreateChild(
            DepartmentName name,
            DepartmentIdentifier identifier,
            Department parent,
            bool isActive = true)
        {
            var id = DepartmentId.Create();
            var path = DepartmentPath.CreateForChild(parent.Path, identifier.Value);
            var depth = parent.Depth.Increment();
            var lifeTime = EntityLifeTime.Create(
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow
            );

            return new Department(
                id,
                name,
                identifier,
                parent.Id,
                path,
                depth,
                isActive,
                lifeTime
            );
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
            EntityLifeTime lifeTime)
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
            var updatedLifeTime = EntityLifeTime.Create(
                createdAt: LifeTime.CreatedAt,
                updatedAt: DateTime.UtcNow,
                isActive: isActive
            );

            return new Department(
                Id,
                Name,
                Identifier,
                ParentId,
                Path,
                Depth,
                isActive,
                updatedLifeTime
            );
        }

        // Метод для проверки, является ли подразделение корневым
        public bool IsRoot() => ParentId == null;

        // Метод для проверки, является ли подразделение дочерним относительно другого
        public bool IsChildOf(Department parent)
        {
            if (parent == null) return false;
            return Path.Value.StartsWith(parent.Path.Value + ".");
        }
    }
}