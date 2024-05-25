using System.ComponentModel.DataAnnotations;

namespace lyricboxd.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Username { get; set; }

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(225)]
        public string Password { get; set; }
    }
}
