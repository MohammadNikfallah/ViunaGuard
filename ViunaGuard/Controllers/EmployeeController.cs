using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("PostPeriodicShift")]
        public async Task<ActionResult> PostPeriodicShift(PeriodicShiftPostDto shift, int employeeId)
        {
            var response = await _employeeService.PostPeriodicShift(shift, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetPermissionsToSign")]
        public async Task<ActionResult> GetPermissionsToSign(int employeeId)
        {
            var response = await _employeeService.GetPermissionsToSign(employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetAllPermissions")]
        public async Task<ActionResult<List<EntrancePermissionGetDto>>> GetPermissions(int employeeId)
        {
            var response = await _employeeService.GetPermissions(employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpPost("PostShift")]
        public async Task<ActionResult> PostPeriodicShift(ShiftPostDto shift, int employeeId)
        {
            var response = await _employeeService.PostShift(shift, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetCurrentShift")]
        public async Task<ActionResult<EmployeeShift>> GetCurrentShift(int employeeId)
        {
            var response = await _employeeService.GetCurrentShift(employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetEmployeeShifts")]
        public async Task<ActionResult<TwoShiftGetDto>> GetEmployeeShifts(int employeeId)
        {
            var response = await _employeeService.GetEmployeeShifts(employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("PostEmployeeWeeklyShift")]
        public async Task<ActionResult> PostEmployeeWeeklyShift(WeeklyShiftPostDto weeklyShiftPostDto, int employeeId)
        {
            var response = await _employeeService.PostEmployeeWeeklyShift(weeklyShiftPostDto, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpPost("PostEmployeeMonthlyShift")]
        public async Task<ActionResult> PostEmployeeMonthlyShift(MonthlyShiftPostDto monthlyShiftPostDto, int employeeId)
        {
            var response = await _employeeService.PostEmployeeMonthlyShift(monthlyShiftPostDto, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpPost("SignEntrancePermission")]
        public async Task<ActionResult> SignEntrancePermission(int entrancePermissionId, int employeeId)
        {
            var response = await _employeeService.SignEntrancePermission(entrancePermissionId, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
    }
}
