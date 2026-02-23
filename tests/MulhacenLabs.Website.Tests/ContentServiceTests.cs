using Microsoft.AspNetCore.Hosting;
using MulhacenLabs.Website.Services;
using NSubstitute;

namespace MulhacenLabs.Website.Tests;

public class ContentServiceTests : IDisposable
{
    private readonly string _tempDir;
    private readonly ContentService _service;

    public ContentServiceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"content-tests-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDir);

        var env = Substitute.For<IWebHostEnvironment>();
        env.ContentRootPath.Returns(_tempDir);

        _service = new ContentService(env);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
        GC.SuppressFinalize(this);
    }

    private void WriteContentFile(string category, string fileName, string content)
    {
        var dir = Path.Combine(_tempDir, "Content", category);
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, fileName), content);
    }

    [Fact]
    public void GetCards_ReturnsEmpty_WhenCategoryDirectoryDoesNotExist()
    {
        var cards = _service.GetCards("nonexistent");

        Assert.Empty(cards);
    }

    [Fact]
    public void GetCards_ReturnsEmpty_WhenDirectoryHasNoMarkdownFiles()
    {
        Directory.CreateDirectory(Path.Combine(_tempDir, "Content", "plugins"));

        var cards = _service.GetCards("plugins");

        Assert.Empty(cards);
    }

    [Fact]
    public void GetCards_ParsesFrontmatterAndContent()
    {
        WriteContentFile("plugins", "test-plugin.md",
            "---\ntitle: Test Plugin\ndescription: A test plugin\ndate: 2025-01-15\nfeatured: true\ntags:\n  - synth\n  - effect\n---\nThis is the **body** content.\n");

        var cards = _service.GetCards("plugins");

        var card = Assert.Single(cards);
        Assert.Equal("Test Plugin", card.Title);
        Assert.Equal("A test plugin", card.Description);
        Assert.Equal(new DateTime(2025, 1, 15), card.Date);
        Assert.True(card.Featured);
        Assert.Equal(["synth", "effect"], card.Tags);
        Assert.Equal("test-plugin", card.Slug);
        Assert.Equal("plugins", card.Category);
        Assert.Contains("<strong>body</strong>", card.Content);
    }

    [Fact]
    public void GetCards_SortsByDateDescending()
    {
        WriteContentFile("blog", "old-post.md",
            "---\ntitle: Old Post\ndescription: An older post\ndate: 2024-06-01\n---\nOld content.\n");

        WriteContentFile("blog", "new-post.md",
            "---\ntitle: New Post\ndescription: A newer post\ndate: 2025-03-01\n---\nNew content.\n");

        var cards = _service.GetCards("blog");

        Assert.Equal(2, cards.Count);
        Assert.Equal("New Post", cards[0].Title);
        Assert.Equal("Old Post", cards[1].Title);
    }

    [Fact]
    public void GetCards_SkipsMalformedFiles()
    {
        WriteContentFile("blog", "good.md",
            "---\ntitle: Good Post\ndescription: Valid post\ndate: 2025-01-01\n---\nContent here.\n");

        // No frontmatter delimiters
        WriteContentFile("blog", "bad.md", "Just some plain text with no frontmatter.");

        var cards = _service.GetCards("blog");

        var card = Assert.Single(cards);
        Assert.Equal("Good Post", card.Title);
    }

    [Fact]
    public void GetCards_HandlesMinimalFrontmatter()
    {
        WriteContentFile("events", "minimal.md",
            "---\ntitle: Minimal Event\ndescription: Just the basics\ndate: 2025-05-20\n---\n");

        var cards = _service.GetCards("events");

        var card = Assert.Single(cards);
        Assert.Equal("Minimal Event", card.Title);
        Assert.False(card.Featured);
        Assert.Empty(card.Tags);
        Assert.Equal(string.Empty, card.Image);
    }

    [Fact]
    public void GetCards_ParsesYoutubeId()
    {
        WriteContentFile("videos", "demo-video.md",
            "---\ntitle: Demo Video\ndescription: A demo\nyoutubeId: abc123XYZ\ndate: 2026-01-10\n---\nWatch above.\n");

        var cards = _service.GetCards("videos");

        var card = Assert.Single(cards);
        Assert.Equal("abc123XYZ", card.YoutubeId);
    }

    [Fact]
    public void GetCards_YoutubeIdIsNullWhenNotInFrontmatter()
    {
        WriteContentFile("plugins", "no-video.md",
            "---\ntitle: A Plugin\ndescription: No video here\ndate: 2026-01-01\n---\nContent.\n");

        var cards = _service.GetCards("plugins");

        var card = Assert.Single(cards);
        Assert.Null(card.YoutubeId);
    }

    [Fact]
    public void GetCards_SkipsFilesWithMalformedYaml()
    {
        WriteContentFile("blog", "good.md",
            "---\ntitle: Good Post\ndescription: Valid\ndate: 2025-01-01\n---\nContent.\n");

        // Invalid YAML (mapping value not allowed here)
        WriteContentFile("blog", "malformed-yaml.md",
            "---\ntitle: [unclosed bracket\ndate: 2025-01-01\n---\nContent.\n");

        var cards = _service.GetCards("blog");

        var card = Assert.Single(cards);
        Assert.Equal("Good Post", card.Title);
    }

    [Fact]
    public void GetCardsPaged_ReturnsCorrectPage()
    {
        for (var i = 1; i <= 5; i++)
            WriteContentFile("blog", $"post-{i:D2}.md",
                $"---\ntitle: Post {i}\ndescription: Desc\ndate: 2025-01-{i:D2}\n---\n");

        var result = _service.GetCardsPaged("blog", page: 1, pageSize: 3);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(5, result.TotalItems);
        Assert.False(result.HasPreviousPage);
        Assert.True(result.HasNextPage);
    }

    [Fact]
    public void GetCardsPaged_ClampsOutOfRangePageNumber()
    {
        WriteContentFile("blog", "post.md",
            "---\ntitle: Only Post\ndescription: Desc\ndate: 2025-01-01\n---\n");

        var resultLow = _service.GetCardsPaged("blog", page: -5, pageSize: 9);
        var resultHigh = _service.GetCardsPaged("blog", page: 99, pageSize: 9);

        Assert.Equal(1, resultLow.CurrentPage);
        Assert.Equal(1, resultHigh.CurrentPage);
    }

    [Fact]
    public void GetCardsPaged_ReturnsEmptyPageForMissingCategory()
    {
        var result = _service.GetCardsPaged("nonexistent", page: 1, pageSize: 9);

        Assert.Empty(result.Items);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.TotalItems);
    }
}
