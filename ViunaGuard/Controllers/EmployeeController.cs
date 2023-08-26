using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // [HttpPost]
        // public async Task<ActionResult> PostEmployee(EmployeePostDto employeePostDto)
        // {
        //     
        // }
        
    }
}
