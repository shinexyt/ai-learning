using AIStudySite.Module.Indexing;
using AIStudySite.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using YesSql.Indexes;

namespace AIStudySite.Module;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions<AgentUploadOptions>()
            .BindConfiguration("AIStudySite:AgentUpload");

        services.AddSingleton<IIndexProvider, TutorialArticleIndexProvider>();
        services.AddScoped<TutorialArticleService>();
    }
}
