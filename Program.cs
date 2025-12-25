using Lab4_Delivery.Domain;
using Lab4_Delivery.Services;

var burger = new MenuItem("Бургер", 8.5m, "основное");
var fries = new MenuItem("Картофель фри", 3m, "гарнир");

var factory = new OrderFactory();
var order = factory.CreateFastDelivery(builder =>
{
	builder.AddItem(burger, 1)
		   .AddItem(fries, 2)
		   .WithPreference("Больше кетчупа");
});

var service = new OrderService();
service.PlaceOrder(order);

Console.WriteLine($"Заказ {order.Id} начинается в состоянии: {order.State.Name}");
Console.WriteLine($"Итог с учетом сборов: {service.CalculateTotal(order.Id):C}");