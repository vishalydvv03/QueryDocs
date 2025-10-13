using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QueryDocs.Domain.Dtos;
using QueryDocs.Domain.Entities;
using QueryDocs.Infrastructure.DbContexts;
using QueryDocs.Infrastructure.ResponseHelpers;


namespace QueryDocs.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly PasswordHasher<User> hasher;
        private readonly ChatDbContext dbContext;
        public UserService(ChatDbContext dbContext, PasswordHasher<User> hasher)
        {
            this.dbContext = dbContext;
            this.hasher = hasher;
        }

        public async Task<ServiceResult> UpdateUser(int userId, UserUpdate updateModel)
        {
            var result = new ServiceResult();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                result.SetNotFound("User not found");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(updateModel.UserName))
                {
                    user.UserName = updateModel.UserName;
                }

                if (!string.IsNullOrWhiteSpace(updateModel.Email))
                {
                    user.Email = updateModel.Email;
                }

                if (!string.IsNullOrWhiteSpace(updateModel.Password))
                {
                    user.PasswordHash = hasher.HashPassword(user, updateModel.Password);
                }

                if (!string.IsNullOrWhiteSpace(updateModel.ContactNo))
                {
                    user.ContactNo = updateModel.ContactNo;
                }

                await dbContext.SaveChangesAsync();
                result.SetSuccess("User updated successfully");
            }

            return result;
        }

        public async Task<ServiceResult> DeleteUser(int userId)
        {
            var result = new ServiceResult();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                result.SetNotFound("User not found");

            }
            else
            {
                user.IsActive = false;
                await dbContext.SaveChangesAsync();
                result.SetSuccess("User deleted successfully");
            }
            return result;
        }

        public async Task<bool> IsUserAdmin(int userId)
        {
            return await dbContext.Users.AnyAsync(u => u.UserId == userId && u.IsAdmin && u.IsActive);
        }
    }
}
