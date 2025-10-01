using System.Text;

namespace TaskManager.Domain.ValueObjects;

public class PasswordHash : IEquatable<PasswordHash>
{
    public string Value { get; private set; }

    private PasswordHash() { Value = string.Empty; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Hash da senha não pode ser vazio", nameof(value));

        Value = value;
    }

    public static PasswordHash Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Senha não pode ser vazia", nameof(password));

        if (password.Length < 6)
            throw new ArgumentException("Senha deve ter pelo menos 6 caracteres", nameof(password));

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return new PasswordHash(hash);
    }

    public bool Verify(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        return BCrypt.Net.BCrypt.Verify(password, Value);
    }

    public bool Equals(PasswordHash? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PasswordHash);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return "[REDACTED]";
    }

    public static bool operator ==(PasswordHash? left, PasswordHash? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PasswordHash? left, PasswordHash? right)
    {
        return !Equals(left, right);
    }
}
