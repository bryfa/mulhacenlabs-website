namespace MulhacenLabs.Website.Models
{
    public static class CategoryRegistry
    {
        public static readonly IReadOnlyList<CategoryInfo> All = new List<CategoryInfo>
        {
            new()
            {
                Slug = "plugins",
                DisplayName = "Plugins",
                Description = "Professional audio plugins for music production",
                Color = "bg-red-500",
                IconSvg = "<path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z\"/>",
                ExploreDescription = "Audio tools for production"
            },
            new()
            {
                Slug = "releases",
                DisplayName = "Releases",
                Description = "Music releases like albums, singles, and EPs",
                Color = "bg-blue-500",
                IconSvg = "<path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M9 19V6l12-3v13M9 19c0 1.105-1.343 2-3 2s-3-.895-3-2 1.343-2 3-2 3 .895 3 2zm12-3c0 1.105-1.343 2-3 2s-3-.895-3-2 1.343-2 3-2 3 .895 3 2z\"/>",
                ExploreDescription = "Albums, singles & EPs"
            },
            new()
            {
                Slug = "sample-packs",
                DisplayName = "Sample Packs",
                Description = "High quality samples for your productions",
                Color = "bg-yellow-400",
                IconSvg = "<path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10\"/>",
                ExploreDescription = "Sounds for your projects"
            },
            new()
            {
                Slug = "events",
                DisplayName = "Events",
                Description = "Live shows and appearances",
                Color = "bg-green-500",
                IconSvg = "<path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z\"/>",
                ExploreDescription = "Live shows & appearances"
            },
            new()
            {
                Slug = "blogs",
                DisplayName = "Blog",
                Description = "Tutorials, insights and updates from the lab",
                Color = "bg-purple-500",
                IconSvg = "<path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z\"/>",
                ExploreDescription = "Tutorials & insights"
            },
            new()
            {
                Slug = "videos",
                DisplayName = "Videos",
                Description = "Watch our latest video content, tutorials, and demonstrations",
                Color = "bg-rose-600",
                IconSvg = "<path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M14.752 11.168l-3.197-2.132A1 1 0 0010 9.87v4.263a1 1 0 001.555.832l3.197-2.132a1 1 0 000-1.664z\"/><path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M21 12a9 9 0 11-18 0 9 9 0 0118 0z\"/>",
                ExploreDescription = "Video tutorials & demos"
            }
        };

        public static readonly IReadOnlyDictionary<string, CategoryInfo> BySlug =
            All.ToDictionary(c => c.Slug, c => c, StringComparer.OrdinalIgnoreCase);
    }
}
