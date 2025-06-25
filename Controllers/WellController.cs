using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assessment.Data;
using assessment.Models;
using Microsoft.AspNetCore.Authorization;

namespace assessment.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/wells")]
    public class WellController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WellController> _logger;

        public WellController(ApplicationDbContext context, ILogger<WellController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WellDto>>> GetAll()
        {
            var wells = await _context.Wells.ToListAsync();
            return Ok(wells);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WellDto>> GetById(int id)
        {
            var well = await _context.Wells.FindAsync(id);
            if (well == null)
                return NotFound();

            return Ok(well);
        }
    }
}