using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assessment.Data;
using assessment.Models;
using Microsoft.AspNetCore.Authorization;
using assessment.Responses;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace assessment.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/platforms")]
    public class PlatformController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlatformController> _logger;
        private readonly TokenStoreService _tokenStore;
        private static readonly HttpClient client = new HttpClient();

        public PlatformController(ApplicationDbContext context, ILogger<PlatformController> logger, TokenStoreService tokenStore)
        {
            _context = context;
            _logger = logger;
            _tokenStore = tokenStore;
        }

        private static async Task<string> GetTokenAsync()
        {
            var loginData = new
            {
                Username = "user@aemenersol.com",
                Password = "Test@123"
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://test-demo.aemenersol.com/api/Account/Login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();
            dynamic tokenObj = JsonConvert.DeserializeObject(result);
            return (string)tokenObj;
        }

        [HttpGet("actual")]
        public async Task<ActionResult<IEnumerable<PlatformDto>>> UpdateAllActual()
        {
            var token = await GetTokenAsync();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual");

            var json = await response.Content.ReadAsStringAsync();
            var platforms = JsonConvert.DeserializeObject<List<PlatformDto>>(json);

            foreach (var platform in platforms)
            {
                var existing = await _context.Platforms
                    .Include(p => p.Wells)
                    .FirstOrDefaultAsync(p => p.PlatformId == platform.Id);

                if (existing == null)
                {
                    var newPlatform = new PlatformDto
                    {
                        PlatformId = platform.Id,
                        UniqueName = platform.UniqueName,
                        Latitude = platform.Latitude,
                        Longitude = platform.Longitude,
                        CreatedAt = platform.CreatedAt,
                        UpdatedAt = platform.UpdatedAt,
                        Wells = new List<WellDto>()
                    };

                    foreach (var well in platform.Wells)
                    {
                        var newWell = new WellDto
                        {
                            WellId = well.Id,
                            UniqueName = well.UniqueName,
                            Latitude = well.Latitude,
                            Longitude = well.Longitude,
                            CreatedAt = well.CreatedAt,
                            UpdatedAt = well.UpdatedAt,
                            PlatformId = platform.Id
                        };

                        newPlatform.Wells.Add(newWell);
                    }

                    _context.Platforms.Add(newPlatform);
                }
                else
                {
                    existing.UniqueName = platform.UniqueName ?? existing.UniqueName;
                    existing.Latitude = platform.Latitude ?? existing.Latitude;
                    existing.Longitude = platform.Longitude ?? existing.Longitude;
                    existing.CreatedAt = platform.CreatedAt ?? existing.CreatedAt;
                    existing.UpdatedAt = platform.UpdatedAt ?? existing.UpdatedAt;

                    foreach (var incomingWell in platform.Wells)
                    {
                        var existingWell = existing.Wells.FirstOrDefault(w => w.WellId == incomingWell.Id);

                        if (existingWell == null)
                        {
                            existing.Wells.Add(new WellDto
                            {
                                WellId = incomingWell.Id,
                                UniqueName = incomingWell.UniqueName,
                                Latitude = incomingWell.Latitude,
                                Longitude = incomingWell.Longitude,
                                CreatedAt = incomingWell.CreatedAt,
                                UpdatedAt = incomingWell.UpdatedAt,
                                PlatformId = existing.PlatformId
                            });
                        }
                        else
                        {
                            existingWell.UniqueName = incomingWell.UniqueName ?? existingWell.UniqueName;
                            existingWell.Latitude = incomingWell.Latitude ?? existingWell.Latitude;
                            existingWell.Longitude = incomingWell.Longitude ?? existingWell.Longitude;
                            existingWell.CreatedAt = incomingWell.CreatedAt ?? existingWell.CreatedAt;
                            existingWell.UpdatedAt = incomingWell.UpdatedAt ?? existingWell.UpdatedAt;
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new { Success = true });
        }

        [HttpGet("dummy")]
        public async Task<ActionResult<IEnumerable<PlatformDto>>> UpdateAllDummy()
        {
            var token = await GetTokenAsync();
         
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy");

            var json = await response.Content.ReadAsStringAsync();
            var platforms = JsonConvert.DeserializeObject<List<PlatformDto>>(json);

            foreach (var platform in platforms)
            {
                var existing = await _context.Platforms
                    .Include(p => p.Wells)
                    .FirstOrDefaultAsync(p => p.PlatformId == platform.Id);

                if (existing == null)
                {
                    var newPlatform = new PlatformDto
                    {
                        PlatformId = platform.Id,
                        UniqueName = platform.UniqueName,
                        Latitude = platform.Latitude,
                        Longitude = platform.Longitude,
                        CreatedAt = platform.CreatedAt,
                        UpdatedAt = platform.UpdatedAt,
                        Wells = new List<WellDto>()
                    };

                    foreach (var well in platform.Wells)
                    {
                        var newWell = new WellDto
                        {
                            WellId = well.Id,
                            UniqueName = well.UniqueName,
                            Latitude = well.Latitude,
                            Longitude = well.Longitude,
                            CreatedAt = well.CreatedAt,
                            UpdatedAt = well.UpdatedAt,
                            PlatformId = platform.Id
                        };

                        newPlatform.Wells.Add(newWell);
                    }

                    _context.Platforms.Add(newPlatform);
                }
                else
                {
                    existing.UniqueName = platform.UniqueName ?? existing.UniqueName;
                    existing.Latitude = platform.Latitude ?? existing.Latitude;
                    existing.Longitude = platform.Longitude ?? existing.Longitude;
                    existing.CreatedAt = platform.CreatedAt ?? existing.CreatedAt;
                    existing.UpdatedAt = platform.UpdatedAt ?? existing.UpdatedAt;

                    foreach (var incomingWell in platform.Wells)
                    {
                        var existingWell = existing.Wells.FirstOrDefault(w => w.WellId == incomingWell.Id);

                        if (existingWell == null)
                        {
                            existing.Wells.Add(new WellDto
                            {
                                WellId = incomingWell.Id,
                                UniqueName = incomingWell.UniqueName,
                                Latitude = incomingWell.Latitude,
                                Longitude = incomingWell.Longitude,
                                CreatedAt = incomingWell.CreatedAt,
                                UpdatedAt = incomingWell.UpdatedAt,
                                PlatformId = existing.PlatformId
                            });
                        }
                        else
                        {
                            existingWell.UniqueName = incomingWell.UniqueName ?? existingWell.UniqueName;
                            existingWell.Latitude = incomingWell.Latitude ?? existingWell.Latitude;
                            existingWell.Longitude = incomingWell.Longitude ?? existingWell.Longitude;
                            existingWell.CreatedAt = incomingWell.CreatedAt ?? existingWell.CreatedAt;
                            existingWell.UpdatedAt = incomingWell.UpdatedAt ?? existingWell.UpdatedAt;
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new { Success = true });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformDto>>> GetAll()
        {
            var platforms = await _context.Platforms
                .Include(p => p.Wells)
                .ToListAsync();

            var result = platforms.Select(p => new PlatformApiResponse
            {
                Id = p.PlatformId,
                UniqueName = p.UniqueName,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Wells = p.Wells.Select(w => new WellApiResponse
                {
                    Id = w.WellId,
                    PlatformId = w.PlatformId,
                    UniqueName = w.UniqueName,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformDto>> GetById(int id)
        {
            var platform = await _context.Platforms
                .Include(p => p.Wells)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (platform == null)
                return NotFound();

            return Ok(platform);
        }
    }
}