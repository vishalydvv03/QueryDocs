using QueryDocs.Domain.Dtos;
using QueryDocs.Infrastructure.ResponseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDocs.Services.UserServices
{
    public interface IUserService
    {
        Task<ServiceResult> UpdateUser(int userId, UserUpdate updateModel);
        Task<ServiceResult> DeleteUser(int userId);
        Task<bool> IsUserAdmin(int userId);
    }
}
