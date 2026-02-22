# /add-content — Create a new content item

Create a new markdown content file with frontmatter template.

## Arguments

`<category> <slug>` — e.g., `/add-content blogs my-new-post`

## Steps

1. Parse the category and slug from the arguments.
2. Read `src/MulhacenLabs.Website/Models/CategoryRegistry.cs` and validate the category slug exists (valid slugs: plugins, releases, sample-packs, events, blogs). If invalid, list valid categories and stop.
3. Check if `src/MulhacenLabs.Website/Content/{category}/{slug}.md` already exists. If it does, warn the user and stop.
4. Create the file `src/MulhacenLabs.Website/Content/{category}/{slug}.md` with the following template:

```markdown
---
title: ""
description: ""
date: {today's date in YYYY-MM-DD format}
image: ""
tags: []
featured: false
---

Content goes here.
```

5. Tell the user the file was created and remind them to fill in the `title` and `description` fields.
