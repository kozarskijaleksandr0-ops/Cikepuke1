using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Department.ValueObjects
{
    public class DepartmentPath
    {
        public string Value { get; }
        private DepartmentPath(string value)
        {
            Value = value;
        }
        public static DepartmentPath Create(string value)
        {
            if (value == string.Empty)
            {
                throw new ArgumentException("Путь не может быть пустым.", nameof(value));
            }
            return new DepartmentPath(value);
        }
    }
}
