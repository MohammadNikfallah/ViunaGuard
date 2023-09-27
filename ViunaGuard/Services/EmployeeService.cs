using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ViunaGuard.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeService(IMapper mapper, DataContext context,IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse> PostPeriodicShift(PeriodicShiftPostDto shift, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e =>  e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;

        if (!employee!.UserAccessRole.UserAccess.CanChangeShifts)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't add shifts";
            return response;
        }

        var employeePeriodicShift = _mapper.Map<EmployeePeriodicShift>(shift);
        employeePeriodicShift.OrganizationId = employee!.OrganizationId;
        employeePeriodicShift.ShiftMakerEmployeeId = employeeId;
        
        await _context.EmployeePeriodicShifts.AddAsync(employeePeriodicShift);
        await _context.SaveChangesAsync();
            
        response.HttpResponseCode=200;
        return response;
    }

    private bool CheckEmployeeId(Employee? employee, ServiceResponse response, out ServiceResponse serviceResponse)
    {
        if (employee == null)
        {
            response.HttpResponseCode = 400;
            response.Message = "There is no Employee with this id";
            {
                serviceResponse = response;
                return true;
            }
        }

        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
        if (employee.PersonId != userId!)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant access this employee";
            {
                serviceResponse = response;
                return true;
            }
        }

        serviceResponse = new ServiceResponse();
        return false;
    }

    public async Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift, int employeeId)
    {
        var response = new ServiceResponse<List<EmployeeShift>>();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e =>  e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse))
        {
            response.HttpResponseCode = serviceResponse.HttpResponseCode;
            response.Message = serviceResponse.Message;
            return response;
        };

        if (!employee!.UserAccessRole.UserAccess.CanChangeShifts)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't add shifts";
            return response;
        }

        var shiftEmployee = await _context.Employees.FindAsync(shift.EmployeeId);
        if (shiftEmployee == null)
        {
            response.HttpResponseCode = 404;
            response.Message = "Employee Not Found";
            return response;
        }

        if (shiftEmployee.OrganizationId != employee!.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add shift for this employee";
            return response;
        }


        var employeeShift = _mapper.Map<EmployeeShift>(shift);
        employeeShift.ShiftMakerEmployeeId = employeeId;
        employeeShift.OrganizationId = employee.OrganizationId;
        
        await _context.EmployeeShifts.AddAsync(employeeShift);
        await _context.SaveChangesAsync();
        var shifts = await _context.EmployeeShifts.ToListAsync();

        response.Data = shifts;
        response.HttpResponseCode=200;
        return response;
    }

    public async Task<ServiceResponse<TwoShiftGetDto>> GetEmployeeShifts(int employeeId)
    {
        var response = new ServiceResponse<TwoShiftGetDto>();
        var employee = await _context.Employees.FindAsync(employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse))
        {
            response.Message = serviceResponse.Message;
            response.HttpResponseCode = serviceResponse.HttpResponseCode;
            return response;
        }

        response.Data = new TwoShiftGetDto();
        var time = DateTime.Now;

        response.Data.TodayShifts.AddRange(await GetDayShifts(time, employeeId));
        response.Data.TomorrowShifts.AddRange(await GetDayShifts(time.AddDays(1), employeeId));
        response.HttpResponseCode = 200;
        return response;
    }

    private async Task<List<EmployeeShiftGetDto>> GetDayShifts(DateTime time, int employeeId)
    {
        var result = new List<EmployeeShiftGetDto>();
        var pc = new PersianCalendar();
            
        var monthlyShift = _context.EmployeeShiftsPeriodicMonthly
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayMonthlyShifts = monthlyShift
            .Where(e => e.DayOfMonth == pc.GetDayOfMonth(time));
            
        var tempResult = await todayMonthlyShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        foreach (var t in tempResult)
        {
            t.StartTime = t.StartTime.AddDays(time.DayOfYear - t.StartTime.DayOfYear);
            t.StartTime = t.StartTime.AddYears(time.Year - t.StartTime.Year);
            t.FinishTime = t.FinishTime.AddDays(time.DayOfYear - t.FinishTime.DayOfYear);
            t.FinishTime = t.FinishTime.AddYears(time.Year - t.FinishTime.Year);
        }
        result.AddRange(tempResult);
        
        var periodicShift = _context.EmployeePeriodicShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayPeriodicShifts = periodicShift
            .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
            
        tempResult = await todayPeriodicShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        foreach (var t in tempResult)
        {
            t.StartTime = t.StartTime.AddDays(time.DayOfYear - t.StartTime.DayOfYear);
            t.StartTime = t.StartTime.AddYears(time.Year - t.StartTime.Year);
            t.FinishTime = t.FinishTime.AddDays(time.DayOfYear - t.FinishTime.DayOfYear);
            t.FinishTime = t.FinishTime.AddYears(time.Year - t.FinishTime.Year);
        }
        result.AddRange(tempResult);

        var shift = _context.EmployeeShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var nowShift = shift.Where(e => time.Date <= e.FinishTime.Date && time.Date >= e.StartTime.Date);
        tempResult = await nowShift.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        foreach (var t in tempResult)
        {
            if (t.StartTime.AddDays(1).Date <= time.Date)
                t.StartTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MinValue);
            if (t.FinishTime.AddDays(-1).Date >= time.Date)
                t.FinishTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MaxValue);
        }
        result.AddRange(tempResult);

        return result;
    }

    public async Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift(int employeeId)
    {
        var response = new ServiceResponse<EmployeeShiftGetDto>();
        var employee = await _context.Employees.FindAsync(employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse))
        {
            response.Message = serviceResponse.Message;
            response.HttpResponseCode = serviceResponse.HttpResponseCode;
            return response;
        }

        var time = DateTime.Now;
            
        var periodicShift = _context.EmployeePeriodicShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayPeriodicShifts = periodicShift
            .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
        var nowPeriodicShift = await todayPeriodicShifts
            .FirstOrDefaultAsync(p => time.TimeOfDay > p.StartTime.TimeOfDay && time.TimeOfDay < p.FinishTime.TimeOfDay);
        if (nowPeriodicShift != null)
        {
            var dayDifference = time.DayOfYear - nowPeriodicShift.StartTime.DayOfYear;
            var yearDifference = time.Year - nowPeriodicShift.StartTime.Year;
            nowPeriodicShift.StartTime = nowPeriodicShift.StartTime.AddDays(dayDifference);
            nowPeriodicShift.StartTime = nowPeriodicShift.StartTime.AddYears(yearDifference);
            nowPeriodicShift.FinishTime = nowPeriodicShift.FinishTime.AddDays(dayDifference);
            nowPeriodicShift.FinishTime = nowPeriodicShift.FinishTime.AddYears(yearDifference);
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowPeriodicShift);
            return response;
        }

        var pc = new PersianCalendar();
            
        var monthlyShift = _context.EmployeeShiftsPeriodicMonthly
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayMonthlyShifts = monthlyShift
            .Where(e => e.DayOfMonth == pc.GetDayOfMonth(time));
        var nowMonthlyShift = await todayMonthlyShifts
            .FirstOrDefaultAsync(p => time.TimeOfDay > p.StartTime.TimeOfDay && time.TimeOfDay < p.FinishTime.TimeOfDay);
        if (nowMonthlyShift != null)
        {
            var dayDifference = time.DayOfYear - nowMonthlyShift.StartTime.DayOfYear;
            var yearDifference = time.Year - nowMonthlyShift.StartTime.Year;
            nowMonthlyShift.StartTime = nowMonthlyShift.StartTime.AddDays(dayDifference);
            nowMonthlyShift.StartTime = nowMonthlyShift.StartTime.AddYears(yearDifference);
            nowMonthlyShift.FinishTime = nowMonthlyShift.FinishTime.AddDays(dayDifference);
            nowMonthlyShift.FinishTime = nowMonthlyShift.FinishTime.AddYears(yearDifference);
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowMonthlyShift);
            return response;
        }
            
        var shift = _context.EmployeeShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var nowShift = await shift
            .FirstOrDefaultAsync(e => time.Date < e.FinishTime.Date && time.Date > e.StartTime.Date);
        if (nowShift != null)
        {
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowShift);
            return response;
        }
            
        response.HttpResponseCode = 404;
        response.Message = "There is no current shift for this employee";
        return response;
    }

    public async Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetPermissionsToSign(int employeeId)
    {
        var response = new ServiceResponse<List<EntrancePermissionGetDto>>();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse))
        {
            response.Message = serviceResponse.Message;
            response.HttpResponseCode = serviceResponse.HttpResponseCode;
            return response;
        }

        if (!employee.UserAccessRole.UserAccess.CanSeeEntrancePermissions)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't see the entrance permissions";
            return response;
        }

        var permissions = await 
            _context.EntrancePermissions
                .Include(e => e.Signatures)
                .Include(e => e.Person)
                .Include(e => e.Car)
                .Where(e => e.OrganizationId == employee.OrganizationId && e.PermissionGranted == false && e.EndValidityTime > DateTime.Now)
                .Select(e => _mapper.Map<EntrancePermissionGetDto>(e))
                .ToListAsync();

        var permissionNeeds = await _context.SignatureNeedForEntrancePermissions
            .Where(s => s.OrganizationId == employee.OrganizationId)
            .ToListAsync();
        permissionNeeds.Sort((p,p1) 
            => p.MinAuthorityLevel - p1.MinAuthorityLevel);

        var result = new List<EntrancePermissionGetDto>();

        foreach (var permission in permissions)
        {
            if (permissionNeeds[permission.Signatures.Count].MinAuthorityLevel <=
                employee.UserAccessRole.UserAccess.AuthorityLevel)
            {
                result.Add(permission);
            }
        }
        
        response.HttpResponseCode = 200;
        response.Data = result;
        return response;
    }

    public async Task<ServiceResponse> PostEmployee(EmployeePostDto employeePostDto, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;

        if (!employee!.UserAccessRole.UserAccess.CanAddEmployees)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add employees";
            return response;
        }

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        response.HttpResponseCode = 200;
        return response;
    }

    public async Task<ServiceResponse> PostEmployeeWeeklyShift(WeeklyShiftPostDto weeklyShiftPostDto, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e =>  e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;

        if (!employee!.UserAccessRole.UserAccess.CanChangeShifts)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't add shifts";
            return response;
        }

        var shiftEmployee = await _context.Employees.FindAsync(weeklyShiftPostDto.EmployeeId);
        if (shiftEmployee == null)
        {
            response.HttpResponseCode = 404;
            response.Message = "Target Employee Not Found";
            return response;
        }

        if (shiftEmployee.OrganizationId != employee.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add shift for this employee";
            return response;
        }


        var periodicShift = _mapper.Map<EmployeePeriodicShift>(weeklyShiftPostDto);
        periodicShift.PeriodDayRange = 7;
        periodicShift.OrganizationId = employee.OrganizationId;
        periodicShift.ShiftMakerEmployeeId = employeeId;
        
        periodicShift.StartTime = periodicShift.StartTime.AddYears(DateTime.Now.Year - periodicShift.StartTime.Year);
        periodicShift.StartTime = periodicShift.StartTime.AddMonths(DateTime.Now.Month - periodicShift.StartTime.Month);
        periodicShift.StartTime = periodicShift.StartTime.AddDays
            ((weeklyShiftPostDto.DayOfWeek - ((int) DateTime.Now.DayOfWeek + 1) % 7) + (DateTime.Now.DayOfYear - periodicShift.StartTime.DayOfYear));
        
        periodicShift.FinishTime = periodicShift.FinishTime.AddYears(DateTime.Now.Year - periodicShift.FinishTime.Year);
        periodicShift.FinishTime = periodicShift.FinishTime.AddMonths(DateTime.Now.Month - periodicShift.FinishTime.Month);
        periodicShift.FinishTime = periodicShift.FinishTime.AddDays
            ((weeklyShiftPostDto.DayOfWeek - ((int) DateTime.Now.DayOfWeek + 1) % 7) + (DateTime.Now.DayOfYear - periodicShift.FinishTime.DayOfYear));

        await _context.EmployeePeriodicShifts.AddAsync(periodicShift);
        await _context.SaveChangesAsync();

        response.HttpResponseCode = 200;
        return response;
    }

    public async Task<ServiceResponse> PostEmployeeMonthlyShift(MonthlyShiftPostDto monthlyShiftPostDto, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e =>  e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;

        if (!employee!.UserAccessRole.UserAccess.CanChangeShifts)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't add shifts";
            return response;
        }

        var shiftEmployee = await _context.Employees.FindAsync(monthlyShiftPostDto.EmployeeId);
        if (shiftEmployee == null)
        {
            response.HttpResponseCode = 404;
            response.Message = "Employee Not Found";
            return response;
        }

        if (shiftEmployee.OrganizationId != employee!.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add shift for this employee";
            return response;
        }

        var monthlyShift = _mapper.Map<EmployeeShiftPeriodicMonthly>(monthlyShiftPostDto);
        monthlyShift.ShiftMakerEmployeeId = employeeId;
        monthlyShift.OrganizationId = employee.OrganizationId;

        await _context.EmployeeShiftsPeriodicMonthly.AddAsync(monthlyShift);
        await _context.SaveChangesAsync();

        response.HttpResponseCode = 200;
        return response;
    }

    public async Task<ServiceResponse> SignEntrancePermission(int entrancePermissionId, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstAsync(e => e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;

        if (!employee.UserAccessRole.UserAccess.CanSignEntrancePermissions)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't sign entrance Permissions";
            return response;
        }

        var permission = await _context.EntrancePermissions
            .Include(e => e.Signatures)
            .FirstAsync(e => e.Id == entrancePermissionId);

        if (permission.OrganizationId != employee.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "you cant access this entrance permission";
            return response;
        }
        
        if (permission.EndValidityTime < DateTime.Now)
        {
            response.HttpResponseCode = 400;
            response.Message = "this Entrance permission has been expired";
            return response;
        }

        var permissionNeeds = await _context.SignatureNeedForEntrancePermissions
            .Where(s => s.OrganizationId == employee.OrganizationId)
            .ToListAsync();
        
        if (permissionNeeds[permission.Signatures.Count].MinAuthorityLevel <=
            employee.UserAccessRole.UserAccess.AuthorityLevel)
        {
            if (permission.Signatures.Count + 1 == permissionNeeds.Count)
            {
                permission.PermissionGranted = true;
                _context.EntrancePermissions.Update(permission);
            }
            var sign = new EntranceSignaturePostDto()
            {
                OrganizationId = employee.OrganizationId,
                AuthorityLevel = employee.UserAccessRole.UserAccess.AuthorityLevel,
                Time = DateTime.Now,
                EntrancePermissionId = permission.Id,
                SignedByEmployeeId = employeeId
            };
            await _context.SignedEntrancePermissions.AddAsync(_mapper.Map<SignedEntrancePermission>(sign));
            await _context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            return response;
        }

        response.HttpResponseCode = 400;
        response.Message = "you dont need to sign this entrance permission";
        return response;
    }
    
    public async Task<ServiceResponse> RevokeEntrancePermission(int entrancePermissionId, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstAsync(e => e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;

        if (!employee.UserAccessRole.UserAccess.CanRevokeEntrancePermissions)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't Revoke entrance Permissions";
            return response;
        }

        var permission = await _context.EntrancePermissions
            .FindAsync(entrancePermissionId);

        if (permission.OrganizationId != employee.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "you cant access this entrance permission";
            return response;
        }
        
        if (permission.EndValidityTime < DateTime.Now)
        {
            response.HttpResponseCode = 400;
            response.Message = "this Entrance permission has been expired";
            return response;
        }

        var permissionNeeds = await _context.SignatureNeedForEntrancePermissions
            .Where(s => s.OrganizationId == employee.OrganizationId)
            .ToListAsync();
        
        if (permissionNeeds[permission.Signatures.Count].MinAuthorityLevel <=
            employee.UserAccessRole.UserAccess.AuthorityLevel)
        {
            permission.Revoked = true;
            
            _context.EntrancePermissions.Update(permission);
            await _context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            return response;
        }

        response.HttpResponseCode = 400;
        response.Message = "you can't Revoke this entrance permission";
        return response;
    }

    public async Task<ServiceResponse> SignVisitedPlace(int entrancePermissionId, int employeeId)
    {
        var response = new ServiceResponse();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstAsync(e => e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse)) return serviceResponse;
        
        if (!employee.UserAccessRole.UserAccess.CanSignVisitedPlace)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't Sign visited place";
            return response;
        }

        var permission = await _context.EntrancePermissions
            .FindAsync(entrancePermissionId);

        var currentShift = await GetCurrentShift(employeeId);

        if (permission.OrganizationPlaceId != currentShift.Data.WorkPlace.Id)
        {
            response.HttpResponseCode = 400;
            response.Message = "you cant access this entrance permission";
            return response;
        }
        
        permission.DidVisitOrgPlace = true;
        permission.OrgPlaceSignEmployeeId = employeeId;
        
        _context.EntrancePermissions.Update(permission);
        await _context.SaveChangesAsync();
        response.HttpResponseCode = 200;
        return response;
    }

    public async Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetPermissions(int employeeId)
    {
        var response = new ServiceResponse<List<EntrancePermissionGetDto>>();
        var employee = await _context.Employees
            .Include(e => e.UserAccessRole)
            .ThenInclude(e => e.UserAccess)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
        if (CheckEmployeeId(employee, response, out var serviceResponse))
        {
            response.Message = serviceResponse.Message;
            response.HttpResponseCode = serviceResponse.HttpResponseCode;
            return response;
        }

        if (employee.UserAccessRole.UserAccess.CanSeeEntrancePermissions)
        {
            response.HttpResponseCode = 400;
            response.Message = "You can't see entrance permissions";
            return response;
        }

        var permissions = await 
            _context.EntrancePermissions
                .Include(e => e.Signatures)
                .Include(e => e.Person)
                .Include(e => e.Car)
                .Where(e => e.OrganizationId == employee.OrganizationId && e.EndValidityTime > DateTime.Now)
                .Select(e => _mapper.Map<EntrancePermissionGetDto>(e))
                .ToListAsync();

        response.HttpResponseCode = 200;
        response.Data = permissions;
        return response;    
    }
}