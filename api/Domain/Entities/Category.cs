using System.Drawing;
using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category : IAggregateRoot
{
    public EntityId<Category> Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? IconUrl { get; private set; } = default!;
    public Color? Color { get; private set; }
    private Category() { }
    public static Category Create(string name, string? description = null, string? iconUrl = null, Color? color = null)
    {
        return new Category
        {
            Name = name,
            Description = description,
            IconUrl = iconUrl,
            Color = color
        };
    }
    public void Update(string? name, string? description = null, string? iconUrl = null, Color? color = null)
    {
        Name = name ?? Name;
        Description = description ?? Description;
        IconUrl = iconUrl ?? IconUrl;
        Color = color ?? Color;
    }
}