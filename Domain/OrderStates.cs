namespace Lab4_Delivery.Domain;

public interface IOrderState
{
    string Name { get; }
    IOrderState Next(Order order);
    IOrderState Cancel(Order order);
}

public sealed class PreparingState : IOrderState
{
    public string Name => "Готовится";

    public IOrderState Next(Order order)
    {
        return new OutForDeliveryState();
    }

    public IOrderState Cancel(Order order)
    {
        return new CancelledState();
    }
}

public sealed class OutForDeliveryState : IOrderState
{
    public string Name => "Доставляется";

    public IOrderState Next(Order order)
    {
        return new CompletedState();
    }

    public IOrderState Cancel(Order order)
    {
        return new CancelledState();
    }
}

public sealed class CompletedState : IOrderState
{
    public string Name => "Завершен";

    public IOrderState Next(Order order)
    {
        return this;
    }

    public IOrderState Cancel(Order order)
    {
        return this;
    }
}

public sealed class CancelledState : IOrderState
{
    public string Name => "Отменен";

    public IOrderState Next(Order order)
    {
        return this;
    }

    public IOrderState Cancel(Order order)
    {
        return this;
    }
}
