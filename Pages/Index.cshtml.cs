using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Models;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages;

public class IndexModel : PageModel
    {
        private readonly ContentService _contentService;
        public List<CardItem> Plugins { get; set; } = new();

        public IndexModel(ContentService contentService)
        {
            _contentService = contentService;
        }

        public void OnGet()
        {
            Plugins = _contentService.GetCards("plugins");
        }
    }
