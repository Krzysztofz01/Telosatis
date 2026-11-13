namespace Telosatis.Common;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; }
    
    protected Entity() => Id = Guid.NewGuid();

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return GetType() == other.GetType()
            && Id != Guid.Empty
            && other.Id != Guid.Empty
            && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity other && Equals(other);
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    public override string ToString()
    {
        return $"{GetType().Name} {Id}";
    }

    public static explicit operator Guid(Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        return entity.Id;
    }
}
