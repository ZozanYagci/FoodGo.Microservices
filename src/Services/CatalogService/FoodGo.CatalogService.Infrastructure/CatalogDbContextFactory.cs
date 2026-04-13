using FoodGo.CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace FoodGo.CatalogService.Infrastructure
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional:false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionBuilder = new DbContextOptionsBuilder<CatalogDbContext>();

            
            optionBuilder.UseSqlServer(connectionString);

            return new CatalogDbContext(optionBuilder.Options);
        }
    }
}
