using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public interface IGuardService
    {
        // public Task<ServiceResponse<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
        //     , int personId, int carId, int guardId, int enterOrExitId);
        // public Task<ServiceResponse<Entrance>> PostEntrance(EntrancePostDto entrancePostDto);
        public Task<ServiceResponse<object>> PostEntrances
            (EntranceGroupPostDto entranceGroupPost);

        public Task<ServiceResponse<List<EntranceGroupGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate,
            int doorId
            , int guardId, int enterOrExitId);
    }
}
