using Markdig;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using MulhacenLabs.Website.Models;

namespace MulhacenLabs.Website.Services
{
    public class ContentService
    {
        private readonly string _contentPath;
        private readonly MarkdownPipeline _pipeline;
        private readonly IDeserializer _deserializer;

        public static readonly HashSet<string> ValidCategories = new(StringComparer.OrdinalIgnoreCase)
        {
            "plugins", "releases", "sample-packs", "events", "blogs"
        };

        public ContentService(IWebHostEnvironment env)
        {
            _contentPath = Path.Combine(env.ContentRootPath, "Content");
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();
        }

        public List<CardItem> GetCards(string category)
        {
            var cards = new List<CardItem>();
            var categoryPath = Path.Combine(_contentPath, category);

            if (!Directory.Exists(categoryPath))
                return cards;

            foreach (var file in Directory.GetFiles(categoryPath, "*.md"))
            {
                var card = ParseMarkdownFile(file, category);
                if (card != null)
                    cards.Add(card);
            }

            return cards.OrderByDescending(c => c.Date).ToList();
        }

        public PagedResult<CardItem> GetCardsPaged(string category, int page, int pageSize)
        {
            var allCards = GetCards(category);
            var totalItems = allCards.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var items = allCards
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<CardItem>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };
        }

        private CardItem? ParseMarkdownFile(string filePath, string category)
        {
            var content = File.ReadAllText(filePath);
            var parts = content.Split("---", 3, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
                return null;

            var frontMatter = parts[0].Trim();
            var markdownContent = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            var card = _deserializer.Deserialize<CardItem>(frontMatter);
            card.Content = Markdown.ToHtml(markdownContent, _pipeline);
            card.Slug = Path.GetFileNameWithoutExtension(filePath);
            card.Category = category;

            return card;
        }
    }
}