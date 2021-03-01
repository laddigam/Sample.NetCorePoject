using System;
using System.Collections.Generic;
using System.Text;
using Joho.Services.DTO.Response;
using System.Threading.Tasks;
using Joho.Services.Entities.Models;

namespace Joho.Services.Abstract
{
   
    
    
    public interface  ISalaryDetailsManager
    {
        public Task<IEnumerable<SalaryInfoRes>> GetSalaryDetailsList(int sId);
        public Task<IList<SalaryInfoRes>> GetAllSalaryDetails();
        public Task<IList<usersalInfo>> GetUserSalaryDetails();
        public Task<CommonRespose> SaveSalaryDetails(SalaryInfoReq salaryInfoReq);
        public Task<CommonRespose> UpdateSalaryDetailsById(SalaryInfoReq salaryInfoReq);
        public Task<CommonRespose> DeleteSalryDetailsById(int sId);
    }
}

