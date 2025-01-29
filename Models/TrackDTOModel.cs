using Microsoft.EntityFrameworkCore;
namespace lab7.Models

{
    public class TrackDTOModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public double Duration { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormattedDuration => TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss");
        public string TrackInfo => $"{Artist} - {Title}";
    }
}
