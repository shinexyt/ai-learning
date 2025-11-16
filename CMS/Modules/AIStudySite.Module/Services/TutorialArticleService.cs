using AIStudySite.Module.Indexing;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Html.Models;
using OrchardCore.Title.Models;
using YesSql;
using YesSql.Services;

namespace AIStudySite.Module.Services;

public class TutorialArticleService
{
    private readonly ISession _session;
    private readonly IContentManager _contentManager;

    public TutorialArticleService(ISession session, IContentManager contentManager)
    {
        _session = session;
        _contentManager = contentManager;
    }

    public async Task<IEnumerable<TutorialArticleSummary>> GetSummariesAsync(string culture, string? nodeId)
    {
        var query = _session.QueryIndex<TutorialArticleIndex>(index => index.Published && index.Culture == culture);

        if (!string.IsNullOrWhiteSpace(nodeId))
        {
            query = query.Where(index => index.KnowledgeNodeId == nodeId);
        }

        var results = await query.OrderByDescending(i => i.CreatedUtc).ListAsync();
        return results.Select(index => new TutorialArticleSummary
        {
            ContentItemId = index.ContentItemId,
            Title = index.DisplayText,
            Slug = index.AutoroutePath,
            NodeId = index.KnowledgeNodeId,
            Culture = index.Culture
        });
    }

    public async Task<TutorialArticleDetails?> GetDetailsBySlugAsync(string slug, string culture)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var normalizedSlug = slug.Trim('/');
        var index = await _session.QueryIndex<TutorialArticleIndex>(i => i.Published && i.AutoroutePath == normalizedSlug && i.Culture == culture)
            .FirstOrDefaultAsync();

        if (index == null)
        {
            return null;
        }

        var contentItem = await _contentManager.GetAsync(index.ContentItemId);
        if (contentItem == null)
        {
            return null;
        }

        var titlePart = contentItem.As<TitlePart>();
        var htmlBody = contentItem.As<HtmlBodyPart>();
        var localization = contentItem.As<LocalizationPart>();

        return new TutorialArticleDetails
        {
            ContentItemId = contentItem.ContentItemId,
            Title = titlePart?.Title ?? contentItem.DisplayText,
            Body = htmlBody?.Html ?? string.Empty,
            NodeId = index.KnowledgeNodeId,
            Slug = index.AutoroutePath,
            Culture = localization?.Culture ?? index.Culture
        };
    }
}

public record TutorialArticleSummary
{
    public string ContentItemId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string NodeId { get; init; } = string.Empty;
    public string Culture { get; init; } = string.Empty;
}

public record TutorialArticleDetails : TutorialArticleSummary
{
    public string Body { get; init; } = string.Empty;
}
