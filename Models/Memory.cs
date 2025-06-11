using Memories.Models;
using System.ComponentModel.DataAnnotations;

public class Memory
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Text { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public List<MediaFile> MediaFiles { get; set; } = new();
}
