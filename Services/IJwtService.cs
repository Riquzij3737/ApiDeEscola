using ApiDeEscola.Models;

namespace ApiDeEscola.Services
{
    public interface IJwtService
    {
        public void Config(IServiceCollection services,IConfiguration jwtcfng);
        public string GenerationToken(EmploymentModel usr);
    }
}
