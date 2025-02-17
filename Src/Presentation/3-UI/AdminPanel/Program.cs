using Serilog;
using TaskoMask.Infrastructure.Data.ReadModel.DataProviders;
using TaskoMask.Infrastructure.Data.WriteModel.DataProviders;
using TaskoMask.Presentation.Framework.Web.Configuration.Startup;

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddMvcProjectConfigureServices(builder.Configuration, builder.Environment);
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMvcProjectConfigure(app.Services, builder.Environment);
    WriteDbSeedData.SeedAdminPanelTempData(app.Services);
    ReadDbSeedData.SyncAdminPanelTempData(app.Services);
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=login}/{id?}");

    });
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


