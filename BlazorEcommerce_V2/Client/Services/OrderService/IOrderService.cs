﻿namespace BlazorEcommerce_V2.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> PlaceOrder();

        Task<List<OrderOverviewResponse>> GetOrders();

        Task<OrderDetailsResponse> GetOrderDetails(int orderId);
    }
}
