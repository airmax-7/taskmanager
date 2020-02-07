using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagerAPI.Models.DTO
{
    public class BoardDTO
    {
        public BoardDTO()
        {
            this.Tasks = new List<TaskDTO>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<TaskDTO> Tasks { get; set; }
    }

    public static class BoardExtension
    {
        public static BoardDTO ConvertToBoardDTO(this Board board)
        {
            BoardDTO dto = new BoardDTO()
            {
                Id = board.Id,
                Description = board.Description,
                Name = board.Name
            };
            foreach (var item in board.Tasks)
            {
                dto.Tasks.Add(item.ConvertToTaskDTO());
            }
            return dto;
        }

        public static List<BoardDTO> ConvertToBoardDto(this IEnumerable<Board> boards)
        {
            var list = new List<BoardDTO>();
            foreach (var item in boards)
            {
                list.Add(item.ConvertToBoardDTO());
            }
            return list;
        }
    }
}