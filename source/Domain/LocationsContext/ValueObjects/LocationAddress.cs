namespace Domain.LocationsContext.ValueObjects
{
    public sealed record LocationAddress
    {
        private readonly List<string> _addressParts = new();

        public string Value { get; }

        public IReadOnlyList<string> AddressParts => _addressParts.AsReadOnly();

        private LocationAddress(IEnumerable<string> parts)
        {
            _addressParts = parts.ToList();
            Value = string.Join(", ", parts);
        }

        public static LocationAddress Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Адрес локации не может быть пустым.", nameof(value));

            var parts = value
                .Split(',')
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToList();

            if (parts.Count == 0)
                throw new ArgumentException(
                    "Адрес локации должен содержать хотя бы одну часть.",
                    nameof(value));

            return new LocationAddress(parts);
        }

        public static LocationAddress Create(IEnumerable<string> parts)
        {
            if (!parts.Any())
                throw new ArgumentException(
                    "Адрес локации должен содержать хотя бы одну часть.",
                    nameof(parts));

            var trimmedParts = parts
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToList();

            if (trimmedParts.Count == 0)
                throw new ArgumentException(
                    "Адрес локации должен содержать хотя бы одну часть.",
                    nameof(parts));

            return new LocationAddress(trimmedParts);
        }
    }
}
