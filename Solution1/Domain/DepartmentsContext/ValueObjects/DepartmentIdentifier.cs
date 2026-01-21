using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DirectoryService.Domain.DepartmentsContext.ValueObjects
{
    public sealed record DepartmentIdentifier
    {
        private const string Pattern = @"^[a-z0-9-]+$";
        public const int MaxLength = 50;
        public const int MinLength = 2;

        public string Value { get; }

        private DepartmentIdentifier(string value) => Value = value;

        public static DepartmentIdentifier Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Идентификатор подразделения не может быть пустым.", nameof(value));

            if (value.Length > MaxLength)
                throw new ArgumentException($"Идентификатор подразделения не может превышать {MaxLength} символов.", nameof(value));

            if (value.Length < MinLength)
                throw new ArgumentException($"Идентификатор подразделения должен быть от {MinLength} до {MaxLength} символов.", nameof(value));

            if (!Regex.IsMatch(value, Pattern))
                throw new ArgumentException(
                    "Идентификатор подразделения должен содержать только строчные латинские буквы, цифры и дефисы.",
                    nameof(value));

            return new DepartmentIdentifier(value);
        }
    }
}