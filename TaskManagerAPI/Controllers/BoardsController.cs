using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTO;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    //[RoutePrefix("/api/board")]
    public class BoardsController : BaseController
    {
        // GET api/boards
        [HttpGet]
        public IHttpActionResult GetBoards()
        {
            var boards = context.Boards.ToList();
            return Ok(boards.ConvertToBoardDto());
        }

        // GET api/boards/2
        [HttpGet]
        public async Task<IHttpActionResult> GetBoardById(int id)
        {
            Board boards = await context.Boards.FindAsync(id);
            return Ok(boards.ConvertToBoardDTO());
        }

        // POST api/boards
        [HttpPost]
        public async Task<IHttpActionResult> CreateBoard(Board newBoard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = context.Boards.Add(newBoard);
                await context.SaveChangesAsync();
                return Ok(newBoard.ConvertToBoardDTO());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/boards/2
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBoard(int id)
        {
            try
            {
                Board board = await context.Boards.FindAsync(id);
                if (board == null)
                {
                    return NotFound();
                }
                context.Boards.Remove(board);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

    }
}
