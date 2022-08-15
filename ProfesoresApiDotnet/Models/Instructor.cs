using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfesoresApiDotnet.Models
{
    [Table("Instructors")]
    public class Instructor
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(16)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(40)]
        public string? DateBirth { get; set; } //mes/día/año
        [Required]
        [MaxLength(30)]
        public string? TypeInstructor { get; set; }
        [Required]
        public string? TypeCurrency { get; set; }
        public float PriceHour { get; set; }

        //public ICollection<Lesson>? Lessons { get; set; }//un instructor tiene relacion con multiples lecciones impartidas
    }
}
