using System;

namespace DirectoryService.Domain.Shared
{
    public sealed record EntityLifeTime
    {
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public bool IsActive { get; }

        private EntityLifeTime(DateTime createdAt, DateTime updatedAt, bool isActive)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsActive = isActive;
        }

        public static EntityLifeTime Create(DateTime createdAt, DateTime updatedAt, bool isActive = true)
        {
            if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
                throw new ArgumentException("Некорректное значение даты создания.", nameof(createdAt));

            if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
                throw new ArgumentException("Некорректное значение даты обновления.", nameof(updatedAt));

            if (updatedAt < createdAt)
                throw new ArgumentException("Дата обновления не может быть меньше даты создания.", nameof(updatedAt));

            return new EntityLifeTime(createdAt, updatedAt, isActive);
        }
    }
}