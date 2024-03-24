﻿using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services
{
    public class DriverService: IDriverService
    {
        private readonly IRepository repo;

        public DriverService(IRepository _repo)
        {
            repo = _repo;
        }


        public async Task Create(string userId, string phoneNumber)
        {
            var Driver = new Driver()
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            };

            await repo.AddAsync(Driver);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(string userId)
        {
            return await repo.All<Driver>()
                .AnyAsync(dc => dc.UserId == userId);
        }
  
        public async Task<int> GetDriverId(string userId)
        {
            return (await repo.AllReadonly<Driver>()
                .FirstOrDefaultAsync(dc => dc.UserId == userId))?.DriverId ?? 0;
        }

        public async Task<bool> UserHasRents(string userId)
        {
            return await repo.All<Shipment>()
                .AnyAsync(cr => cr.RenterId == userId);
        }

        public async Task<bool> UserWithPhoneNumberExists(string phoneNumber)
        {
            return await repo.All<Driver>()
                .AnyAsync(dc => dc.PhoneNumber == phoneNumber);
        }
    }
}
