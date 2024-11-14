using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JukeBox
{
    public class VolumeDto
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string SoundZoneId { get; set; }
        [Required]
        public int VolumeCount { get; set; }
    }
}
