global using BlazorEcommerce_V2.Shared;
global using System.Net.Http.Json;
global using BlazorEcommerce_V2.Client.Services.CategoryService;
global using BlazorEcommerce_V2.Client.Services.ProductService;
global using BlazorEcommerce_V2.Client.Services.CartService;
global using BlazorEcommerce_V2.Client.Services.AuthService;

using BlazorEcommerce_V2.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();   

await builder.Build().RunAsync();
