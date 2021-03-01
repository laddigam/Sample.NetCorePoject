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
    public class SalaryDetailsController : BaseController
    {
        #region Variables       
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ISalaryDetailsManager _salaryManger;
        #endregion
        public SalaryDetailsController(IConfiguration configuration, IMapper mapper, ISalaryDetailsManager salaryManger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _salaryManger = salaryManger;
        }
        [HttpGet("GetSalaryDetailsList/{id}")]
        public ActionResult<IEnumerable<string>> GetSalaryDetailsList(int id)
        {
            return new OkObjectResult(_salaryManger.GetSalaryDetailsList(id).Result);
        }
        [Route("GetAllSalaryDetails")]
        [HttpGet]
        public ActionResult<IList<UserInfoRes>> GetAllSalaryDetails()
        {
            return new OkObjectResult(_salaryManger.GetAllSalaryDetails().Result);
        }

        [Route("GetAllUserSalaryDetails")]
        [HttpGet]
        public ActionResult<IList<usersalInfo>> GetUserSalaryDetails()
        {
            return new OkObjectResult(_salaryManger.GetUserSalaryDetails().Result);
        }
        [Route("SaveSalaryDetails")]
        [HttpPost]
        public async Task<ActionResult> SaveSalaryDetails([FromBody] SalaryInfoReq salaryInfoReq)
        {
            CommonRespose commonRespose = await _salaryManger.SaveSalaryDetails(salaryInfoReq);
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
        public async Task<ActionResult> UpdateUserDetailsById([FromBody] SalaryInfoReq salaryInfoReq)
        {
            CommonRespose commonRespose = await _salaryManger.UpdateSalaryDetailsById(salaryInfoReq);
            return ObjectOk(commonRespose);

        }
        /// <summary>
        /// Delete User Post By Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [Route("DeleteSalryDetailsById")]
        [HttpDelete]
        public async Task<ActionResult> DeleteSalryDetailsById(int sId)
        {
            CommonRespose commonRespose = await _salaryManger.DeleteSalryDetailsById(sId);
            return ObjectOk(commonRespose);

        }
    }
}




