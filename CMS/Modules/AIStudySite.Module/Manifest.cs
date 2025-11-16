using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "AI Study Module",
    Author = "AI Study Team",
    Version = "1.0.0",
    Description = "Custom features for the AI tutorial learning site",
    Category = "Content",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.ContentFields",
        "OrchardCore.ContentLocalization",
        "OrchardCore.Html",
        "OrchardCore.Localization",
        "OrchardCore.Media",
        "OrchardCore.Users",
        "OrchardCore.Roles",
        "OrchardCore.Title"
    }
)]
