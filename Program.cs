
using ApiDeEscola.DataContext;
using ApiDeEscola.Models;
using ApiDeEscola.Models.Enums;
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

        public static EmploymentModel Select(ref EmploymentModel funcionario, JobsEnum jobs)
        {
            switch (jobs)
            {
                case JobsEnum.Teacher:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Teacher);
                    break;

                case JobsEnum.Janitor:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Janitor);
                    break;

                case JobsEnum.Monitor:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Monitor);
                    break;

                case JobsEnum.Safety:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Safety);
                    break;

                case JobsEnum.Children_Counselor:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Children_Counselor);
                    break;

                case JobsEnum.Pedagogue:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Pedagogue);
                    break;

                case JobsEnum.Cook:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Cook);
                    break;

                case JobsEnum.Librarian:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Librarian);
                    break;

                case JobsEnum.Director:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Director);
                    break;

                case JobsEnum.Coordinator:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Coordinator);
                    break;

                case JobsEnum.Deputy_director:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.Deputy_director);
                    break;

                default:
                    funcionario.Role = Enum.GetName<JobsEnum>(JobsEnum.None);
                    funcionario.Salary = 0;
                    break;


            }
            return funcionario;
        }
    }
}
