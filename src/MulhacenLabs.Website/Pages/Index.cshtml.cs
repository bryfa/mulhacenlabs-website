using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Models;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages;

public class IndexModel : PageModel
    {
        private readonly ContentService _contentService;
        public Dictionary<string, List<CardItem>> CategoryItems { get; set; } = new();

        public IndexModel(ContentService contentService)
        {
            _contentService = contentService;
        }

        public void OnGet()
        {
            foreach (var cat in CategoryRegistry.All)
            {
                CategoryItems[cat.Slug] = _contentService.GetCards(cat.Slug).Take(3).ToList();
            }
        }
    }
