using API.Services.Interfaces;
using API.Services.Services;

namespace API.Services
{
    public static class Main
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDivisionService, DivisionService>();
        }
    }
}