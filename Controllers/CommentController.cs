using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAPI.DTOs.Comment;
using SampleAPI.Extensions;
using SampleAPI.Interfaces;
using SampleAPI.Mappers;
using SampleAPI.Models;

namespace SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepo, IStockRepository StockRepo, UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly ICommentRepository _commentRepo = commentRepo;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IStockRepository _stockRepo = StockRepo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var comments = await _commentRepo.GetAllAsync();

            var commentDtos = comments.Select(x => x.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{StockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int StockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await _stockRepo.StockExists(StockId))
            {
                return BadRequest("Stock not found");
            }

            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToComment(StockId);
            commentModel.AppUserId = appuser.Id;
            commentModel.appUser = appuser;

            Comment comment = await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());

        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var comment = await _commentRepo.UpdateAsync(commentDto.ToComment(), Id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var comment = await _commentRepo.DeleteAsync(Id);
            if (comment == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}