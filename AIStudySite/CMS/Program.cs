using AIStudySite.Cms.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

builder.Services.AddLocalization(options => options.ResourcesPath = "Localization");
builder.Services.AddRazorPages();
builder.Services
    .AddOrchardCms()
    .AddSetupFeatures("OrchardCore.AutoSetup")
    .AddMvc();



builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<KnowledgeTreeLoader>();

var app = builder.Build();

var supportedCultures = new[] { "en-US", "zh-CN" };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.MapDefaultControllerRoute();
//app.MapBlazorHub();

app.MapGet("/", context =>
{
    context.Response.Redirect("/KnowledgeTree", permanent: false);
    return Task.CompletedTask;
});

app.UseOrchardCore();

app.Run();
