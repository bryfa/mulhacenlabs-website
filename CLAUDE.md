# CLAUDE.md — Project Context for Claude Code

This file gives Claude Code the context needed to work effectively on this project. Keep it up to date as the project evolves.

---

## What this project is

**Mulhacen Labs Website** — portfolio and showcase site for a Spain-based audio technology company. It displays content across five categories: plugins, albums, sample packs, events, and blog posts.

The design goal is a simple, clean site that is easy to maintain via markdown files — no database, no admin panel.

---

## Stack summary

- **Backend:** ASP.NET Core 10, Razor Pages, C#
- **CMS:** File-based. Markdown + YAML frontmatter in `/Content/{category}/*.md`
- **Markdown:** Markdig (advanced extensions enabled)
- **YAML:** YamlDotNet with camelCase naming convention
- **CSS:** Tailwind CSS v3 (source: `Styles/tailwind.css`, output: `wwwroot/css/site.css`)
- **JS:** Vanilla ES6, minimal — only `wwwroot/js/site.js` (mobile menu toggle)
- **Namespace:** `MulhacenLabs.Website`

---

## Key files

| File | Role |
|---|---|
| `Services/ContentService.cs` | Core CMS: reads and parses markdown files |
| `Models/CardItem.cs` | Data model for all content items |
| `Pages/Shared/_Card.cshtml` | Reusable card UI partial |
| `Pages/Shared/_Layout.cshtml` | Site layout, nav, footer |
| `Pages/Index.cshtml(.cs)` | Homepage, renders all category sections |
| `Pages/Item.cshtml(.cs)` | Detail page for a single content item |
| `tailwind.config.js` | Tailwind scans `Pages/**/*.cshtml` |
| `package.json` | npm scripts for Tailwind build |

---

## Conventions

### C# / ASP.NET

- Namespace: `MulhacenLabs.Website` (and sub-namespaces like `.Models`, `.Services`)
- Nullable reference types are enabled — use `?` and null-checks where needed
- `ContentService` is a singleton — do not store per-request state in it
- Content is read from disk on every request (no caching yet) — avoid heavy computation in `ContentService`
- Razor Pages convention: page model in `Pages/Foo.cshtml.cs`, view in `Pages/Foo.cshtml`

### CMS / Content

- Each content type lives in its own subdirectory under `Content/`
- Filename becomes the URL slug (lowercase, hyphenated)
- All frontmatter fields are camelCase in YAML (mapped by YamlDotNet)
- Required frontmatter fields: `title`, `description`, `date`
- Optional: `image`, `tags`, `featured`
- Do not add new frontmatter fields without updating `Models/CardItem.cs`

### CSS / Tailwind

- Never write custom CSS unless Tailwind utilities are genuinely insufficient
- All class scanning happens from `.cshtml` files — add new dynamic classes to `tailwind.config.js` safelist if they are built programmatically in C# strings
- Run `npm run dev:css` when working on styles — it watches and rebuilds automatically
- Commit `wwwroot/css/site.css` — it is the built output and is needed for deployment

### JavaScript

- Keep JS minimal and vanilla (no frameworks)
- Add new scripts to `wwwroot/js/site.js` or create new files in `wwwroot/js/` referenced from `_Layout.cshtml`

---

## Development workflow

```bash
# Install npm dependencies (first time)
npm install

# Watch Tailwind (run in a separate terminal)
npm run dev:css

# Run the site
dotnet run

# Production CSS build
npm run build:css:prod

# Publish
dotnet publish -c Release -o ./publish
```

---

## Adding content

1. Drop a `.md` file into `Content/{category}/`
2. Add YAML frontmatter at the top between `---` delimiters
3. Write Markdown body after the second `---`
4. No server restart needed

---

## Adding a new category

1. Create `Content/{new-category}/`
2. Add a section to `Pages/Index.cshtml` modelled on an existing section
3. Add a colour entry in `Pages/Shared/_Card.cshtml` `categoryColors` dict
4. Add a nav link in `Pages/Shared/_Layout.cshtml` (both desktop and mobile menus)

---

## Testing strategy (planned)

The project has no tests yet. When adding tests:

- Use **xUnit** for unit tests
- Create a separate test project: `MulhacenLabs.Website.Tests/`
- Priority test targets:
  - `ContentService.ParseMarkdownFile()` — frontmatter parsing edge cases
  - `ContentService.GetCards()` — missing directory, empty directory, malformed files
  - Razor Page model `OnGet()` methods — verify correct data loading
- For integration tests, use `Microsoft.AspNetCore.Mvc.Testing` with `WebApplicationFactory`

---

## Known issues / TODOs

- `.gitignore` — was regenerated and is clean; keep `wwwroot/css/site.css` tracked
- Contact form does not send email yet — needs SMTP or transactional email service wired up
- No image assets yet — cards fall back to category colour blocks
- `ContentService` reads files synchronously — consider async file I/O if content volume grows
- No caching on `ContentService.GetCards()` — add `IMemoryCache` if performance becomes a concern

---

## Re-use as a template

This project is designed to be reproducible for other sites. See `ARCHITECTURE.md` for the template pattern. Key things to change per-site:

1. Company name in `_Layout.cshtml` (logo, footer copyright)
2. Nav links and category list
3. Content in `Content/`
4. Colour scheme in `tailwind.config.js` (extend theme)
5. `RootNamespace` in the `.csproj`
6. `launchSettings.json` ports if running multiple sites locally
