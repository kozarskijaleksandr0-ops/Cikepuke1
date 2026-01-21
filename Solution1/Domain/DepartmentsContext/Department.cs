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
        public bool IsActive { get; }
        public EntityLifeTime LifeTime { get; }

        // Конструктор для корневого подразделения
        public Department(
            DepartmentId id,
            DepartmentName name,
            DepartmentIdentifier identifier,
            bool isActive,
            EntityLifeTime lifeTime)
        {
            Id = id;
            Name = name;
            Identifier = identifier;
            ParentId = null;
            Path = DepartmentPath.CreateForRoot(identifier.Value);
            IsActive = isActive;
            LifeTime = lifeTime;
        }

        // Конструктор для дочернего подразделения
        public Department(
            DepartmentId id,
            DepartmentName name,
            DepartmentIdentifier identifier,
            DepartmentId parentId,
            DepartmentPath parentPath,
            bool isActive,
            EntityLifeTime lifeTime)
        {
            Id = id;
            Name = name;
            Identifier = identifier;
            ParentId = parentId;
            Path = DepartmentPath.CreateForChild(parentPath.Value, identifier.Value);
            IsActive = isActive;
            LifeTime = lifeTime;
        }

        // Метод для создания дочернего подразделения
        public Department CreateChild(
            DepartmentName childName,
            DepartmentIdentifier childIdentifier,
            bool isActive = true)
        {
            return new Department(
                id: DepartmentId.Create(),
                name: childName,
                identifier: childIdentifier,
                parentId: Id,
                parentPath: Path,
                isActive: isActive,
                lifeTime: EntityLifeTime.Create(
                    createdAt: DateTime.UtcNow,
                    updatedAt: DateTime.UtcNow
                )
            );
        }

        // Метод для деактивации подразделения
        public Department Deactivate()
        {
            // В реальном проекте здесь была бы логика создания нового объекта
            // с обновленным состоянием, так как объекты иммутабельны
            // Для простоты оставим как есть
            return this;
        }
    }
}