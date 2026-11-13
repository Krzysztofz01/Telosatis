using System.Reflection;

namespace Telosatis.Common;

public abstract class ValueObject<TValueObject> where TValueObject : ValueObject<TValueObject>
{
    private IEnumerable<PropertyInfo>? _properties;
    private IEnumerable<FieldInfo>? _fields;

    public static bool operator ==(ValueObject<TValueObject> a, ValueObject<TValueObject> b)
    {
        if (Equals(a, null))
        {
            if (Equals(b, null))
            {
                return true;
            }

            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject<TValueObject> a, ValueObject<TValueObject> b)
    {
        return !(a == b);
    }

    public bool Equals(ValueObject<TValueObject> obj)
    {
        return Equals(obj as object);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType()) return false;

        return GetProperties().All(p => PropertiesAreEqual(obj, p)) &&
               GetFields().All(f => FieldsAreEqual(obj, f));
    }

    private bool FieldsAreEqual(object obj, FieldInfo f)
    {
        return Equals(f.GetValue(this), f.GetValue(obj));
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    {
        return Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        if (_fields is null)
        {
            _fields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToList();
        }

        return _fields;
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        if (_properties is null)
        {
            _properties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();
        }

        return _properties;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        if (_properties is not null)
        {
            foreach (var propert in _properties)
            {
                hashCode.Add(propert);
            }
        }
        else
        {
            hashCode.Add(0);
        }

        if (_fields is not null)
        {
            foreach (var field in _fields)
            {
                hashCode.Add(field);
            }
        }
        else
        {
            hashCode.Add(0);
        }

        return hashCode.ToHashCode();
    }
}
