using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoList.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [MinLength(5)]
        public string userName { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        public bool active { get; set; }
    }
}