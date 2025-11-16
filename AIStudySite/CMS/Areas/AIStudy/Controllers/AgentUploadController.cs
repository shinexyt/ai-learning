using AIStudySite.Cms.Areas.AIStudy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;
using OrchardCore.Html.Models;
using OrchardCore.Autoroute.Models;
using OrchardCore.Localization.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AIStudySite.Cms.Areas.AIStudy.Controllers;

[ApiController]
[Area("AIStudy")]
[Route("api/agent")]
public class AgentUploadController : ControllerBase
{
    private readonly IContentManager _contentManager;
    private readonly IConfiguration _configuration;

    public AgentUploadController(IContentManager contentManager, IConfiguration configuration)
    {
        _contentManager = contentManager;
        _configuration = configuration;
    }

    [HttpPost("upload-tutorial")]
    [AllowAnonymous]
    public async Task<IActionResult> UploadAsync([FromBody] TutorialUploadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (!Request.Headers.TryGetValue("X-Agent-Token", out var token) || token != _configuration["AgentUpload:ApiToken"])
        {
            return Unauthorized("Missing or invalid API token.");
        }

        var contentItem = await _contentManager.NewAsync("TutorialArticle");
        contentItem.DisplayText = request.Title;
        contentItem.Alter<TitlePart>(part => part.Title = request.Title);
        contentItem.Alter<HtmlBodyPart>(part => part.Html = request.Body);
        contentItem.Alter<LocalizationPart>(part => part.Culture = request.Culture);
        contentItem.Alter<AutoroutePart>(part => part.Path = $"tutorials/{request.NodeId}-{Guid.NewGuid():N}");

        contentItem.Content["TutorialMetadata"] = JObject.FromObject(new
        {
            request.NodeId,
            Tags = request.Tags ?? Array.Empty<string>()
        });

        await _contentManager.CreateAsync(contentItem, OrchardCore.ContentManagement.VersionOptions.Latest);
        return Ok(new { contentItemId = contentItem.ContentItemId });
    }
}
