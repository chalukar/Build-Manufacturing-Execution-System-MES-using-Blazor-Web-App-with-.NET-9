using MES.Domain.Interfaces;
using MES.Infrastructure.Persistence;
using MES.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<MesDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("MesDatabase"),
                    sql => sql.MigrationsAssembly(typeof(MesDbContext).Assembly.FullName)));

            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            services.AddScoped<IWorkCentreRepository, WorkCentreRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
