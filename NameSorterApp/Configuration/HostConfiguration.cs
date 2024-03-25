using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NameSorterCore;
using NameSorterCore.Models;
using NameSorterCore.Interfaces;
using NameSorterCore.Sorters;
using NameSorterCore.Validators;
using NameSorterCore.Services;

namespace NameSorterApp.Configuration;

public static class HostConfiguration
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            // Bind configuration to settings object
            var appSettings = new AppSettings();
            hostContext.Configuration.GetSection("AppSettings").Bind(appSettings);

            // Register settings as a singleton
            services.AddSingleton(appSettings);

            // Register dependencies more dynamically to facilitate easy swapping and testing.
            RegisterServices(services);
        });

    private static void RegisterServices(IServiceCollection services)
    {
        // FileSystem and NameValidator dependencies are straightforward and likely won't change, 
        // making them suitable for singleton registration.
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<INameValidator, NameValidator>();

        // For components that are part of the core functionality but might have alternate implementations,
        // consider using scoped or transient lifetimes depending on use case.
        services.AddScoped<IFileProcessor, NameFileProcessor>();

        // Dynamically configure the sorting strategy to allow for easy changes in sorting behavior.
        // This allows the application to adapt to new requirements without modifying the service registration code.
        services.AddScoped<ISortStrategy<PersonName>>(provider =>
        {
            // This lambda could be replaced with logic to select a sorting strategy based on configuration or other criteria.
            return new LinqSortStrategy<PersonName>(
                primarySortCriteria: name => name.LastName,
                secondarySortCriteria: name => string.Join(" ", name.GivenNames));
        });

        // NameSorter requires a specific sort strategy and name parser, which are injected here.
        // This setup emphasizes the flexibility of injecting specific implementations.
        services.AddScoped<INameSorter, NameSorter>();
        services.AddScoped<INameParser, NameParser>();

        services.AddScoped<INameSorterService, NameSorterService>();
    }
}

