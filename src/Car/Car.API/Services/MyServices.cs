using CarApi.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace CarApi.Services
{
    public static class MyServices
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<ICarService, CarService>();
    }
}
