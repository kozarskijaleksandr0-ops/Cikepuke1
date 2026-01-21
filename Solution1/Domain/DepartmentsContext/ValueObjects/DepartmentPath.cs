using System;
using System.Linq;

namespace DirectoryService.Domain.DepartmentsContext.ValueObjects
{
    public sealed record DepartmentPath
    {
        public string Value { get; }
        public int Depth { get; }

        private DepartmentPath(string value, int depth)
        {
            Value = value;
            Depth = depth;
        }

        public static DepartmentPath CreateForRoot(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Идентификатор не может быть пустым.", nameof(identifier));

            return new DepartmentPath(identifier, 0);
        }

        public static DepartmentPath CreateForChild(string parentPath, string childIdentifier)
        {
            if (string.IsNullOrWhiteSpace(parentPath))
                throw new ArgumentException("Путь родителя не может быть пустым.", nameof(parentPath));

            if (string.IsNullOrWhiteSpace(childIdentifier))
                throw new ArgumentException("Идентификатор ребенка не может быть пустым.", nameof(childIdentifier));

            var newPath = $"{parentPath}.{childIdentifier}";
            var depth = newPath.Count(c => c == '.'); // Подсчет точек для определения глубины

            return new DepartmentPath(newPath, depth);
        }

        public string GetParentPath()
        {
            if (Depth == 0) return string.Empty;

            var lastDotIndex = Value.LastIndexOf('.');
            return lastDotIndex > 0 ? Value.Substring(0, lastDotIndex) : string.Empty;
        }
    }
}