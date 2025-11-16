using OrchardCore.Autoroute.Models;
using OrchardCore.Autoroute.Settings;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace AIStudySite.Module.Migrations;

public class TutorialArticleMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public TutorialArticleMigrations(IContentDefinitionManager contentDefinitionManager)
    {
        _contentDefinitionManager = contentDefinitionManager;
    }

    public int Create()
    {
        _contentDefinitionManager.AlterPartDefinition("TutorialArticlePart", part => part
            .WithField("KnowledgeNodeId", field => field
                .OfType("TextField")
                .WithDisplayName("Knowledge Node Id")
                .WithPosition("0")
                .WithSettings(new TextFieldSettings
                {
                    Required = true,
                    Hint = "Enter the knowledge tree node identifier"
                }))
            .WithField("Culture", field => field
                .OfType("TextField")
                .WithDisplayName("Culture")
                .WithPosition("1")
                .WithSettings(new TextFieldSettings
                {
                    Required = true,
                    Hint = "Use zh-CN or en-US"
                }))
            .WithField("Tags", field => field
                .OfType("TextField")
                .WithDisplayName("Tags")
                .WithPosition("2"))
        );

        _contentDefinitionManager.AlterTypeDefinition("TutorialArticle", type => type
            .DisplayedAs("Tutorial Article")
            .Creatable()
            .Listable()
            .Draftable()
            .Versionable()
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("AutoroutePart", part => part.WithPosition("1").WithSettings(new AutoroutePartSettings
            {
                AllowCustomPath = true,
                Pattern = "tutorials/{ContentItem.DisplayText}",
                ShowHomepageOption = false
            }))
            .WithPart("LocalizationPart", part => part.WithPosition("2"))
            .WithPart("HtmlBodyPart", part => part.WithPosition("3"))
            .WithPart("TutorialArticlePart", part => part.WithPosition("4"))
        );

        return 1;
    }
}
