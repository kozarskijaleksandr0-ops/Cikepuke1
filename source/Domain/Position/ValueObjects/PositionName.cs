using System;

namespace DirectoryService.Domain.PositionsContext.ValueObjects
{
    public sealed record PositionName
    {
        public const int MaxLength = 128;
        public const int MinLength = 2;

        public string Value { get; }

        private PositionName(string value) => Value = value;

        public static PositionName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название позиции не может быть пустым.", nameof(value));

            if (value.Length > MaxLength)
                throw new ArgumentException($"Название позиции не может превышать {MaxLength} символов.", nameof(value));

            if (value.Length < MinLength)
                throw new ArgumentException($"Название позиции должно быть от {MinLength} до {MaxLength} символов.", nameof(value));

            return new PositionName(value);
        }
    }
}