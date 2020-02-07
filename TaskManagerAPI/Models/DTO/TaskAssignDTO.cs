using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagerAPI.Models.DTO
{
    public class TaskAssignDTO
    {
        public int TaskId { get; set; }
        public string AssigneeEmail { get; set; }
    }
}