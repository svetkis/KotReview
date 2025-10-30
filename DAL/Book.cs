public class Book
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Publisher { get; set; } = "";
    public bool Blocked { get; set; } = false;
    public DateTime RegDate { get; set; } = DateTime.UtcNow;
    public List<Author> Authors { get; set; } = new();
}
