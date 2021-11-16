using DatabaseLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseLayer
{
    public static class DatabaseLayerDependencies
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DatabaseRepository>();
            services.AddScoped<TableRepository>();
            services.AddScoped<ColumnRepository>();
            services.AddScoped<ColumnDataRepository>();
            services.AddScoped<ValidateRepository>();
            services.AddScoped<TablesDifferenceRepository>();
            
            services.AddScoped<TableProjectionRepository>();
        }
    }
}