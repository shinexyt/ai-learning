using AIStudySite.Module.Models;
using AIStudySite.Module.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Html.Models;
using OrchardCore.Title.Models;

namespace AIStudySite.Module.Controllers;

[ApiController]
[Route("api/agent")]
public class AgentUploadController : ControllerBase
{
    private readonly IContentManager _contentManager;
    private readonly AgentUploadOptions _options;
    private readonly ILogger<AgentUploadController> _logger;

    public AgentUploadController(
        IContentManager contentManager,
        IOptions<AgentUploadOptions> options,
        ILogger<AgentUploadController> logger)
    {
        _contentManager = contentManager;
        _options = options.Value;
        _logger = logger;
    }

    [HttpPost("upload-tutorial")]
    public async Task<IActionResult> UploadTutorialAsync([FromBody] AgentUploadRequest request)
    {
        if (!_options.Enabled)
        {
            return Forbid();
        }

        if (!Request.Headers.TryGetValue("X-Agent-Token", out var token) || token != _options.ApiToken)
        {
            return Unauthorized(new { message = "Missing or invalid agent token" });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var contentItem = await _contentManager.NewAsync("TutorialArticle");
        contentItem.DisplayText = request.Title;

        var titlePart = contentItem.As<TitlePart>();
        titlePart.Title = request.Title;
        contentItem.Apply(titlePart);

        var htmlBody = contentItem.As<HtmlBodyPart>();
        htmlBody.Html = request.Body;
        contentItem.Apply(htmlBody);

        var localization = contentItem.As<LocalizationPart>();
        localization.Culture = request.Culture;
        contentItem.Apply(localization);

        var autoroute = contentItem.As<AutoroutePart>();
        autoroute.Path = $"tutorials/{Slugify(request.Title)}";
        contentItem.Apply(autoroute);

        ApplyTextField(contentItem, "KnowledgeNodeId", request.NodeId);
        ApplyTextField(contentItem, "Culture", request.Culture);

        await _contentManager.CreateAsync(contentItem, OrchardCore.ContentManagement.VersionOptions.Published);

        _logger.LogInformation("Agent uploaded tutorial {Title} ({ContentItemId})", request.Title, contentItem.ContentItemId);

        return Created($"/tutorials/{autoroute.Path}", new { contentItem.ContentItemId, autoroute.Path });
    }

    private static void ApplyTextField(ContentItem contentItem, string fieldName, string value)
    {
        var part = contentItem.Content["TutorialArticlePart"] as JObject ?? new JObject();
        part[fieldName] = new JObject
        {
            ["Text"] = value
        };
        contentItem.Content["TutorialArticlePart"] = part;
    }

    private static string Slugify(string text)
    {
        var normalized = text.ToLowerInvariant();
        normalized = string.Concat(normalized.Select(ch => char.IsLetterOrDigit(ch) ? ch : '-'));
        while (normalized.Contains("--"))
        {
            normalized = normalized.Replace("--", "-");
        }
        return normalized.Trim('-');
    }
}
