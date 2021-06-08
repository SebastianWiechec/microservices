using Microsoft.Extensions.DependencyInjection;
using UserApi.IServices;

namespace UserApi.Sevices
{

    public static class MyServices
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<IUserService, UserService>();
    }
}


