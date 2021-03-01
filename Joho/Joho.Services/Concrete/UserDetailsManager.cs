using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Joho.Services.Abstract;
using Joho.Services.DTO.Response;
using Joho.Services.Repository.Abstract;
using System.Threading.Tasks;
using Joho.Services.Entities.Models;

namespace Joho.Services.Concrete
{
    public class UserDetailsManager: IUserDetailsManager
    {
        IRepositoryWrapper _irepositorywrapper;
        private readonly IMapper _mapper;
        public UserDetailsManager(IRepositoryWrapper irepositorywrapper, IMapper _mapper)
        {
            this._mapper = _mapper;
            this._irepositorywrapper = irepositorywrapper;
        }
       
             public async Task<IEnumerable<UserInfoRes>> GetUserDetailsList(int userId)
        {
            var userlist = await _irepositorywrapper.User.GetUserDetailsList(userId);

            return _mapper.Map<IList<UserInfoRes>>(userlist);
        }
        
          public async Task<IList<UserInfoRes>> GetAllUserDetails()
        {
            var alluser = await _irepositorywrapper.User.GetAllUserDetails();
            return _mapper.Map<IList<UserInfoRes>>(alluser);
        }

        
             public async Task<CommonRespose> SaveuserDetails(UserInfoReq userInfoReq)
        {
            CommonRespose returnResponse = new CommonRespose();
            int userAdd = await _irepositorywrapper.User.SaveuserDetails(userInfoReq);

            if (userAdd == 1)
            {
                returnResponse.Id = 1;
                returnResponse.Status = true;
                returnResponse.Message = "Saved Succesfully! ";

                return returnResponse;

            }
            else
            {
                returnResponse.Id = 0;
                returnResponse.Status = false;
                returnResponse.Message = "Saved failed! ";

                return returnResponse;

            }
        }




   
            public async Task<CommonRespose> UpdateUserDetailsById(UserInfoReq userInfoReq)
        {
            CommonRespose returnResponse = new CommonRespose();
            int userdetailsupdate = await _irepositorywrapper.User.UpdateUserDetailsById(userInfoReq);

            if (userdetailsupdate == 1)
            {
                returnResponse.Id = 1;
                returnResponse.Status = true;
                returnResponse.Message = "Updated Succesfully! ";

                return returnResponse;
            }
            else
            {
                returnResponse.Id = 0;
                returnResponse.Status = false;
                returnResponse.Message = "Update failed! ";

                return returnResponse;
            }
        }

        
          public async Task<CommonRespose> DeleteUserDetailsById(int userId)
        {
            CommonRespose returnResponse = new CommonRespose();
            var result = await _irepositorywrapper.User.DeleteUserDetailsById(userId);
            if (result == 1)
            {
                returnResponse.Id = 1;
                returnResponse.Status = true;
                returnResponse.Message = "Deleted Succesfully! ";

                return returnResponse;

            }
            else
            {
                returnResponse.Id = 0;
                returnResponse.Status = false;
                returnResponse.Message = "Deletion failed! ";

                return returnResponse;

            }
        }


    }
}


   