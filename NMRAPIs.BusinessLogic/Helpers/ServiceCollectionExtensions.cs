// <copyright file="ServiceCollectionExtensions.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;

    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to add the MediatR Pipleline handlers.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> service.</param>
        public static void AddMediatRPipelines(this IServiceCollection services)
        {
            string assemblyName = Assembly.GetEntryAssembly().FullName;
            if (!assemblyName.Contains("testhost"))
            {
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AttachContextUserPipelineBehavior<,>));
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationPipelineBehavior<,>));
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceLoggingBehavior<,>));
            }
        }

        /// <summary>
        /// Extension Method to add all the handler for a particular generic Type T Interface.
        /// </summary>
        /// <typeparam name="T">Generic Type.</typeparam>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> service.</param>
        /// <param name="assemblies">List of Assemblies to Scan for the Interface.</param>
        /// <param name="lifetime">Lifespan for the Service.</param>
        public static void RegisterAllTypesWithBaseInterface<T>(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                Type myType = type.GetInterfaces()[0];
                services.Add(new ServiceDescriptor(myType, type, lifetime));
            }
        }

        /// <summary>
        /// Extension method to add all the MediatR Authorization Handlers for the Authorization Pipeline behavior.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> service.</param>
        /// <param name="assemblies">List of Assemblies to Scan for the Interface.</param>
        /// <param name="lifetime">Lifespan for the Service.</param>
        public static void RegisterAuthorizationHandlers(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            // Could have more robust solution than hard-coding string. But will do for now.
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterface("IAuthorize`1") != null));
            foreach (var type in typesFromAssemblies)
            {
                Type myType = type.GetInterface("IAuthorize`1");
                services.Add(new ServiceDescriptor(myType, type, lifetime));
            }
        }
    }
}
