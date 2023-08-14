using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViunaGuard.Dtos;
using ViunaGuard.Models;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserController(IUserService userService, DataContext context, IMapper mapper)
        {
            this.userService = userService;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("postCar")]
        public async Task<ActionResult<Person>> PostCar(Car car, int PersonId)
        {
            var response = await userService.PostCar(car, PersonId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("postPerson")]
        public async Task<ActionResult<List<Person>>> PostPerson(PersonPostDto personDto)
        {
            var response = await userService.PostPerson(personDto);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("PostShiftMonthly")]
        public async Task<ActionResult<List<EmployeeShift>>> PostShiftMonthly(EmployeeShiftPeriodicMonthly shift)
        {
            var response = await userService.PostShiftMonthly(shift);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetOrganizationSignatureNeed")]
        public async Task<ActionResult<List<SignatureNeedForEntrancePermission>>> GetOrganizationSignatureNeed
            (int organizationId)
        {
            var response = await userService.GetOrganizationSignatureNeed(organizationId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetOrganizationAuthorities")]
        public async Task<ActionResult<List<Authority>>> GetOrganizationAuthorities(int organizationId)
        {
            var response = await userService.GetOrganizationAuthorities(organizationId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetPersonDetails")]
        public async Task<ActionResult<Person>> GetPersonDetails()
        {
            var response = await userService.GetPersonDetails();
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetCurrentShift")]
        public async Task<ActionResult<EmployeeShift>> GetCurrentShift(int EmployeeId)
        {
            var response = await userService.GetCurrentShift(EmployeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetEntrancePermissions")]
        public async Task<ActionResult<List<EntrancePermissionGetDto>>> GetEntrancePermissions()
        {
            var response = await userService.GetEntrancePermissions();
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

    }
}
