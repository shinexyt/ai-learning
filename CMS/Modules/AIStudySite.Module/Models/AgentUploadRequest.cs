using System.ComponentModel.DataAnnotations;

namespace AIStudySite.Module.Models;

public class AgentUploadRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    [Required]
    [RegularExpression("(zh-CN|en-US)", ErrorMessage = "Culture must be zh-CN or en-US")]
    public string Culture { get; set; } = "en-US";

    [Required]
    public string NodeId { get; set; } = string.Empty;
}
