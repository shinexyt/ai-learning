# AI Study Site

This repository contains a minimum viable product for an AI tutorial learning website that combines **Orchard Core CMS** with a **Blazor Server** front end component. It is optimized for multilingual content (English + Chinese), dark/light themes, and integrations that allow agents to push content by calling a secured API.

## Projects

| Project | Description |
| --- | --- |
| `AIStudySite.Cms` | Orchard Core CMS host that exposes tutorial content, the knowledge tree Blazor component, and the secured upload API. |
| `CustomTheme` | Orchard Core theme with Tailwind-friendly CSS variables and dark/light toggling logic. |

## Prerequisites

* .NET 8 SDK
* SQLite (bundled with .NET, no external service needed)

## Getting started

```bash
cd AIStudySite/CMS
# Restore packages and run the CMS
# dotnet run
```

During the first boot Orchard Core runs the `ai-study` recipe automatically (via AutoSetup) and creates the SQLite database in `App_Data/ai-study.db`.

### Admin access

* URL: `https://localhost:5001/Admin`
* User: `admin`
* Password: `Admin@123`

### Knowledge tree page

Navigate to `/KnowledgeTree` to see the Cytoscape-powered visualization that is rendered inside a Blazor Server component. Knowledge node data is stored in `wwwroot/data/knowledge-tree.<culture>.json`.

### Tutorial articles

The recipe adds the `TutorialArticle` content type with `Title`, `HtmlBody`, `Localization`, and `Autoroute` parts. Authors can create localized variants through the Orchard Core admin UI.

### Agent upload API

```
POST /api/agent/upload-tutorial
Header: X-Agent-Token: sample-token
```

Body:

```json
{
  "title": "什么是多模态模型？",
  "body": "<p>多模态模型融合图像、文本、音频...</p>",
  "culture": "zh-CN",
  "nodeId": "llm-multimodal",
  "tags": ["featured"]
}
```

The request creates a `TutorialArticle` content item with localization metadata.

### User registration & roles

* Modules for `Users`, `Roles`, `Login`, and `Registration` are enabled by default.
* New registrations receive the `FreeUser` role.
* The `/PaidOnly` Razor page requires the `PaidUser` role and demonstrates permission enforcement.

### Localization resources

UI strings use Orchard Core localization with `.po` files stored in `Localization/`. The language picker in the theme switches between `en-US` and `zh-CN` via the `Culture` controller.

### Theme switching

The `CustomTheme` provides CSS variables plus `wwwroot/Themes/CustomTheme/Assets/js/theme-toggle.js` to persist the preferred theme in `localStorage` and respect `prefers-color-scheme`.

## Extending the MVP

* Replace the REST upload API with GraphQL.
* Synchronize knowledge tree data with CMS-managed content items.
* Integrate payment providers (Stripe / Alipay) to automatically grant the `PaidUser` role.
