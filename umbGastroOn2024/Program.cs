using Application.Common.JukeBox;
using Domain.Interfaces.JukeBox;
using Infrastructure;
using Infrastructure.Services.JukeBox;
using umbGastroOn2024;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

ConfigureBuilder(builder);
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

await app.BootUmbracoAsync();
ConfigureWebApplication(app);
await app.RunAsync();

static void ConfigureBuilder(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddMediatr();
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("JukeBoxSettings"));
    builder.CreateUmbracoBuilder()
        .AddBackOffice()
        .AddWebsite()
        .AddUmbraco()
        .AddDeliveryApi()
        .AddComposers()
        .Build();
}

static void ConfigureWebApplication(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseUmbraco()
        .WithMiddleware(u =>
        {
            u.UseBackOffice();
            u.UseWebsite();
        })
        .WithEndpoints(u =>
        {
            u.UseInstallerEndpoints();
            u.UseBackOfficeEndpoints();
            u.UseWebsiteEndpoints();
        });

    app.MapControllers();
    app.UseRouting();
    app.MapHub<SoundtrackService>("/jukeBoxHub");
}