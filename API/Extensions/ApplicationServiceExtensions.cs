

using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddCors();
            services.AddScoped<IFakturaRepository, FakturaRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config
                    .GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}