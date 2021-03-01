using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Joho.Services.Abstract;
using Joho.Web.API.Filters;
using Joho.Web.API.Filters.JwtAuthentication;
using Joho.Services.Entities.Models;
using Joho.Services.DTO.Response;

namespace Joho.Web.API.API.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : BaseController
    {
        #region Variables       
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserDetailsManager _userManger;
        #endregion

        public UserDetailsController(IConfiguration configuration, IMapper mapper, IUserDetailsManager userManger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManger = userManger;
        }
        [HttpGet("user/{id}")]
        public ActionResult<IEnumerable<string>> GetUserDetailsList(int id)
        {
            return new OkObjectResult(_userManger.GetUserDetailsList(id).Result);
        }
        [Route("GetallUserDetails")]
        [HttpGet]
        public ActionResult<IList<UserInfoRes>> GetallUserDetails()
        {
            return new OkObjectResult(_userManger.GetAllUserDetails().Result);
        }
        [Route("SaveuserDetails")]
        [HttpPost]
        public async Task<ActionResult> SaveuserDetails([FromBody] UserInfoReq userInfoReq)
        {
            CommonRespose commonRespose = await _userManger.SaveuserDetails(userInfoReq);
            return ObjectOk(commonRespose);
        }



                /// <summary>
        /// Update Post userdetails Status
        /// 1-Approved, 2- Declined
        /// </summary>
        /// <param name="userPostsUpdateinfo"></param>
        /// <returns></returns>
        [Route("UpdateUserDetailsById")]
        [HttpPut]
        public async Task<ActionResult> UpdateUserDetailsById([FromBody] UserInfoReq userInfoReq)
        {
            CommonRespose commonRespose = await _userManger.UpdateUserDetailsById(userInfoReq);
            return ObjectOk(commonRespose);

        }
        /// <summary>
        /// Delete User Post By Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [Route("DeleteUserDetailsById")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUserDetailsById(int userId)
        {
            CommonRespose commonRespose = await _userManger.DeleteUserDetailsById(userId);
            return ObjectOk(commonRespose);

        }
    }
}




  




        
