using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class School
    {
        public int Id { get; set; } // Primary Key, Auto Increment

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Principal { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // 1 School - Many Students
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
