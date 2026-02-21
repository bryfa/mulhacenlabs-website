using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Models;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages;

public class IndexModel : PageModel
    {
        private readonly ContentService _contentService;
        public List<CardItem> Plugins { get; set; } = new();
        public List<CardItem> Releases { get; set; } = new();
        public List<CardItem> SamplePacks { get; set; } = new();
        public List<CardItem> Events { get; set; } = new();
        public List<CardItem> Blogs { get; set; } = new();

        public IndexModel(ContentService contentService)
        {
            _contentService = contentService;
        }

        public void OnGet()
        {
            Plugins = _contentService.GetCards("plugins").Take(3).ToList();
            Releases = _contentService.GetCards("releases").Take(3).ToList();
            SamplePacks = _contentService.GetCards("sample-packs").Take(3).ToList();
            Events = _contentService.GetCards("events").Take(3).ToList();
            Blogs = _contentService.GetCards("blogs").Take(3).ToList();
        }
    }
