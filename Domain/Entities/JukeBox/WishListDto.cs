using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JukeBox
{
    public class WishListDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string SoundZoneId { get; set; }
        [Required]
        [MaxLength(250)]
        public string TrackId { get; set; }
        [Required]
        [MaxLength(150)]
        public string AccountId { get; set; }
        [Required]
        public int UpVoteCount { get; set; }
    }
}
