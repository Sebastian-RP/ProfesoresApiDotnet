using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfesoresApiDotnet.Models
{
    [Table("Lessons")]
    public class Lesson
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(16)]
        public DateTime LessonDate { get; set; }
        [Required]
        public int DurationLesson { get; set; } //minutos
        [Required]
        public Instructor? Instructor { get; set; }//vinculacion de ambas tablas y llave foranea InstructorId
        public int InstructorId { get; set; }
    }
}
