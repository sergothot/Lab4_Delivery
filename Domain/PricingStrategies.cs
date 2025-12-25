namespace Lab4_Delivery.Domain;

public interface IPricingStrategy
{
    decimal CalculateTotal(Order order);
}

public sealed class StandardPricingStrategy : IPricingStrategy
{
    private const decimal DeliveryFee = 4m;
    private const decimal TaxRate = 0.10m;

    public decimal CalculateTotal(Order order)
    {
        var tax = order.Subtotal * TaxRate;
        return order.Subtotal + DeliveryFee + tax;
    }
}

public sealed class DiscountPricingStrategy : IPricingStrategy
{
    private const decimal DeliveryFee = 4m;
    private const decimal TaxRate = 0.10m;
    private const decimal DiscountRate = 0.10m;

    public decimal CalculateTotal(Order order)
    {
        var discount = order.Subtotal * DiscountRate;
        var discounted = order.Subtotal - discount;
        var tax = discounted * TaxRate;
        return discounted + DeliveryFee + tax;
    }
}

public sealed class ExpeditedPricingStrategy : IPricingStrategy
{
    private const decimal DeliveryFee = 6m;
    private const decimal RushFee = 6m;
    private const decimal TaxRate = 0.10m;

    public decimal CalculateTotal(Order order)
    {
        var tax = order.Subtotal * TaxRate;
        return order.Subtotal + DeliveryFee + RushFee + tax;
    }
}
