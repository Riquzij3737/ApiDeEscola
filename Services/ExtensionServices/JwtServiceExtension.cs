using ApiDeEscola.Services.Implementations;

namespace ApiDeEscola.Services.ConfigServices
{
    public static class JwtServiceExtension
    {
        public static IServiceCollection addJwtService(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, jwtService>();            

            return services;
        }
    }
}
