using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MES.API.Hubs
{
    [Authorize]
    public class ProductionHub : Hub
    {
        public async Task JoinWorkCentreGroup(string workCentreCode)
        => await Groups.AddToGroupAsync(Context.ConnectionId, workCentreCode);

        public async Task LeaveWorkCentreGroup(string workCentreCode)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, workCentreCode);
    }
}
