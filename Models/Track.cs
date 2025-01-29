using System.ComponentModel.DataAnnotations;

namespace lab7.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Artist { get; set; }

        [StringLength(100)]
        public string Album { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        public double Duration { get; set; }

        [StringLength(200)]
        [DataType(DataType.Url)]
        public string FileUrl { get; set; }

        private DateTime _createdAt;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime _updatedAt;
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}
