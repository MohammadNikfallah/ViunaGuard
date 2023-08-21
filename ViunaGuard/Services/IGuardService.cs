using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public interface IGuardService
    {
        public Task<ServiceResponse<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId);
        public Task<ServiceResponse<Entrance>> PostEntrance(EntrancePostDto entrancePostDto);
        public Task<ServiceResponse<object>> PostSameGroupEntrances
            (EntranceGroupPostDto entranceGroupPost);
    }
}
