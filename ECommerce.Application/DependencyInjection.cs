using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IOrderDetailService,OrderDetailService>();
            // Add other services as needed

            return services;
        }
    }
}