using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Models;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages
{
    public class ItemModel : PageModel
    {
        private readonly ContentService _contentService;
        public CardItem? Card { get; set; }

        public ItemModel(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IActionResult OnGet(string category, string slug)
        {
            var cards = _contentService.GetCards(category);
            Card = cards.FirstOrDefault(c => c.Slug == slug);

            if (Card == null)
                return NotFound();

            return Page();
        }
    }
}