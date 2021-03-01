using System;
using System.Collections.Generic;
using System.Text;
using Joho.Services.Entities.Models;
using System.Threading.Tasks;

namespace Joho.Services.Repository.Abstract
{
   public  interface ISalaryDetailsRepository
    {
        public Task<IEnumerable<SalaryInfoRes>> GetSalaryDetailsList(int sId);
        public Task<IList<SalaryInfoRes>> GetAllSalaryDetails();
        public Task <IList<usersalInfo>> GetUserSalaryDetails();
        public Task<int> SaveSalaryDetails(SalaryInfoReq salaryInfoReq);
        public Task<int> UpdateSalaryDetailsById(SalaryInfoReq salaryInfoReq);
        public Task<int> DeleteSalryDetailsById(int sId);
    }
}


