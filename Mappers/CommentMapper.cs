using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleAPI.DTOs.Comment;
using SampleAPI.Models;

namespace SampleAPI.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
                Content = comment.Content,
                CreatedBy = comment.appUser.UserName

            };
        }

        public static Comment ToComment(this CreateCommentDto createCommentDto, int StockId)
        {
            return new Comment
            {
                Title = createCommentDto.Title,
                StockId = StockId,
                Content = createCommentDto.Content,



            };
        }

        public static Comment ToComment(this UpdateCommentDto updateCommentDto)
        {
            return new Comment
            {
                Title = updateCommentDto.Title,
                Content = updateCommentDto.Content,
            };
        }
    }
}