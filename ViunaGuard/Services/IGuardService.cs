namespace ViunaGuard.Services
{
    public interface IGuardService
    {
        public Task<ServiceResponse> PostEntrances
            (EntranceGroupPostDto entranceGroupPost, int employeeId);

        public Task<ServiceResponse<List<EntranceGroupGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate,
            int doorId, int guardId, int enterOrExitId, int employeeId, int entranceCount);
        public Task<ServiceResponse<EntrancePermissionCheckDto>> CheckEntrancePermission(string nationalId, int emplyeeId);
        public Task<ServiceResponse<CheckExitPermissionDto>> CheckExitPermission(string nationalId, int employeeId);
        public Task<ServiceResponse> PostPerson(PersonPostDto personPostDto, int employeeId);
    }
}
