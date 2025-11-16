using System.Text.Json;

namespace AIStudySite.Cms.Services;

public class KnowledgeTreeLoader
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<KnowledgeTreeLoader> _logger;

    public KnowledgeTreeLoader(IWebHostEnvironment env, ILogger<KnowledgeTreeLoader> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<JsonDocument> LoadAsync(string culture)
    {
        var normalizedCulture = culture?.Equals("zh-CN", StringComparison.OrdinalIgnoreCase) == true
            ? "zh-CN"
            : "en-US";

        var path = Path.Combine(_env.ContentRootPath, "wwwroot", "data", $"knowledge-tree.{normalizedCulture}.json");
        if (!File.Exists(path))
        {
            _logger.LogWarning("Knowledge tree file {Path} was not found. Falling back to English.", path);
            path = Path.Combine(_env.ContentRootPath, "wwwroot", "data", "knowledge-tree.en-US.json");
        }

        await using var stream = File.OpenRead(path);
        return await JsonDocument.ParseAsync(stream);
    }
}
