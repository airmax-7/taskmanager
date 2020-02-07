using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagerAPI.Models.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public short Status { get; set; }
        public int BoardId { get; set; }

        public virtual ICollection<ApplicationUser> Assignees { get; set; }
    }
}