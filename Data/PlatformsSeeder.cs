using assessment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using assessment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace assessment.Data
{
    public static class PlatformSeeder
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task Seed(ApplicationDbContext context)
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Failed to get token.");
                return;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Failed to fetch platform data.");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var platforms = JsonConvert.DeserializeObject<List<PlatformDto>>(json);

            foreach (var platform in platforms)
            {
                var existing = await context.Platforms
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

                    context.Platforms.Add(newPlatform);
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

                await context.SaveChangesAsync();
            }
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
    }
}