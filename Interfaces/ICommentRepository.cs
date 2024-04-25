using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleAPI.Models;

namespace SampleAPI.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int Id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment?> UpdateAsync(Comment comment, int Id);

        Task<Comment?> DeleteAsync(int Id);

    }
}