using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;

namespace ViunaGuard.Services;

public interface IEmployeeService
{
    public Task<ServiceResponse> PostPeriodicShift(PeriodicShiftPostDto shift);
    public Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift);
    public Task<ServiceResponse<TwoShiftGetDto>> GetEmployeeShifts();
    public Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift();
    public Task<ServiceResponse> PostEmployee(EmployeePostDto employeePostDto);
    public Task<ServiceResponse> PostEmployeeWeeklyShift(WeeklyShiftPostDto weeklyShiftPostDto);
    public Task<ServiceResponse> PostEmployeeMonthlyShift(MonthlyShiftPostDto monthlyShiftPostDto);


}