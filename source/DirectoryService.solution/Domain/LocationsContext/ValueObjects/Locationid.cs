using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LocationsContext.ValueObjects
{
    public sealed class Locationid
    {
        public Guid Value { get; }

        private Locationid(Guid value)
        {
            Value = value;
        }

        public static Locationid Create(Guid? value = null)
        {
            var id = value ?? Guid.NewGuid();

            if (id == Guid.Empty)
                throw new ArgumentException("Идентификатор локации не может быть пустым.", nameof(value));

            return new Locationid(id);
        }
    }
}
    

