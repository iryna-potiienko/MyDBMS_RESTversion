using DatabaseLayer.IRepositories;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseLayer
{
    public static class DatabaseLayerDependencies
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository, MyDBMSContext>();
            services.AddDbContext<MyDBMSContext>();
            
            services.AddScoped<DatabaseRepository>();
            services.AddScoped<TableRepository>();
            services.AddScoped<ColumnRepository>();
            services.AddScoped<ColumnDataRepository>();
            services.AddScoped<ValidateRepository>();
            services.AddScoped<TablesDifferenceRepository>();
            
            services.AddScoped<TableProjectionRepository>();
            
            services.AddScoped<IDatabaseRepository, DatabaseRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<ITableProjectionRepository,TableProjectionRepository>();
        }
    }
}