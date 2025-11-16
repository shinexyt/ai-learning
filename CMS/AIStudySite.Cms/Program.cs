using AIStudySite.Cms.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<UICultureState>();
builder.Services.AddSingleton<LocalizedTextService>();
builder.Services.AddHttpClient();

builder.Services.AddOrchardCms(cms =>
{
    cms.AddSetupFeatures("OrchardCore.AutoSetup");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseOrchardCore();

app.Run();
