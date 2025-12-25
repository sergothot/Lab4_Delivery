namespace Lab4_Delivery.Domain;

public sealed class MenuItem
{
    public MenuItem(string name, decimal price, params string[] tags)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Tags = tags ?? Array.Empty<string>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public IReadOnlyList<string> Tags { get; }
}
