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
    public class SalaryDetailsManager: ISalaryDetailsManager
    {
        IRepositoryWrapper _irepositorywrapper;
        private readonly IMapper _mapper;
        public SalaryDetailsManager(IRepositoryWrapper irepositorywrapper, IMapper _mapper)
        {
            this._mapper = _mapper;
            this._irepositorywrapper = irepositorywrapper;
        }

        public async Task<IEnumerable<SalaryInfoRes>> GetSalaryDetailsList(int sId)
        {
            var userlist = await _irepositorywrapper.Salary.GetSalaryDetailsList(sId);

            return _mapper.Map<IList<SalaryInfoRes>>(userlist);
        }

        public async Task<IList<SalaryInfoRes>> GetAllSalaryDetails()
        {
            var alluser = await _irepositorywrapper.Salary.GetAllSalaryDetails();
            return _mapper.Map<IList<SalaryInfoRes>>(alluser);
        }
        
        public async Task<IList<usersalInfo>> GetUserSalaryDetails()
        {
            var alluser = await _irepositorywrapper.Salary.GetUserSalaryDetails();
            return _mapper.Map<IList<usersalInfo>>(alluser);
        }


        public async Task<CommonRespose> SaveSalaryDetails(SalaryInfoReq salaryInfoReq)
        {
            CommonRespose returnResponse = new CommonRespose();
            int userAdd = await _irepositorywrapper.Salary.SaveSalaryDetails(salaryInfoReq);

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


        public async Task<CommonRespose> UpdateSalaryDetailsById(SalaryInfoReq salaryInfoReq)
        {
            CommonRespose returnResponse = new CommonRespose();
            int userdetailsupdate = await _irepositorywrapper.Salary.UpdateSalaryDetailsById(salaryInfoReq);

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


        public async Task<CommonRespose> DeleteSalryDetailsById(int sId)
        {
            CommonRespose returnResponse = new CommonRespose();
            var result = await _irepositorywrapper.Salary.DeleteSalryDetailsById(sId);
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




