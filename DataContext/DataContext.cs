using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ApiDeEscola.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace ApiDeEscola.DataContext
{
    public class DataApplicationContext : DbContext
    {
        public DataApplicationContext(DbContextOptions<DataApplicationContext> options) : base(options) { }

        public DbSet<UsersModel> Users { get; set; }
        public DbSet<EmploymentModel> Employments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersModel>(x =>
            {
                x.HasKey(x => x.Id).HasName("PK_ID");
                x.Property(x => x.Id).HasColumnType("VARCHAR(45)").IsRequired();
                x.Property(x => x.Name).HasColumnType("TEXT").IsRequired();
                x.Property(x => x.Emaíl).HasColumnType("TEXT").IsRequired();
                x.Property(x => x.password).HasColumnType("TEXT").IsRequired();                                
                
                modelBuilder.Entity<EmploymentModel>(z =>
                {
                    z.HasKey(z => z.CPF).HasName("PK_CPF");
                    z.Property(z => z.CPF).HasColumnType("VARCHAR(11)").IsRequired();
                    z.Property(z => z.Role).HasColumnType("TEXT").IsRequired();
                    z.Property(z => z.Date_Employment).HasColumnType("DATE").IsRequired();
                    z.Property(z => z.Salary).HasColumnType("FLOAT").IsRequired();
                    z.Property(z => z.Name).HasColumnType("TEXT").IsRequired();
                    z.Property(z => z.idade).HasColumnType("int").IsRequired().HasMaxLength(110);
                });
            });

            base.OnModelCreating(modelBuilder);
        }
    }

    public class DataApplicationContextFactory : IDesignTimeDbContextFactory<DataApplicationContext>
    {
        public DataApplicationContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // pasta onde tu rodar o comando
                .AddJsonFile("appsettings.json")
                .Build();

            var connStr = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DataApplicationContext>();
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));

            return new DataApplicationContext(optionsBuilder.Options);
        }
    }

}
