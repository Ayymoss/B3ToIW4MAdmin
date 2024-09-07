using System.Collections.Concurrent;
using B3ToIW4MAdmin.Context;
using B3ToIW4MAdmin.Utilities;
using Data.Context;
using Data.Models;
using Data.Models.Client;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace B3ToIW4MAdmin;

public class AppEntry(SourceContext sourceContext, DatabaseContext destContext)
{
    private readonly ConcurrentDictionary<long, EFClient> _clients = [];
    private readonly ConcurrentBag<EFPenalty> _penalties = [];
    private EFClient? _iw4MAdminClient;

    private static readonly Dictionary<string, EFPenalty.PenaltyType> PenaltyArray = new()
    {
        ["Kick"] = EFPenalty.PenaltyType.Kick,
        ["Ban"] = EFPenalty.PenaltyType.Ban,
        ["TempBan"] = EFPenalty.PenaltyType.TempBan,
        ["Warning"] = EFPenalty.PenaltyType.Warning
    };

    public async Task Run()
    {
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [yellow]Applying migrations...[/]");
        await destContext.Database.MigrateAsync();
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [green]Migrations applied[/]");

        await IW4MAdminSeedAsync();
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [green]Added IW4MAdmin seed[/]");

        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [yellow]Processing clients...[/]");
        await LoadClientsAsync();
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [green]Saved clients[/]");

        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [yellow]Processing penalties...[/]");
        await LoadPenaltiesAsync();
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [green]Saved penalties[/]");

        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [yellow]Saving...[/]");
        await destContext.SaveChangesAsync();
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [green]Saved[/]");
        AnsiConsole.MarkupLine($"[[[aqua]{DateTimeOffset.UtcNow:HH:mm:ss.fff}[/]]] [green]Migration complete![/]\n");
    }

    private async Task LoadPenaltiesAsync()
    {
        var penalties = await sourceContext.Penalties
            .AsNoTracking()
            .Where(x => x.ClientId != 0)
            .Where(x => PenaltyArray.Keys.Contains(x.Type))
            .ToListAsync();

        Parallel.ForEach(penalties, penalty =>
        {
            if (!_clients.TryGetValue(penalty.ClientId, out var offender)) return;
            if (!_clients.TryGetValue(penalty.AdminId, out var punisher)) return;

            var dateTimeAdded = DateTimeOffset.FromUnixTimeSeconds(penalty.TimeAdd).DateTime;

            var expires = penalty.TimeExpire is -1
                ? (DateTime?)null
                : DateTimeOffset.FromUnixTimeSeconds(penalty.TimeExpire).DateTime;

            var type = PenaltyArray[penalty.Type];
            var active = !(penalty.Inactive is 1 && type is EFPenalty.PenaltyType.TempBan or EFPenalty.PenaltyType.Ban);

            var newPenalty = new EFPenalty
            {
                Active = active,
                Link = offender.AliasLink,
                Offender = offender,
                Punisher = penalty.AdminId is 0 ? null : punisher,
                PunisherId = penalty.AdminId is 0 ? 1 : 0,
                When = dateTimeAdded,
                Expires = penalty.TimeExpire is 0 ? dateTimeAdded : expires,
                Offense = penalty.Reason,
                Type = type,
            };

            _penalties.Add(newPenalty);
        });

        destContext.Penalties.AddRange(_penalties);
    }

    private async Task LoadClientsAsync()
    {
        var clients = await sourceContext.Clients
            .Where(x => x.Guid != "WORLD")
            .AsNoTracking()
            .ToListAsync();

        Parallel.ForEach(clients.DistinctBy(x => x.Guid), client =>
        {
            if (!ulong.TryParse(client.Guid!, out var ulongGuid)) return;

            var aliasLink = new EFAliasLink();
            var dateTimeAdded = DateTimeOffset.FromUnixTimeSeconds(client.TimeAdd).DateTime;
            var name = client.Name.CapClientName(24);

            var newClient = new EFClient
            {
                NetworkId = (long)ulongGuid,
                Connections = (int)client.Connections,
                TotalConnectionTime = 0,
                FirstConnection = dateTimeAdded,
                LastConnection = DateTimeOffset.FromUnixTimeSeconds(client.TimeEdit).DateTime,
                GameName = Reference.Game.IW3,
                AliasLink = aliasLink,
                Level = EFClient.Permission.User,
                CurrentAlias = new EFAlias
                {
                    Link = aliasLink,
                    Name = name,
                    SearchableName = name.ToLower(),
                    IPAddress = client.Ip.ConvertToIp(),
                    DateAdded = dateTimeAdded
                }
            };

            _clients.TryAdd(client.Id, newClient);
        });

        destContext.Clients.AddRange(_clients.Values.Where(x => x.ClientId != 1));
    }

    private async Task IW4MAdminSeedAsync()
    {
        _iw4MAdminClient = await destContext.Clients.SingleOrDefaultAsync(x => x.ClientId == 1);
        if (_iw4MAdminClient is null)
        {
            var link = new EFAliasLink();
            _iw4MAdminClient = new EFClient
            {
                Active = false,
                Connections = 0,
                FirstConnection = DateTime.UtcNow,
                LastConnection = DateTime.UtcNow,
                Level = EFClient.Permission.Console,
                Masked = true,
                NetworkId = 0,
                AliasLink = link,
                CurrentAlias = new EFAlias
                {
                    Link = link,
                    Active = true,
                    DateAdded = DateTime.UtcNow,
                    Name = "IW4MAdmin",
                },
            };

            destContext.Clients.Add(_iw4MAdminClient);
            await destContext.SaveChangesAsync();
        }

        _clients.TryAdd(0, _iw4MAdminClient);
    }
}
