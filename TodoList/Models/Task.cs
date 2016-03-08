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
        public string title { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public Status status { get; set; }

        public bool deleted { get; set; }

        public int createdBy { get; set; }

        public int assignedTo { get; set; }

        public DateTime lastUpdated { get; set; }

        public int created_by { get; set; }

        public int assing_to { get; set; }

    }
}