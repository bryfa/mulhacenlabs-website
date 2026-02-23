using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MulhacenLabs.Website.Services;

namespace MulhacenLabs.Website.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ContactModel> _logger;

        public ContactModel(IEmailService emailService, ILogger<ContactModel> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [BindProperty]
        public ContactForm Form { get; set; } = new();

        public string? Message { get; set; }
        public bool IsError { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _emailService.SendContactEmailAsync(Form.Name, Form.Email, Form.Subject, Form.Message);
                Message = "Thanks for reaching out! I'll get back to you soon.";
                ModelState.Clear();
                Form = new ContactForm();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact email from {Email}", Form.Email);
                Message = "Sorry, something went wrong sending your message. Please try again later.";
                IsError = true;
            }

            return Page();
        }
    }

    public class ContactForm
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
