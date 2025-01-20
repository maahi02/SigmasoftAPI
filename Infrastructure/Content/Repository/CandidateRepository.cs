using Domain.Models;
using Infrastructure.Content.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Content.Repository
{
    public class CandidateRepository : IRepository<Candidate>
    {
        private readonly AppDbContext _context;

        public CandidateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Candidate>> GetAllAsync() => await _context.Candidates.ToListAsync();

        public async Task<Candidate> GetByIdAsync(Guid id)
        {
            var result = await _context.Candidates.FindAsync(id);
            return result;
        }

        public async Task<Candidate> GetByEmailAsync(string email)
        {
            return await _context.Candidates.SingleOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddAsync(Candidate entity) => await _context.Candidates.AddAsync(entity);

        public async Task UpdateAsync(Candidate entity) => _context.Candidates.Update(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
