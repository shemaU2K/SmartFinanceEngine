using Microsoft.Extensions.DependencyInjection;
using SFE.Domain.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;

namespace SFE.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
