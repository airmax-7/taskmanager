using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagerAPI.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public static class UserExtension
    {
        public static UserDTO ConvertToUserDTO(this ApplicationUser user)
        {
            UserDTO dto = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email
            };
            return dto;
        }
    }
}