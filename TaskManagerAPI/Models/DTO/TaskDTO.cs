using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagerAPI.Models.DTO
{
    public class TaskDTO
    {
        public TaskDTO()
        {
            this.Assignees = new List<UserDTO>();
        }
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public short Status { get; set; }
        public int BoardId { get; set; }

        public virtual ICollection<UserDTO> Assignees { get; set; }
    }

    public static class TaskExtensions
    {
        public static TaskDTO ConvertToTaskDTO(this Task task)
        {
            TaskDTO dto = new TaskDTO()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                BoardId = task.BoardId
            };

            foreach (var item in task.Assignees)
            {
                dto.Assignees.Add(item.ConvertToUserDTO());
            }
            return dto;
        }
    }
}