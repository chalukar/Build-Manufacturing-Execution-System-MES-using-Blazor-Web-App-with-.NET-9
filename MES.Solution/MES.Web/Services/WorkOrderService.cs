using MES.Application.DTOs;
using MES.Domain.Enums;
using System.Net.Http.Json;

namespace MES.Web.Services
{
    public class WorkOrderService(HttpClient http)
    {
        public async Task<List<WorkOrderDto>?> GetAllAsync(
            WorkOrderStatus? status = null)
        {
            try
            {
                var url = status.HasValue
                    ? $"api/workorders?status={status}"
                    : "api/workorders";

                return await http.GetFromJsonAsync<List<WorkOrderDto>>(url);
            }
            catch (Exception)
            {
                // Return empty list instead of crashing the page
                return [];
            }
        }

        public async Task<WorkOrderDto?> GetByIdAsync(Guid id)
        {
            try
            {
                return await http
                    .GetFromJsonAsync<WorkOrderDto>($"api/workorders/{id}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Guid?> CreateAsync(CreateWorkOrderDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/workorders", dto);
                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<Guid>()
                    : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<HttpResponseMessage> ReleaseAsync(Guid id)
            => http.PostAsync($"api/workorders/{id}/release", null);

        public Task<HttpResponseMessage> StartAsync(Guid id)
            => http.PostAsync($"api/workorders/{id}/start", null);

        public Task<HttpResponseMessage> CompleteAsync(Guid id)
            => http.PostAsync($"api/workorders/{id}/complete", null);

        public Task<HttpResponseMessage> RecordProductionAsync(RecordProductionDto dto)
            => http.PostAsJsonAsync("api/workorders/production", dto);
    }
}
