using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        public bool active { get; set; }
    }
}