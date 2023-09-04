using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("postCar")]
        public async Task<ActionResult<Person>> PostCar(Car car, int personId)
        {
            var response = await _userService.PostCar(car, personId);
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
            var response = await _userService.PostPerson(personDto);
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
            var response = await _userService.GetOrganizationSignatureNeed(organizationId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetPersonDetails")]
        public async Task<ActionResult<PersonGetDto>> GetPersonDetails()
        {
            var response = await _userService.GetPersonDetails();
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
            var response = await _userService.GetEntrancePermissions();
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpGet("GetPersonJobs")]
        public async Task<ActionResult<List<EmployeeGetDto>>> GetPersonJobs()
        {
            var response = await _userService.GetPersonJobs();
            if (response.HttpResponseCode == 200)
                    return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("PostEntrancePermission")]
        public async Task<ActionResult> PostEntrancePermission(EntrancePermissionPostDto entrancePermissionPostDto)
        {
            var response = await _userService.PostEntrancePermission(entrancePermissionPostDto);
            if (response.HttpResponseCode == 200)
                    return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

    }
}
