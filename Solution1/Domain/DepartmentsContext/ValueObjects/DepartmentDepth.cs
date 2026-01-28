using System;

namespace DirectoryService.Domain.DepartmentsContext.ValueObjects
{
    public sealed record DepartmentDepth
    {
        public const short MaxDepth = 10;

        public short Value { get; }

        private DepartmentDepth(short value)
        {
            Value = value;
        }

        public static DepartmentDepth Create(short value)
        {
            if (value < 0)
                throw new ArgumentException("Глубина подразделения не может быть отрицательной.", nameof(value));

            if (value > MaxDepth)
                throw new ArgumentException($"Глубина подразделения не может превышать {MaxDepth}.", nameof(value));

            return new DepartmentDepth(value);
        }

        public static DepartmentDepth CalculateFromPath(DepartmentPath path)
        {
            var depth = path.CalculateDepth();
            return Create(depth);
        }

        public DepartmentDepth Increment()
        {
            return Create((short)(Value + 1));
        }
    }
}