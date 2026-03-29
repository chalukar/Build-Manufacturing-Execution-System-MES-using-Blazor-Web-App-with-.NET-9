using MES.Application.DTOs;
using System.Net.Http.Json;

namespace MES.Web.Services
{
    public class WorkCentreService(HttpClient http)
    {
        public async Task<(List<WorkCentreDto> Data, string? Error)> GetAllAsync()
        {
            try
            {
                var response = await http.GetAsync("api/workcentres");

                // Check status before trying to deserialize
                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();

                    return ([], $"API returned {(int)response.StatusCode} " +
                                $"{response.StatusCode}. " +
                                $"Check [Authorize] on WorkCentresController.");
                }

                var contentType = response.Content.Headers.ContentType?.MediaType;

                // Guard against HTML error pages
                if (contentType is null || !contentType.Contains("application/json"))
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return ([], $"API returned non-JSON response ({contentType}). " +
                                $"Body starts with: {body[..Math.Min(100, body.Length)]}");
                }

                var data = await response.Content
                    .ReadFromJsonAsync<List<WorkCentreDto>>();

                return (data ?? [], null);
            }
            catch (HttpRequestException ex)
            {
                return ([], $"Cannot reach API: {ex.Message}. " +
                            $"Is MES.API running on the correct port?");
            }
            catch (Exception ex)
            {
                return ([], $"Unexpected error: {ex.Message}");
            }
        }
    }
}
