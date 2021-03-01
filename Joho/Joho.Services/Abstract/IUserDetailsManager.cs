using System;
using System.Collections.Generic;
using System.Text;
using Joho.Services.DTO.Response;
using System.Threading.Tasks;
using Joho.Services.Entities.Models;

namespace Joho.Services.Abstract
{
    public interface IUserDetailsManager
    {

        public Task<IEnumerable<UserInfoRes>> GetUserDetailsList(int userId);
        public Task<IList<UserInfoRes>> GetAllUserDetails();
        public Task<CommonRespose> SaveuserDetails(UserInfoReq userInfoReq);
        public Task<CommonRespose> UpdateUserDetailsById(UserInfoReq userInfoReq);
        public Task<CommonRespose> DeleteUserDetailsById(int userId);
    }
}




