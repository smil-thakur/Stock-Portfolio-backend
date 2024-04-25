using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 or more letter long")]
        [MaxLength(100, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "content must be 5 or more letter long")]
        [MaxLength(280, ErrorMessage = "content cannot be over 280 characters")]
        public string Content { get; set; } = string.Empty;

    }
}