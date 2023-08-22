using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<Person>> PostCar(Car car, int id);
        public Task<ServiceResponse<List<Person>>> PostPerson(PersonPostDto personDto);
        public Task<ServiceResponse<List<EmployeeShiftPeriodicMonthly>>> PostShiftMonthly(MonthlyShiftPostDto shift);
        public Task<ServiceResponse<List<EmployeeShiftPeriodicWeekly>>> PostShiftWeekly(WeeklyShiftPostDto shift);
        public Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift);
        public Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed (int organizationId);
        public Task<ServiceResponse<List<Authority>>> GetOrganizationAuthorities(int organizationId);
        public Task<ServiceResponse<PersonGetDto>> GetPersonDetails();
        public Task<ServiceResponse<EmployeeShift>> GetCurrentShift(int EmployeeId);
        public Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetEntrancePermissions();
        public Task<ServiceResponse<List<EmployeeGetDto>>> GetPersonJobs();
    }
}
