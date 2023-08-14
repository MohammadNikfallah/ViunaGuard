using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public interface IGuardService
    {
        public Task<ServiceResponse<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate
            , [Required] int employeeId, int doorId, int personId
            , int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId);
        public Task<ServiceResponse<Entrance>> PostEntrance(EntrancePostDto entrancePostDto, int employeeId);
        public Task<ServiceResponse<object>> PostSameGroupEntrances
            (List<EntrancePostDto> entrancePostDtos, int driverID, [Required] int employeeId);
    }
}
