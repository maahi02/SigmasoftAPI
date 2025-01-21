using Application.Common.Models;
using Application.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Content
{
    public interface ICandidateService
    {
        Task<ServiceResult<IEnumerable<CandidateGetModelDto>>> GetAllAsync();
        Task<ServiceResult<Candidate>> CreateOrUpdateAsync(CandidateDto candidateDto);
    }
}
