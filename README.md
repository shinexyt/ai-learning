# AI Study Site

This repository hosts a proof-of-concept AI learning portal that combines **Orchard Core CMS** with a **Blazor Server** front end. The MVP delivers:

- AI knowledge tree homepage rendered as a reusable Blazor component backed by local JSON.
- Multi-lingual tutorial articles authored through Orchard Core (`TutorialArticle` content type) and consumed via Blazor pages.
- REST API (`/api/agent/upload-tutorial`) secured by a token header for automated article ingestion.
- Dark/Light theme toggle with `localStorage` persistence plus Tailwind-inspired styling.
- Localization for English (`en-US`) and Simplified Chinese (`zh-CN`) including UI text, menu labels, and Orchard `.po` resources.
- Orchard Core membership with registration enabled, custom `FreeUser` and `PaidUser` roles, and a Blazor page protected by `[Authorize(Roles="PaidUser")]`.

## Project structure

```
AIStudySite.sln
└── CMS
    ├── AIStudySite.Cms           # OrchardCore host + Blazor Server front-end
    ├── Modules/AIStudySite.Module # Custom content definitions, API, indexing
    └── Themes/AIStudySite.Theme   # Custom dual-theme styles for CMS pages
```

## Prerequisites

- .NET 8 SDK (the solution references Orchard Core 1.8.x packages).
- SQLite (default development database) – no extra configuration needed.

## Getting started

1. **Restore and build**
   ```bash
   dotnet restore AIStudySite.sln
   dotnet build AIStudySite.sln
   ```
2. **Run the CMS host**
   ```bash
   cd CMS/AIStudySite.Cms
   dotnet run
   ```
3. Navigate to `https://localhost:7052` (or the port printed in the console). Orchard Core will guide you through setup—select the `AI Tutorial Site` recipe to enable all features automatically. Provide admin credentials and keep the default SQLite connection string (`App_Data/ai-study.db`).
4. Once setup completes you can:
   - Browse `/` for the Blazor-powered knowledge tree (supports culture toggle + dark mode).
   - Visit `/articles` to query published `TutorialArticle` items filtered by knowledge node.
   - Access `/pro` with an account that belongs to the `PaidUser` role.
   - Use the Orchard dashboard to author localized tutorials.
   - POST to `/api/agent/upload-tutorial` with header `X-Agent-Token` to ingest content programmatically.

### Example API request

```bash
curl -X POST https://localhost:7052/api/agent/upload-tutorial \
  -H "Content-Type: application/json" \
  -H "X-Agent-Token: ChangeMeSuperSecretToken" \
  -d '{
    "title": "What is a multimodal model?",
    "body": "<p>Multimodal models fuse text, images, audio...</p>",
    "culture": "en-US",
    "nodeId": "llm-multimodal"
  }'
```

### Multi-language + theming

- `wwwroot/data/knowledge-tree.en-US.json` and `.zh-CN.json` drive the localized knowledge tree.
- `LocalizedTextService` powers UI labels; Orchard `.po` files cover CMS surfaces.
- `ThemeToggle` component + `wwwroot/js/themeToggle.js` persists user preference and respects the OS scheme.

### Users & roles

- Enable registration in the Orchard admin (already part of the recipe).
- New accounts join `FreeUser` automatically; upgrade via the admin panel to `PaidUser` to unlock the protected `/pro` page.

## Limitations & next steps

- GraphQL endpoints, payment gateways, and media/video modules are not part of this MVP.
- The knowledge tree currently loads from static JSON; hooking it into Orchard content items is a natural next iteration.
- API authentication relies on a shared token header—swap for OAuth or signed requests for production.

Happy learning! 🤖
