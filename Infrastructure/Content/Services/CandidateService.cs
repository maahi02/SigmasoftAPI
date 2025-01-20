using Application.Common.Models;
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

        public async Task<ServiceResult<Candidate>> CreateOrUpdateAsync(CandidateDto candidateDto)
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
                return ServiceResult<Candidate>.SuccessResult("Candidate created successfully", cachedCandidate);
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
                return ServiceResult<Candidate>.SuccessResult("Candidate created successfully", cachedCandidate);
            }
            // _cache.Set(cacheKey, cachedCandidate, TimeSpan.FromMinutes(2));
            //  }  // removed this line for development testing..
        }

        public async Task<ServiceResult<IEnumerable<CandidateGetModelDto>>> GetAllAsync()
        {
            string cacheKey = "AllCandidates";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Candidate> cachedCandidates))
            {
                // If not found in cache, fetch from the repository
                cachedCandidates = await _repository.GetAllAsync();

                // If no candidates found, return an empty list
                if (cachedCandidates == null || !cachedCandidates.Any())
                {
                    cachedCandidates = new List<Candidate>();
                }

                // Store the list in cache for 2 minutes
                _cache.Set(cacheKey, cachedCandidates, TimeSpan.FromMinutes(2));
            }

            if (cachedCandidates != null && cachedCandidates.Any())
            {

                // for improvement we can user automapper
                var result = cachedCandidates.Select(c => new CandidateGetModelDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    FreeTextComment = c.FreeTextComment,
                    GitHubProfile = c.GitHubProfile,
                    LinkedInProfile = c.LinkedInProfile,
                    PreferredCallTime = c.PreferredCallTime,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                });
                return ServiceResult<IEnumerable<CandidateGetModelDto>>.SuccessResult("Candidates retrieved successfully.", result);
            }
            else
            {
                return ServiceResult<IEnumerable<CandidateGetModelDto>>.SuccessResult("Candidates Not Found!", new List<CandidateGetModelDto>());
            }

        }


    }

}
