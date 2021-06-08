using CostsApi.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace CostsApi.Services
{
    public static class MyServices
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<ICostsService, CostsService>();
    }
}
