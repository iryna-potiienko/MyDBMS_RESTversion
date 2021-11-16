using DataLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WinFormsApp.Repositories;

namespace DataLayer
{
    public static class DataLayerDependencies
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DatabaseRepository>();
            services.AddScoped<TableRepository>();
            services.AddScoped<ColumnRepository>();
           // services.AddScoped<RowRepository>();
            //services.AddScoped<CellRepository>();
        }
    }
}