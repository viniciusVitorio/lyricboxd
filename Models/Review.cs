using System.ComponentModel.DataAnnotations;

namespace lyricboxd.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SongId { get; set; }

        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        public string ReviewText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacionamentos de navegação (caso tenha modelos User e Song)
        public User User { get; set; }
        //public Song Song { get; set; }
    }
}
