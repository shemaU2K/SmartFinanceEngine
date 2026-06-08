using Microsoft.Extensions.DependencyInjection;
using SFE.Domain.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;

namespace SFE.Application
{
    /// <summary>
    /// Provides extension methods for registering application layer services 
    /// into the Microsoft dependency injection (IoC) container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all required application-tier services, such as MediatR handlers, 
        /// behaviors, and validators, bounded to this assembly.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance being built.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> to allow for method chaining.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Scans the current executing SFE.Application assembly and registers all matching IRequest/IRequestHandler implementations.
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}