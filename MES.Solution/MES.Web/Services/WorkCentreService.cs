using MES.Application.DTOs;
using System.Net.Http.Json;

namespace MES.Web.Services
{
    public class WorkCentreService(HttpClient http)
    {
        public async Task<(List<WorkCentreDto> Data, string? Error)> GetAllAsync(bool activeOnly = true)
        {
            try
            {
                var response = await http.GetAsync($"api/workcentres?activeOnly={activeOnly}");

                // Check status before trying to deserialize
                if (!response.IsSuccessStatusCode)
                {
                    //var body = await response.Content.ReadAsStringAsync();

                    return ([], $"API returned {(int)response.StatusCode} " +
                                $"{response.StatusCode}. ");
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

        public async Task<(Guid? Id, string? Error)> CreateAsync(
        CreateWorkCentreDto dto)
        {
            try
            {
                var response = await http
                    .PostAsJsonAsync("api/workcentres", dto);

                if (response.IsSuccessStatusCode)
                {
                    var id = await response.Content.ReadFromJsonAsync<Guid>();
                    return (id, null);
                }

                var error = await response.Content.ReadAsStringAsync();
                return (null, $"Failed to create: {error}");
            }
            catch (Exception ex)
            {
                return (null, $"Unexpected error: {ex.Message}");
            }
        }
    }
}
