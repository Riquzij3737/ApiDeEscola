using ApiDeEscola.Services.Implementations;

namespace ApiDeEscola.Services.ExtensionServices
{
    public static class SecurityServiceExtension
    {
        public static IServiceCollection addSecurityCryptService(this IServiceCollection services)
        {
            services.AddScoped<ISecurityCrypService, SecurityCrypService>();

            return services;

        }        
    }
}
