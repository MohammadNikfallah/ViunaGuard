using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "Employee,Guard", Policy = "RoleCookie")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _guardService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _guardService = employeeService;
        }

        [HttpPost("PostPeriodicShift")]
        public async Task<ActionResult> PostPeriodicShift(PeriodicShiftPostDto shift)
        {
            var response = await _guardService.PostPeriodicShift(shift);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpPost("PostShift")]
        public async Task<ActionResult> PostPeriodicShift(ShiftPostDto shift)
        {
            var response = await _guardService.PostShift(shift);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetCurrentShift")]
        public async Task<ActionResult<EmployeeShift>> GetCurrentShift()
        {
            var response = await _guardService.GetCurrentShift();
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetEmployeeShifts")]
        public async Task<ActionResult<TwoShiftGetDto>> GetEmployeeShifts()
        {
            var response = await _guardService.GetEmployeeShifts();
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("PostEmployeeWeeklyShift")]
        public async Task<ActionResult> PostEmployeeWeeklyShift(WeeklyShiftPostDto weeklyShiftPostDto)
        {
            var response = await _guardService.PostEmployeeWeeklyShift(weeklyShiftPostDto);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
    }
}
