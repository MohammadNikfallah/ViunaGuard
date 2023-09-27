using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ViunaGuard.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<Person>> PostCar(Car car, string id)
        {
            var response = new ServiceResponse<Person>();

            Person? p = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (p == null)
            {
                response.Message = "Person Not Found";
                response.HttpResponseCode = 404;
                return response;
            }
            p.Cars.Add(car);
            var carInList = await _context.Cars.FirstOrDefaultAsync(c => c.Id == car.Id);
            if (carInList != null)  
                carInList.People.Add(p);
            else
            {
                car.People.Add(p);
                _context.Cars.Add(car);
            }
            await _context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            response.Data = p;
            return response;
        }

        public async Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed
            (int organizationId)
        {
            var response = new ServiceResponse<List<SignatureNeedGetDto>>();

            var data = await _context.SignatureNeedForEntrancePermissions
                .Where(s => s.OrganizationId == organizationId)
                .Select(s => _mapper.Map<SignatureNeedGetDto>(s))
                .ToListAsync();

            if (data.Count == 0)
            {
                response.HttpResponseCode = 404;
                response.Message = "no SignatureNeed found related to this Organization";
                return response;
            }

            response.Data = data;
            response.HttpResponseCode=200;
            return response;
        }

        public async Task<ServiceResponse> PostEntrancePermission(EntrancePermissionPostDto entrancePermissionPostDto)
        {
            var response = new ServiceResponse<PersonGetDto>();

            var entrancePermission = _mapper.Map<EntrancePermission>(entrancePermissionPostDto);
            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            entrancePermission.PersonId = userId!;
            await _context.EntrancePermissions.AddAsync(entrancePermission);
            await _context.SaveChangesAsync();

            response.HttpResponseCode = 200;
            return response;
        }
        
        public async Task<ServiceResponse<PersonGetDto>> GetPersonDetails()
        {
            var response = new ServiceResponse<PersonGetDto>();

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if(id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var data = await _context.People
                .Include(p => p.Nationality)
                .Include(p => p.Religion)
                .Include(p => p.EducationalDegree)
                .Include(p => p.Gender)
                .Include(p => p.MaritalStatus)
                .Include(p => p.CityOfResidence)
                .Include(p => p.BirthPlaceCity)
                .Include(p => p.MilitaryServiceStatus)
                .FirstOrDefaultAsync
                (p => p.Id == id);

            if(data == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }

            var dataDto = _mapper.Map<PersonGetDto>(data);

            response.Data = dataDto;
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetEntrancePermissions()
        {
            var response = new ServiceResponse<List<EntrancePermissionGetDto>> ();

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if (id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var data = await _context.People
                .Include(p => p.EntrancePermissions)
                .ThenInclude(e => e.Signatures)
                .FirstOrDefaultAsync
                (p => p.Id == id);

            if (data == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }

            var permissions = data.EntrancePermissions;

            var dataDto = permissions.Select(p => _mapper.Map<EntrancePermissionGetDto>(p)).ToList();

            response.Data = dataDto;
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<List<EmployeeGetDto>>> GetPersonJobs()
        {
            var response = new ServiceResponse<List<EmployeeGetDto>>();

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if(id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var person = await _context.People
                .FirstOrDefaultAsync
                    (p => p.Id == id);

            if(person == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }
            
            var data = await _context.Employees
                .Include(e => e.EmployeeType)
                .Include(e => e.Organization)
                .Where(e => e.PersonId == id)
                .Select(e => _mapper.Map<EmployeeGetDto>(e))
                .ToListAsync();


            response.Data = data;
            response.HttpResponseCode = 200;
            return response;
        }
    }
}
