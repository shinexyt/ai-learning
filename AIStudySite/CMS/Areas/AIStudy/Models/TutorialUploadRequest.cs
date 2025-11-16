using System.ComponentModel.DataAnnotations;

namespace AIStudySite.Cms.Areas.AIStudy.Models;

public class TutorialUploadRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(en-US|zh-CN)$", ErrorMessage = "Culture must be en-US or zh-CN")]
    public string Culture { get; set; } = "en-US";

    [Required]
    public string NodeId { get; set; } = string.Empty;

    public string[] Tags { get; set; } = Array.Empty<string>();
}
