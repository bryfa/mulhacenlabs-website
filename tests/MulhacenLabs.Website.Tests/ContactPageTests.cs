using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MulhacenLabs.Website.Pages;
using MulhacenLabs.Website.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MulhacenLabs.Website.Tests;

public class ContactPageTests
{
    private static ContactModel CreateModel(IEmailService emailService)
    {
        var logger = Substitute.For<ILogger<ContactModel>>();
        var model = new ContactModel(emailService, logger);

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new PageActionDescriptor(),
            new ModelStateDictionary());
        model.PageContext = new PageContext(actionContext);

        return model;
    }

    [Fact]
    public async Task OnPostAsync_SendsEmailAndShowsSuccessMessage_WhenFormIsValid()
    {
        var emailService = Substitute.For<IEmailService>();
        var model = CreateModel(emailService);
        model.Form = new ContactForm
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Subject = "Hello",
            Message = "Testing the contact form."
        };

        var result = await model.OnPostAsync();

        await emailService.Received(1).SendContactEmailAsync("Jane Doe", "jane@example.com", "Hello", "Testing the contact form.");
        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.Message);
        Assert.False(model.IsError);
        Assert.Equal(string.Empty, model.Form.Name); // form is cleared on success
    }

    [Fact]
    public async Task OnPostAsync_ShowsErrorMessage_WhenEmailServiceThrows()
    {
        var emailService = Substitute.For<IEmailService>();
        emailService
            .SendContactEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .ThrowsAsync(new Exception("SMTP failure"));

        var model = CreateModel(emailService);
        model.Form = new ContactForm
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Subject = "Hello",
            Message = "Testing."
        };

        var result = await model.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.Message);
        Assert.True(model.IsError);
    }

    [Fact]
    public async Task OnPostAsync_DoesNotSendEmail_WhenModelStateIsInvalid()
    {
        var emailService = Substitute.For<IEmailService>();
        var model = CreateModel(emailService);
        model.ModelState.AddModelError("Form.Name", "Required");

        await model.OnPostAsync();

        await emailService.DidNotReceive().SendContactEmailAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
}

public class EmailServiceTests
{
    [Fact]
    public async Task SendContactEmailAsync_CompletesWithoutThrowingWhenNotConfigured()
    {
        var config = new ConfigurationBuilder().Build(); // empty — no Email section
        var logger = Substitute.For<ILogger<EmailService>>();
        var service = new EmailService(config, logger);

        var ex = await Record.ExceptionAsync(() =>
            service.SendContactEmailAsync("Name", "test@example.com", "Subject", "Body"));

        Assert.Null(ex);
    }
}
