using Newtonsoft.Json.Linq;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;
using YesSql.Indexes;

namespace AIStudySite.Module.Indexing;

public class TutorialArticleIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context)
    {
        context.For<TutorialArticleIndex>()
            .When(contentItem => contentItem.ContentType == "TutorialArticle")
            .Map(contentItem =>
            {
                var autoroute = contentItem.As<AutoroutePart>();
                var localization = contentItem.As<LocalizationPart>();
                var knowledgeNodeId = GetFieldValue(contentItem, "KnowledgeNodeId");

                return new TutorialArticleIndex
                {
                    ContentItemId = contentItem.ContentItemId,
                    DisplayText = contentItem.DisplayText,
                    Culture = localization?.Culture ?? "en-US",
                    KnowledgeNodeId = knowledgeNodeId,
                    AutoroutePath = autoroute?.Path ?? string.Empty,
                    Published = contentItem.Published,
                    CreatedUtc = contentItem.CreatedUtc
                };
            });
    }

    private static string GetFieldValue(ContentItem contentItem, string fieldName)
    {
        var part = contentItem.Content["TutorialArticlePart"] as JObject;
        var field = part?[fieldName] as JObject;
        return field?.Value<string>("Text") ?? string.Empty;
    }
}
