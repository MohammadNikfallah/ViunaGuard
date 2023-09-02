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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _guardService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _guardService = employeeService;
        }

        [HttpPost("PostPeriodicShift")]
        public async Task<ActionResult> PostPeriodicShift(PeriodicShiftPostDto shift, int employeeId)
        {
            var response = await _guardService.PostPeriodicShift(shift, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpPost("PostShift")]
        public async Task<ActionResult> PostPeriodicShift(ShiftPostDto shift, int employeeId)
        {
            var response = await _guardService.PostShift(shift, employeeId);
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
            var response = await _guardService.GetCurrentShift(employeeId);
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
            var response = await _guardService.GetEmployeeShifts(employeeId);
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
            var response = await _guardService.PostEmployeeWeeklyShift(weeklyShiftPostDto, employeeId);
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
            var response = await _guardService.PostEmployeeMonthlyShift(monthlyShiftPostDto, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
    }
}
