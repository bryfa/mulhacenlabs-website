using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Models;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ContentService _contentService;

        public PagedResult<CardItem> Result { get; set; } = new();
        public string CategorySlug { get; set; } = string.Empty;
        public CategoryInfo Category { get; set; } = null!;

        public CategoryModel(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IActionResult OnGet(string category, [FromQuery] int page = 1)
        {
            if (!CategoryRegistry.BySlug.TryGetValue(category, out var info))
                return NotFound();

            CategorySlug = category;
            Category = info;

            Result = _contentService.GetCardsPaged(category, page, 9);

            return Page();
        }
    }
}
