﻿using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDist.Core.Configuration;
using PDist.Core.Contracts;
using PDist.Core.HostServices;

namespace PDist.Runner;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        services.AddSingleton<IRunner, PrimaryRunner>();

        Trace.Listeners.Add(new ConsoleTraceListener());

        AddConfigurations(services);
    }

    private void AddConfigurations(IServiceCollection services)
    {
        services.AddOptions<ServerOptions>().Configure<IConfiguration>((option, config) => config.GetSection("server").Bind(option));
    }
}