using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;

namespace ViunaGuard.Services;

public interface IEmployeeService
{
    public Task<ServiceResponse<object>> PostPeriodicShift(PeriodicShiftPostDto shift);
    public Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift);
    public Task<ServiceResponse<TwoShiftGetDto>> GetEmployeeShifts();
    public Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift();
    public Task<ActionResult> PostEmployee(EmployeePostDto employeePostDto);
}