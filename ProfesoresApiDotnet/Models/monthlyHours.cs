using System.ComponentModel.DataAnnotations;

namespace ProfesoresApiDotnet.Models
{
    public class monthlyHours
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(16)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(40)]
        public string? DateBirth { get; set; } //mes/día/año
        [Required]
        public string? TypeInstructor { get; set; }
        [Required]
        public string? TypeCurrency { get; set; }
        public float PriceHour { get; set; }
        public float DurationLesson { get; set; }   
    }
}
