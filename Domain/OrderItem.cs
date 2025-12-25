namespace Lab4_Delivery.Domain;

public sealed class OrderItem
{
    public OrderItem(MenuItem item, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть положительным.");
        }

        MenuItem = item ?? throw new ArgumentNullException(nameof(item));
        Quantity = quantity;
    }

    public MenuItem MenuItem { get; }
    public int Quantity { get; }
    public decimal LineTotal => MenuItem.Price * Quantity;
}
