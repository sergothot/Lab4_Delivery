namespace Lab4_Delivery.Domain;

public interface IOrderObserver
{
    void OnStateChanged(Order order, string previousState, string newState);
}

public sealed class Order
{
    private readonly List<OrderItem> _items;
    private readonly List<IOrderObserver> _observers = new();

    internal Order(IEnumerable<OrderItem> items, string? note, bool fastDelivery, string? preference, IPricingStrategy pricingStrategy)
    {
        Id = Guid.NewGuid();
        _items = items.ToList();
        Note = note ?? string.Empty;
        FastDelivery = fastDelivery;
        Preference = preference ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        State = new PreparingState();
        PricingStrategy = pricingStrategy;
    }

    public Guid Id { get; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public string Note { get; }
    public bool FastDelivery { get; }
    public string Preference { get; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IOrderState State { get; private set; }
    public IPricingStrategy PricingStrategy { get; private set; }

    public decimal Subtotal => _items.Sum(i => i.LineTotal);

    public void MoveToNextState()
    {
        var next = State.Next(this);
        SetState(next);
    }

    public void Cancel()
    {
        var next = State.Cancel(this);
        SetState(next);
    }

    public decimal CalculateTotal()
    {
        return PricingStrategy.CalculateTotal(this);
    }

    public void AddObserver(IOrderObserver observer)
    {
        _observers.Add(observer);
    }

    internal void SetPricingStrategy(IPricingStrategy strategy)
    {
        PricingStrategy = strategy;
    }

    private void SetState(IOrderState newState)
    {
        var previous = State;
        State = newState;
        UpdatedAt = DateTime.UtcNow;
        if (previous != newState)
        {
            Notify(previous.Name, newState.Name);
        }
    }

    private void Notify(string from, string to)
    {
        foreach (var observer in _observers)
        {
            observer.OnStateChanged(this, from, to);
        }
    }
}
