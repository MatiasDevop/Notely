namespace Tags.Api.Data;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public Guid NoteId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
