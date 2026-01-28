using System;
using System.Linq;

namespace DirectoryService.Domain.DepartmentsContext.ValueObjects
{
    public sealed record DepartmentPath
    {
        public const int MaxLength = 500;

        public string Value { get; }

        private DepartmentPath(string value)
        {
            Value = value;
        }

        public static DepartmentPath Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Путь подразделения не может быть пустым.", nameof(value));

            if (value.Length > MaxLength)
                throw new ArgumentException($"Путь подразделения не может превышать {MaxLength} символов.", nameof(value));

            // Проверяем формат пути (должен состоять из идентификаторов, разделенных точками)
            var parts = value.Split('.');
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    throw new ArgumentException("Путь подразделения содержит пустые части.", nameof(value));
            }

            return new DepartmentPath(value);
        }

        public static DepartmentPath CreateForRoot(string identifier)
        {
            return Create(identifier);
        }

        public static DepartmentPath CreateForChild(DepartmentPath parentPath, string childIdentifier)
        {
            var newPath = $"{parentPath.Value}.{childIdentifier}";
            return Create(newPath);
        }

        public short CalculateDepth()
        {
            return (short)Value.Count(c => c == '.');
        }

        public string GetParentPath()
        {
            var lastDotIndex = Value.LastIndexOf('.');
            return lastDotIndex > 0 ? Value.Substring(0, lastDotIndex) : string.Empty;
        }
    }
}