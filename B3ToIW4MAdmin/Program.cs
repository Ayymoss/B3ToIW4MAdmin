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
            AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [italic gray]Loading config...[/]");
            var configurationSetup = new ConfigurationSetup();
            var configuration = configurationSetup.GetConfiguration();
            if (configuration is null) throw new Exception("Failed to load configuration. Delete the configuration and run this again");

            AnsiConsole.MarkupLine(
                $"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [italic gray]Setting PostgreSQL DB Context Options...[/]");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var postgresTargetOptions = new DbContextOptionsBuilder<PostgresqlDatabaseContext>()
                .UseNpgsql(configuration.DestinationConnectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            AnsiConsole.MarkupLine(
                $"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [italic gray]Setting MySQL DB Context Options...[/]");
            var mySqlTargetOptions = new DbContextOptionsBuilder<SourceContext>()
                .UseMySql(configuration.SourceConnectionString, ServerVersion.AutoDetect(configuration.SourceConnectionString))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [italic gray]Building Service Collection...[/]");
            var serviceCollection = new ServiceCollection()
                .AddSingleton<DatabaseContext>(new PostgresqlDatabaseContext(postgresTargetOptions))
                .AddSingleton(new SourceContext(mySqlTargetOptions))
                .AddSingleton(configuration)
                .AddSingleton<AppEntry>();

            AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [italic gray]Building Service Provider...[/]");
            var serviceProvider = serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<AppEntry>();

            AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [italic gray]Setup complete. Starting...[/]");
            await serviceProvider.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e}");
            Console.WriteLine("Migration failed. Press any key to exit.");
        }

        AnsiConsole.MarkupLine("[italic red]Press any key to exit.[/]");
        Console.ReadKey();
    }
}
