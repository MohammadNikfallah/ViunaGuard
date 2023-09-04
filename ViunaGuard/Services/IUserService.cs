namespace ViunaGuard.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<Person>> PostCar(Car car, int id);
        public Task<ServiceResponse<List<Person>>> PostPerson(PersonPostDto personDto);
        public Task<ServiceResponse<object>> PostPeriodicShift(PeriodicShiftPostDto shift);
        public Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift);
        public Task<ServiceResponse<TwoShiftGetDto>> GetPersonShifts(int employeeId);
        public Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed (int organizationId);
        public Task<ServiceResponse> PostEntrancePermission(EntrancePermissionPostDto entrancePermissionPostDto);
        public Task<ServiceResponse<PersonGetDto>> GetPersonDetails();
        public Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift(int employeeId);
        public Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetEntrancePermissions();
        public Task<ServiceResponse<List<EmployeeGetDto>>> GetPersonJobs();
    }
}
