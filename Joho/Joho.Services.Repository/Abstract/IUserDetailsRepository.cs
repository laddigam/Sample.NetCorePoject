using System;
using System.Collections.Generic;
using System.Text;
using Joho.Services.Entities.Models;
using System.Threading.Tasks;

namespace Joho.Services.Repository.Abstract
{
    public interface IUserDetailsRepository
    {
       public Task<IEnumerable<UserInfoRes>> GetUserDetailsList(int userId);
        public Task<IList<UserInfoRes>> GetAllUserDetails();
        public Task<int> SaveuserDetails(UserInfoReq userInfoReq);
        public Task<int> UpdateUserDetailsById(UserInfoReq userInfoReq);
        public Task<int> DeleteUserDetailsById(int userId);

    }
}




