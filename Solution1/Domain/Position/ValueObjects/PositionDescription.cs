using System;

namespace DirectoryService.Domain.PositionsContext.ValueObjects
{
    public sealed record PositionDescription
    {
        public const int MaxLength = 500;

        public string Value { get; }

        private PositionDescription(string value) => Value = value;

        public static PositionDescription Create(string value)
        {
            if (value == null)
                return new PositionDescription(string.Empty);

            if (value.Length > MaxLength)
                throw new ArgumentException($"Описание позиции не может превышать {MaxLength} символов.", nameof(value));

            return new PositionDescription(value);
        }

        public static PositionDescription Empty() => new(string.Empty);
    }
}