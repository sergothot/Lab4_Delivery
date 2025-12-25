namespace Lab4_Delivery.Domain;

public sealed class OrderBuilder
{
    private readonly List<OrderItem> _items = new();
    private string? _note;
    private bool _fastDelivery;
    private string? _preference;

    public OrderBuilder AddItem(MenuItem item, int quantity)
    {
        _items.Add(new OrderItem(item, quantity));
        return this;
    }

    public OrderBuilder WithNote(string note)
    {
        _note = note;
        return this;
    }

    public OrderBuilder WithFastDelivery(bool fast = true)
    {
        _fastDelivery = fast;
        return this;
    }

    public OrderBuilder WithPreference(string preference)
    {
        _preference = preference;
        return this;
    }

    public Order Build(IPricingStrategy? pricingStrategy = null)
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Заказ должен содержать хотя бы один элемент.");
        }

        var strategy = pricingStrategy ?? new StandardPricingStrategy();
        return new Order(_items, _note, _fastDelivery, _preference, strategy);
    }
}
