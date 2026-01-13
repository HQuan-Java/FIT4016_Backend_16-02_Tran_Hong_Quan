using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; } // Primary Key, Auto Increment

        [Required]
        public int SchoolId { get; set; } // Foreign Key

        [ForeignKey("SchoolId")]
        public School School { get; set; }

        [Required]
        public string FullName { get; set; } // Not Null

        [Required]
        public string StudentId { get; set; } // Not Null, Unique

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Not Null, Unique

        public string? Phone { get; set; } // Nullable

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
