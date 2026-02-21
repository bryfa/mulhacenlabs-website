# Mulhacen Labs Website

Portfolio and showcase website for **Mulhacen Labs**, a Spain-based company specialising in audio plugins, music releases, sample packs, events, and blog content.

Built with ASP.NET Core 10 Razor Pages, a file-based headless CMS using Markdown + YAML frontmatter, and Tailwind CSS.

> **Status:** Under active construction.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Razor Pages) |
| CMS | File-based: Markdown + YAML frontmatter |
| Markdown parser | [Markdig](https://github.com/xoofx/markdig) v0.45 |
| YAML parser | [YamlDotNet](https://github.com/aaubry/YamlDotNet) v16.3 |
| CSS | [Tailwind CSS](https://tailwindcss.com/) v3.4 |
| JavaScript | Vanilla ES6 (minimal) |
| Runtime | .NET 10.0 |
| Node | npm (Tailwind build only) |

---

## Project Structure

```
mulhacenlabs-website/
├── src/
│   └── MulhacenLabs.Website/           # ASP.NET Core web project
│       ├── Content/                     # CMS content — Markdown files with YAML frontmatter
│       │   ├── blogs/
│       │   ├── events/
│       │   ├── plugins/
│       │   └── sample-packs/
│       ├── Models/
│       │   └── CardItem.cs             # Data model for all content items
│       ├── Pages/
│       │   ├── Shared/
│       │   │   ├── _Layout.cshtml      # Site-wide layout (nav, footer)
│       │   │   └── _Card.cshtml        # Reusable card partial component
│       │   ├── Index.cshtml(.cs)       # Homepage — all content sections
│       │   ├── Item.cshtml(.cs)        # Detail page — individual content item
│       │   ├── Contact.cshtml(.cs)     # Contact form
│       │   └── Privacy.cshtml(.cs)     # Privacy policy
│       ├── Services/
│       │   └── ContentService.cs       # CMS logic — reads, parses, and serves content
│       ├── Styles/
│       │   └── tailwind.css            # Tailwind source (compiled to wwwroot/css/site.css)
│       ├── wwwroot/                    # Static assets served publicly
│       │   ├── css/site.css            # Compiled Tailwind output
│       │   └── js/site.js              # Mobile hamburger menu
│       ├── Program.cs                  # App entry point and DI configuration
│       └── mulhacenlabs-website.csproj
├── tests/
│   └── MulhacenLabs.Website.Tests/     # xUnit test project
│       └── MulhacenLabs.Website.Tests.csproj
├── mulhacenlabs-website.sln            # Solution file (references both projects)
├── tailwind.config.js                  # Tailwind content paths
├── package.json                        # npm scripts for CSS build
└── README.md
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (for Tailwind CSS builds)

### Install dependencies

```bash
npm install
```

### Development

Run the CSS watcher and the .NET dev server in two terminals:

```bash
# Terminal 1 — watch and rebuild Tailwind CSS on changes
npm run dev:css

# Terminal 2 — start the ASP.NET Core dev server
dotnet run --project src/MulhacenLabs.Website
```

The site will be available at:
- HTTP: `http://localhost:5253`
- HTTPS: `https://localhost:7078`

### Production build

```bash
# Build optimised CSS (no watch, minified)
npm run build:css:prod

# Publish the .NET app
dotnet publish src/MulhacenLabs.Website -c Release -o ./publish
```

---

## CMS: Content Authoring

All content lives in `src/MulhacenLabs.Website/Content/{category}/` as `.md` files. Each file has a YAML frontmatter block followed by full Markdown body content.

### Content schema

```yaml
---
title: "Your Title"
description: "Short description shown on cards"
image: "/images/your-image.jpg"   # optional; falls back to category colour block
tags: ["tag1", "tag2"]
date: 2026-02-15                  # YYYY-MM-DD, used for ordering
featured: false                   # shows a "Featured" badge on the card
---

Full **Markdown** content rendered on the detail page.
```

### Categories

| Category | Directory | URL prefix |
|---|---|---|
| Plugins | `src/MulhacenLabs.Website/Content/plugins/` | `/item/plugins/{slug}` |
| Albums | `src/MulhacenLabs.Website/Content/albums/` | `/item/albums/{slug}` |
| Sample Packs | `src/MulhacenLabs.Website/Content/sample-packs/` | `/item/sample-packs/{slug}` |
| Events | `src/MulhacenLabs.Website/Content/events/` | `/item/events/{slug}` |
| Blog | `src/MulhacenLabs.Website/Content/blogs/` | `/item/blogs/{slug}` |

The `slug` is derived automatically from the filename (e.g. `getting-started-with-juce.md` → `getting-started-with-juce`).

### Adding a new content item

1. Create a `.md` file in the relevant category directory.
2. Add YAML frontmatter with the required fields.
3. Write the body content in Markdown below the closing `---`.
4. No restart required — `ContentService` reads files at request time.

To add a new category:
1. Create a new directory under `src/MulhacenLabs.Website/Content/`.
2. Add a section to `src/MulhacenLabs.Website/Pages/Index.cshtml` following the existing pattern.
3. Add a colour mapping entry in `src/MulhacenLabs.Website/Pages/Shared/_Card.cshtml`.

---

## Card System

The reusable `_Card.cshtml` partial renders any `CardItem` uniformly across all categories. Features:

- Category colour-coded image placeholder (falls back when no image provided)
- Title, description, tags, date, featured badge
- "Learn more" link to the detail page
- Hover shadow transition

Cards are rendered in a responsive CSS grid: 1 column (mobile) → 2 columns (tablet) → 3 columns (desktop).

---

## Routing

| Path | Page | Description |
|---|---|---|
| `/` | Index | Homepage with all category sections |
| `/item/{category}/{slug}` | Item | Full detail view for a content item |
| `/contact` | Contact | Contact form |
| `/privacy` | Privacy | Privacy policy |

---

## Development Notes

- `ContentService` is registered as a **singleton**. Content is read from disk on each request — no restart needed to see new content.
- Tailwind CSS is compiled separately via npm. Run `npm run dev:css` in parallel with `dotnet run`.
- The `.gitignore` excludes `bin/`, `obj/`, `node_modules/`, `.vs/`, `.idea/`, and `.DS_Store`.
- The generated `src/MulhacenLabs.Website/wwwroot/css/site.css` is committed to git so deploys do not require a Node.js build step on the server.
- Run tests with `dotnet test` from the repo root.

---

## Roadmap / Known TODOs

- [ ] Add contact form email delivery (currently logs/stubs only)
- [ ] Image upload and management for content items
- [ ] Pagination for content sections
- [ ] Search / filter by tag
- [ ] SEO meta tags (Open Graph, structured data)
- [ ] Expand test coverage (ContentService tests added, see `tests/`)
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Production deployment configuration

---

## Licence

All rights reserved. © 2026 Mulhacen Labs.
