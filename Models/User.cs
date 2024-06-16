using System.ComponentModel.DataAnnotations;

namespace lyricboxd.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(225)]
        public string Password { get; set; } = string.Empty;
    }
}
