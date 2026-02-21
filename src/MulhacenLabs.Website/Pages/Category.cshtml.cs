using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Models;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ContentService _contentService;

        private static readonly Dictionary<string, (string DisplayName, string Description)> CategoryMeta = new()
        {
            { "plugins", ("Plugins", "Professional audio plugins for music production") },
            { "releases", ("Releases", "Music releases like albums, singles, and EPs") },
            { "sample-packs", ("Sample Packs", "High quality samples for your productions") },
            { "events", ("Events", "Live shows and appearances") },
            { "blogs", ("Blog", "Tutorials, insights and updates from the lab") }
        };

        public PagedResult<CardItem> Result { get; set; } = new();
        public string CategorySlug { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public CategoryModel(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IActionResult OnGet(string category, [FromQuery] int page = 1)
        {
            if (!ContentService.ValidCategories.Contains(category))
                return NotFound();

            CategorySlug = category;

            if (CategoryMeta.TryGetValue(category, out var meta))
            {
                DisplayName = meta.DisplayName;
                Description = meta.Description;
            }

            Result = _contentService.GetCardsPaged(category, page, 9);

            return Page();
        }
    }
}
