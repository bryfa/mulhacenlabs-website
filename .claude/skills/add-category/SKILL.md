# /add-category — Add a new content category

Scaffold a new content category across the project.

## Arguments

`<slug>` — e.g., `/add-category tutorials`

## Steps

1. Parse the slug from the arguments. If missing, ask the user for it.
2. Ask the user for the following details (use AskUserQuestion or prompt interactively):
   - **Display name** (e.g., "Tutorials")
   - **Description** (e.g., "Step-by-step guides and walkthroughs")
   - **Explore description** (short version for the homepage grid, e.g., "Guides & walkthroughs")
   - **Color** — a Tailwind bg color class (e.g., `bg-teal-500`)
   - **Icon SVG path** — an SVG `<path>` element for the category icon (suggest one from Heroicons outline set)
3. Add a new entry to `CategoryRegistry.cs` in `src/MulhacenLabs.Website/Models/CategoryRegistry.cs`, following the existing pattern.
4. Create the content directory `src/MulhacenLabs.Website/Content/{slug}/` (add a `.gitkeep` file so the empty directory is tracked).
5. Follow the steps in CLAUDE.md "Adding a new category" section:
   - Add a section to `src/MulhacenLabs.Website/Pages/Index.cshtml` modelled on an existing section.
   - Add a colour entry in `src/MulhacenLabs.Website/Pages/Shared/_Card.cshtml` `categoryColors` dict if one exists.
   - Add nav links in `src/MulhacenLabs.Website/Pages/Shared/_Layout.cshtml` (both desktop and mobile menus).
   - If the color class is dynamic/programmatic, add it to the `safelist` in `tailwind.config.js`.
6. Run `dotnet build src/MulhacenLabs.Website` to verify the project compiles.
7. Report what was created and any manual steps remaining.
