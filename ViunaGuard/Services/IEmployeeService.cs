namespace ViunaGuard.Services;

public interface IEmployeeService
{
    public Task<ServiceResponse> PostPeriodicShift(PeriodicShiftPostDto shift, int employeeId);
    public Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift, int employeeId);
    public Task<ServiceResponse<TwoShiftGetDto>> GetEmployeeShifts(int employeeId);
    public Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift(int employeeId);
    public Task<ServiceResponse> PostEmployee(EmployeePostDto employeePostDto, int employeeId);
    public Task<ServiceResponse> PostEmployeeWeeklyShift(WeeklyShiftPostDto weeklyShiftPostDto, int employeeId);
    public Task<ServiceResponse> PostEmployeeMonthlyShift(MonthlyShiftPostDto monthlyShiftPostDto, int employeeId);


}