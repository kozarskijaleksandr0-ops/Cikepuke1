

namespace Domain.Position.ValueObjects
{
    public class Id
    {
        public Guid Value { get; }

        private Id(Guid value)
        {
            Value = value;
        }
        public static Id Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Идентификатор не может быть пустым.", nameof(value));
            }

            return new Id(value);
        }
    }
}
