using umbGastroOn2024.NotificationHandlers.Persistence;
using Umbraco.Cms.Core.Notifications;

namespace umbGastroOn2024;

public static class DependencyInjection
{
    public static IUmbracoBuilder AddUmbraco(this IUmbracoBuilder builder)
    {
        builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, RunGastroOnMigrations>();
        return builder;
    }

}