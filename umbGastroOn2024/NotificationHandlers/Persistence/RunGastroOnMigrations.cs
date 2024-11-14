using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace umbGastroOn2024.NotificationHandlers.Persistence;

public class RunGastroOnMigrations : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly GastroOnContext _context;
    private readonly ILogger<RunGastroOnMigrations> _logger;

    public RunGastroOnMigrations(GastroOnContext context, ILogger<RunGastroOnMigrations> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for pending migrations...");
        IEnumerable<string> pendingMigrations = await _context.Database.GetPendingMigrationsAsync(cancellationToken);

        if (pendingMigrations.Any())
        {
            await _context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Migrations completed.");
            return;
        }

        _logger.LogInformation("No pending migrations found.");
    }
}