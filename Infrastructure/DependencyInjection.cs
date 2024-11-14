using Domain.Interfaces.JukeBox;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services.JukeBox;
using Infrastructure.Services.POS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Extensions;
using Infrastructure.Repositories.JukeBox;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddUmbracoDbContext<GastroOnContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("umbracoDbDSN"));
        });
        services.AddUmbracoDbContext<JukeBoxDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("umbracoDbDSN"));
        });
        services.AddScoped<IPOSService, POSService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddTransient<IJukeBoxService, JukeBoxService>();
        services.AddTransient<IJukeBoxRepository, JukeBoxRepository>();
        services.AddTransient<IWishListService, WishListService>();
        services.AddTransient<IWishlistRepository, WishListRepository>();
        services.AddSignalR();
        services.AddTransient<ISoundTrackService, SoundtrackService>();
        services.AddSingleton<IGlobalListService, GlobalListService>();
        return services;
    }
}