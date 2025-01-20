using Application.Dtos;
using Domain.Models;
using Infrastructure.Content.Services;
using Microsoft.AspNetCore.Mvc;


namespace SigmasoftAPI.Controllers.Content
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {

        private readonly CandidateService _candidateService;
        public CandidatesController(CandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CandidateDto candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _candidateService.GetOrCreateAsync(candidate);
            return Ok(result);
        }

    }
}