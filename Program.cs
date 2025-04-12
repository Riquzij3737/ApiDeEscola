
using ApiDeEscola.DataContext;
using ApiDeEscola.Services.ConfigServices;
using ApiDeEscola.Services.ExtensionServices;
using ApiDeEscola.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ApiDeEscola
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connstr = builder.Configuration.GetConnectionString("DefaultConnection");
            var cnfg = new jwtService(builder.Configuration);

            // Add services to the container.            
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu_token}"
                });

                // Aplicar o esquema de seguran�a globalmente
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                 });
            });
            builder.Services.addJwtService();
            builder.Services.addSecurityCryptService();
            builder.Services.AddDbContext<DataApplicationContext>(x =>
            {
                x.UseMySql(connstr, ServerVersion.AutoDetect(connstr));
            });

            cnfg.Config(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    app.MapSwagger();

                    opt.DocumentTitle = "Minha Api";

                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();


        }
    }
}
