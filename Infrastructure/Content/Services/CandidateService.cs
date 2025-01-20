using Application.Dtos;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Content.Services
{
    public class CandidateService
    {
        private readonly IRepository<Candidate> _repository;
        private readonly IMemoryCache _cache;

        public CandidateService(IRepository<Candidate> repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Candidate> GetOrCreateAsync(CandidateDto candidateDto)
        {

            var candidate = new Candidate
            {
                CreatedAt = new DateTime(),
                Email = candidateDto.Email,
                FirstName = candidateDto.FirstName,
                LastName = candidateDto.LastName,
                FreeTextComment = candidateDto.FreeTextComment,
                GitHubProfile = candidateDto.GitHubProfile,
                LinkedInProfile = candidateDto.LinkedInProfile,
                PhoneNumber = candidateDto.PhoneNumber,
                PreferredCallTime = candidateDto.PreferredCallTime,
                UpdatedAt = null
            };


            string cacheKey = $"Candidate_{candidate.Email}";
            // if (!_cache.TryGetValue(cacheKey, out Candidate cachedCandidate))
            //  {
            var cachedCandidate = await _repository.GetByEmailAsync(candidate.Email);
            if (cachedCandidate == null)
            {
                await _repository.AddAsync(candidate);
                await _repository.SaveChangesAsync();
                cachedCandidate = candidate;
            }
            else
            {
                cachedCandidate.FirstName = candidate.FirstName;
                cachedCandidate.LastName = candidate.LastName;
                cachedCandidate.PhoneNumber = candidate.PhoneNumber;
                cachedCandidate.PreferredCallTime = candidate.PreferredCallTime;
                cachedCandidate.LinkedInProfile = candidate.LinkedInProfile;
                cachedCandidate.GitHubProfile = candidate.GitHubProfile;
                cachedCandidate.FreeTextComment = candidate.FreeTextComment;
                cachedCandidate.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(cachedCandidate);
                await _repository.SaveChangesAsync();
            }
            _cache.Set(cacheKey, cachedCandidate, TimeSpan.FromMinutes(2));
            //  }  // removed this line for development testing..
            return cachedCandidate;
        }
    }

}
