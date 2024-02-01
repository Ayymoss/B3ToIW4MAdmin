using System.Reflection;
using System.Text.Json;
using B3ToIW4MAdmin.Models;
using Spectre.Console;

namespace B3ToIW4MAdmin.Utilities;

public class ConfigurationSetup
{
    private const string ConfigurationName = "B3MigrationConfiguration.json";
    private readonly JsonSerializerOptions _jsonOptions = new() {WriteIndented = true};

    public Configuration? GetConfiguration()
    {
#if DEBUG
        var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#else
        var workingDirectory = Directory.GetCurrentDirectory();
#endif
        if (workingDirectory is null) throw new NullReferenceException("Unable to retrieve working directory.");

        var configurationFile = Path.Combine(workingDirectory, ConfigurationName);
        if (!File.Exists(configurationFile))
        {
            SetConfiguration(workingDirectory);
            AnsiConsole.MarkupLine("[green]Configuration file created! Modify it then restart the application.[/]\n");
            AnsiConsole.MarkupLine("[italic red]Press any key to exit.[/]");
            Console.ReadKey();
            Environment.Exit(1);
        }

        var configurationJson = File.ReadAllText(configurationFile);
        var configuration = JsonSerializer.Deserialize<Configuration>(configurationJson);
        return configuration;
    }

    private void SetConfiguration(string workingDirectory)
    {
        var configuration = new Configuration
        {
            SourceConnectionString = string.Empty,
            DestinationConnectionString = string.Empty
        };

        var configurationJson = JsonSerializer.Serialize(configuration, _jsonOptions);
        var configurationFile = Path.Combine(workingDirectory, ConfigurationName);
        File.WriteAllText(configurationFile, configurationJson);
    }
}
