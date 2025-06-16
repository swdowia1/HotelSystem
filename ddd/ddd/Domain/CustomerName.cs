using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ddd.Domain
{
    public sealed class CustomerName : IEquatable<CustomerName>
    {
        public string Value { get; }
        public CustomerName()
        {
            
        }
        public CustomerName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("nazwa nie moze byc pusta");

            if (value.Length > 100)
                throw new ArgumentException("jest zbyt dluga");

            Value = value;
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) => Equals(obj as CustomerName);
        public bool Equals(CustomerName? other) => other != null && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
