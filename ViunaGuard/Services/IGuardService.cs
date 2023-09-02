using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public interface IGuardService
    {
        public Task<ServiceResponse<object>> PostEntrances
            (EntranceGroupPostDto entranceGroupPost, int employeeId);

        public Task<ServiceResponse<List<EntranceGroupGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate,
            int doorId, int guardId, int enterOrExitId, int employeeId);
    }
}
