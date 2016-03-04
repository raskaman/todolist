using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoList.Models
{
    public class Task
    {
        [Key]
        public int taskId { get; set; }

        [Required]
        [MaxLength(140)]
        [MinLength(10)]
        public string title { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(20)]
        public string description { get; set; }

        [Required]
        public Status status { get; set; }


        [Required]
        public int user { get; set; }

        public bool completed { get; set; }
        public DateTime lastUpdated { get; set; }


        public int created_by { get; set; }

        public int assing_to { get; set; }

    }

    public enum Status
    {
        ToDo = 1,
        InProgress = 2,
        Completed = 3
    }
}