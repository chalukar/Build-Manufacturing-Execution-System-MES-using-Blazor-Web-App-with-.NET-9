using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;


namespace MES.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            // AutoMapper 
            services.AddAutoMapper(cfg => { }, assembly);

            return services;


        }
    }
}
