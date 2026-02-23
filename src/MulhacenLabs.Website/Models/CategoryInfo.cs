namespace MulhacenLabs.Website.Models
{
    public class CategoryInfo
    {
        public required string Slug { get; init; }
        public required string DisplayName { get; init; }
        public required string Description { get; init; }
        public required string Color { get; init; }
        public required string IconSvg { get; init; }
        public required string ExploreDescription { get; init; }
    }
}
