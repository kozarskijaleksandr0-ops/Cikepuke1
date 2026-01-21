using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LocationsContext.ValueObjects
{
    internal class EntityLifeTime
    {
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public bool IsActivate { get; }

        private EntityLifeTime(DateTime createdAt, DateTime updatedAt, bool isActivate)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsActivate = isActivate;
        }

        public static EntityLifeTime Create(DateTime createdAt, DateTime updatedAt, bool isActivate)
        {
            if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
                throw new ArgumentException("Некорректное значение даты создания.", nameof(createdAt));

            if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
                throw new ArgumentException(
                    "Некорректное значение даты обновления.",
                    nameof(updatedAt)
                );

            if (updatedAt < createdAt)
                throw new ArgumentException(
                    "Дата обновления не может быть меньше даты создания.",
                    nameof(updatedAt)
                );

            return new EntityLifeTime(createdAt, updatedAt, isActivate);
        }
    }

}

    

