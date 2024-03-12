using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Services
{
    public class DriverCarService: ICarDriverService
    {
        private readonly IRepository repo;

        public DriverCarService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> ExistsById(string userId)
        {
            return await repo.All<DriverCar>()
                .AnyAsync(dc => dc.UserId == userId);
        }

        public async Task Create(string userId, string phoneNumber)
        {
            var driverCar = new DriverCar()
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            };

            await repo.AddAsync(driverCar);
            await repo.SaveChangesAsync();
        }

        public async Task<int> GetDriverId(string userId)
        {
            return (await repo.AllReadonly<DriverCar>()
                .FirstOrDefaultAsync(dc => dc.UserId == userId))?.DriverCarId ?? 0;
        }

        public async Task<bool> UserHasRents(string userId)
        {
            return await repo.All<CarRoute>()
                .AnyAsync(cr => cr.RenterId == userId);
        }

        public async Task<bool> UserWithPhoneNumberExists(string phoneNumber)
        {
            return await repo.All<DriverCar>()
                .AnyAsync(dc => dc.PhoneNumber == phoneNumber);
        }
    }
}
