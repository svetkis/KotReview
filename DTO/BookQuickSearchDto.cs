namespace KotReview.DTO;

public sealed class BookQuickSearchDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Publisher { get; set; } = default!;
    public string[] Authors { get; set; } = Array.Empty<string>();
}
