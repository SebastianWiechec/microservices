using Microsoft.Extensions.DependencyInjection;
using TransactionApi.IServices;

namespace TransactionApi.Services
{
    public static class MyServices
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<ITransactionService, TransactionService>();
    }
}
