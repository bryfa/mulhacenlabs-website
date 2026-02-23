namespace MulhacenLabs.Website.Models
{
    public class CardItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public DateTime Date { get; set; }
        public bool Featured { get; set; }
        public string? YoutubeId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}