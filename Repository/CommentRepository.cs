using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Data;
using SampleAPI.Interfaces;
using SampleAPI.Models;

namespace SampleAPI.Repository
{
    public class CommentRepository(ApplicationDBContext context) : ICommentRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int Id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == Id);
            if (comment == null)
            {
                return null;
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a => a.appUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int Id)
        {
            var comment = await _context.Comments.Include(a => a.appUser).FirstOrDefaultAsync(c => c.Id == Id);

            if (comment == null)
            {
                return null;
            }
            return comment;
        }

        public async Task<Comment?> UpdateAsync(Comment comment, int Id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);
            if (commentModel == null)
            {
                return null;
            }
            commentModel.Title = comment.Title;
            commentModel.Content = comment.Content;
            await _context.SaveChangesAsync();
            return commentModel;


        }
    }
}