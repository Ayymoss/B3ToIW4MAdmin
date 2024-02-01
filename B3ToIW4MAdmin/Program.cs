using System.Reflection;
using B3ToIW4MAdmin.Context;
using B3ToIW4MAdmin.Utilities;
using Data.Context;
using Data.MigrationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace B3ToIW4MAdmin;

public static class Program
{
    public static async Task Main()
    {
        try
        {
            var configurationSetup = new ConfigurationSetup();
            var configuration = configurationSetup.GetConfiguration();
            if (configuration is null) throw new Exception("Failed to load configuration. Delete the configuration and run this again");

            var mySqlOptions = new DbContextOptionsBuilder<MySqlDatabaseContext>()
                .UseMySql(configuration.DestinationConnectionString, ServerVersion.AutoDetect(configuration.DestinationConnectionString))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            var serviceCollection = new ServiceCollection()
                .AddSingleton((DatabaseContext)new MySqlDatabaseContext(mySqlOptions))
                .AddSingleton(configuration)
                .AddSingleton<AppEntry>();

            serviceCollection.AddDbContext<SourceContext>(o =>
            {
                o.UseMySql(configuration.SourceConnectionString, ServerVersion.AutoDetect(configuration.SourceConnectionString));
                o.EnableSensitiveDataLogging();
            });

            await serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<AppEntry>().Run();
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [red]Exception: {e}[/]\n");
            AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [red]Migration failed![/]\n");
        }

        AnsiConsole.MarkupLine("[italic red]Press any key to exit.[/]");
        Console.ReadKey();
    }
}
