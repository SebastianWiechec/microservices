using Microsoft.Extensions.DependencyInjection;
using SpendingsApi.IServices;

namespace SpendingsApi.Services
{
    public static class MyServices
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<ISpendingsService, SpendingsService>()
                    .AddTransient<IEmailService, EmailService>();
    }
}