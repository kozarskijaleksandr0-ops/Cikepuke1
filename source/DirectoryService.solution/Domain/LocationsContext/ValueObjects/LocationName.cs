using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LocationsContext.ValueObjects
{
    public sealed class LocationName
    {
        public const int MaxLength = 128;
        public const int MinLength = 3;

        public string Value { get; }

        private LocationName(string value)
        {
            Value = value;
        }

        public static LocationName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название локации не может быть пустым.", nameof(value));

            var trimmedValue = value.Trim();

            if (trimmedValue.Length > MaxLength)
                throw new ArgumentException(
                    $"Название локации не может превышать {MaxLength} символов.",
                    nameof(value));

            if (trimmedValue.Length < MinLength)
                throw new ArgumentException(
                    $"Название локации должно быть от {MinLength} до {MaxLength} символов.",
                    nameof(value));

            return new LocationName(trimmedValue);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
    

