using System;
using Lab4_Delivery.Domain;
using Lab4_Delivery.Services;
using Xunit;

namespace Lab4_Delivery.Tests;

public class OrderTests
{
    [Fact]
    public void PricingStrategies_ChangeTotals()
    {
        var burger = new MenuItem("Бургер", 10m);
        var fries = new MenuItem("Картофель фри", 5m);

        var builder = new OrderBuilder()
            .AddItem(burger, 1)
            .AddItem(fries, 2);

        var standardOrder = builder.Build(new StandardPricingStrategy());
        var discountOrder = builder.Build(new DiscountPricingStrategy());
        var expeditedOrder = builder.WithFastDelivery().Build(new ExpeditedPricingStrategy());

        Assert.Equal(26m, standardOrder.CalculateTotal());
        Assert.Equal(23.8m, discountOrder.CalculateTotal());
        Assert.Equal(34m, expeditedOrder.CalculateTotal());
    }

    [Fact]
    public void StateTransitions_FollowHappyPath()
    {
        var order = new OrderBuilder()
            .AddItem(new MenuItem("Суп", 7m), 1)
            .Build();

        Assert.Equal("Готовится", order.State.Name);

        order.MoveToNextState();
        Assert.Equal("Доставляется", order.State.Name);

        order.MoveToNextState();
        Assert.Equal("Завершен", order.State.Name);

        order.Cancel();
        Assert.Equal("Завершен", order.State.Name);
    }

    [Fact]
    public void FactoryCreatesConfiguredOrders()
    {
        var factory = new OrderFactory();

        var fastOrder = factory.CreateFastDelivery(b =>
        {
            b.AddItem(new MenuItem("Пицца", 12m), 1);
        });

        var discountedOrder = factory.CreateDiscounted(b =>
        {
            b.AddItem(new MenuItem("Салат", 8m), 1)
             .WithNote("Без заправки");
        });

        Assert.True(fastOrder.FastDelivery);
        Assert.Equal("Готовится", fastOrder.State.Name);
        Assert.True(discountedOrder.CalculateTotal() < fastOrder.CalculateTotal());
    }

    [Fact]
    public void ObserverGetsStateChangeNotifications()
    {
        var order = new OrderBuilder()
            .AddItem(new MenuItem("Сэндвич", 9m), 1)
            .Build();

        var observer = new RecordingObserver();
        order.AddObserver(observer);

        order.MoveToNextState();

        Assert.Equal("Готовится", observer.From);
        Assert.Equal("Доставляется", observer.To);
        Assert.Equal(order.Id, observer.OrderId);
    }

    [Fact]
    public void ServiceCalculatesTotals()
    {
        var factory = new OrderFactory();
        var service = new OrderService();

        var order = factory.CreateStandard(b =>
        {
            b.AddItem(new MenuItem("Ролл", 6m), 2);
        });

        service.PlaceOrder(order);
        var total = service.CalculateTotal(order.Id);

        Assert.Equal(order.CalculateTotal(), total);
    }

    private sealed class RecordingObserver : IOrderObserver
    {
        public Guid OrderId { get; private set; }
        public string From { get; private set; } = string.Empty;
        public string To { get; private set; } = string.Empty;

        public void OnStateChanged(Order order, string previousState, string newState)
        {
            OrderId = order.Id;
            From = previousState;
            To = newState;
        }
    }
}
