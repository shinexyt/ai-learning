using YesSql.Indexes;

namespace AIStudySite.Module.Indexing;

public class TutorialArticleIndex : MapIndex
{
    public string ContentItemId { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
    public string Culture { get; set; } = string.Empty;
    public string KnowledgeNodeId { get; set; } = string.Empty;
    public string AutoroutePath { get; set; } = string.Empty;
    public bool Published { get; set; }
    public DateTime? CreatedUtc { get; set; }
}
