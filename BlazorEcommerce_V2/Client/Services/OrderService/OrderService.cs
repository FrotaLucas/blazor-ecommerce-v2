﻿using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce_V2.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authStateProvider;
        public OrderService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient)
        {
            _authStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
            _http = httpClient;

        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
        public async Task PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                var response = await _http.PostAsync("api/order", null);
            }
            else
            {
                _navigationManager.NavigateTo("login");
            }
        }
    }
}
