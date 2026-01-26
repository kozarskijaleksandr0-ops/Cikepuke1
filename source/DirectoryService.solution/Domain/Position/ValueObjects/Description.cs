using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Position.ValueObjects
{
    public class Description
    {
        public string Value { get; }

        private Description(string value)
        {
            Value = value;
        }
        public static Description Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Значение было пустым.", nameof(value));

            if (value.Length > 500)
                throw new ArgumentException("Строка привышает длину символов.", nameof(value));

            return new Description(value);
        }
    }
}
