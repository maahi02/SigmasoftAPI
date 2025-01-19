using Microsoft.AspNetCore.Mvc;


namespace SigmasoftAPI.Controllers.Content
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        

        public CandidatesController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate()
        {
            return Ok();
        }

    }
}