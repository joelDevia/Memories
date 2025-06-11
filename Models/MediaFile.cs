namespace Memories.Models
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public MediaType Type { get; set; }
        

        public int MemoryId { get; set; }
        public Memory Memory { get; set; } = null!;
    }

    public enum MediaType
    {
        Image,
        Audio,
        Video
    }
}
