using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;

namespace ViunaGuard.Services;

public interface IEmployeeService
{
    public Task<ActionResult> PostEmployee(EmployeePostDto employeePostDto);
}