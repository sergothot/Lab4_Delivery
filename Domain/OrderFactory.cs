namespace Lab4_Delivery.Domain;

public sealed class OrderFactory
{
    public Order CreateStandard(Action<OrderBuilder> configure)
    {
        var builder = new OrderBuilder();
        configure(builder);
        return builder.Build(new StandardPricingStrategy());
    }

    public Order CreateFastDelivery(Action<OrderBuilder> configure)
    {
        var builder = new OrderBuilder();
        configure(builder);
        builder.WithFastDelivery();
        return builder.Build(new ExpeditedPricingStrategy());
    }

    public Order CreateDiscounted(Action<OrderBuilder> configure)
    {
        var builder = new OrderBuilder();
        configure(builder);
        return builder.Build(new DiscountPricingStrategy());
    }
}
