namespace Domain.ValueObjects;

public readonly record struct EntityId<T>(int Value)
{
    public static EntityId<T> Empty => new(0);
    public bool IsEmpty => Value == 0;
    public override string ToString() => Value.ToString();
    public static implicit operator int(EntityId<T> id) => id.Value;
    public static implicit operator EntityId<T>(int value) => new(value);
}