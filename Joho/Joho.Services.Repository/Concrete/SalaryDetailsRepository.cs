using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using Joho.Services.Entities;
using Joho.Services.Entities.Models;
using Joho.Services.Repository.Abstract;
using System.Threading.Tasks;

namespace Joho.Services.Repository.Concrete
{
   public class SalaryDetailsRepository: ISalaryDetailsRepository
    {
        private DataBaseConnector _dataBaseConnector;
        List<NpgsqlParameter> paramralist;
        public SalaryDetailsRepository(DataBaseConnector dataBaseConnector)

        {
            _dataBaseConnector = dataBaseConnector;
        }

        public async Task<IEnumerable<SalaryInfoRes>> GetSalaryDetailsList(int sId)
        {
            try
            {
                List<SalaryInfoRes> destination = new List<SalaryInfoRes>();

                var param1 = new NpgsqlParameter("@v_id", NpgsqlDbType.Integer) { Value = sId };
                paramralist = new List<NpgsqlParameter>();
                paramralist.Add(param1);
                List<SalaryInfoRes> result = await _dataBaseConnector.ExcuteSelectOperationAsync("public.getsalarydetailsbyid", paramralist, destination);
                return result;
            }

            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IList<SalaryInfoRes>> GetAllSalaryDetails()
        {
            List<SalaryInfoRes> salAll = new List<SalaryInfoRes>();
            try
            {
                salAll = await _dataBaseConnector.ExcuteSelectOperationAsync("public.getallsalarydetils", paramralist, salAll);
                return salAll;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> SaveSalaryDetails(SalaryInfoReq salaryInfoReq)
        {
            int result = 1;
            try
            {
                paramralist = new List<NpgsqlParameter>();
                paramralist.AddRange(new List<NpgsqlParameter> {
                new NpgsqlParameter("v_basic",NpgsqlDbType.Numeric) { Value =salaryInfoReq.basic},
                new NpgsqlParameter("v_oa",NpgsqlDbType.Numeric) { Value =salaryInfoReq.oa},

                 new NpgsqlParameter("v_uid",NpgsqlDbType.Integer) { Value =salaryInfoReq.u_id}
            });

                result = await _dataBaseConnector.ExcuteCRUDOperationAsync("public.savesalaryDetails", paramralist);
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        public async Task<int> UpdateSalaryDetailsById(SalaryInfoReq salaryInfoReq)
        {
            int result = 1;
            try
            {
                paramralist = new List<NpgsqlParameter>();
                paramralist.AddRange(new List<NpgsqlParameter> {
                new NpgsqlParameter("v_sid",NpgsqlDbType.Integer){Value=salaryInfoReq.id},
                new NpgsqlParameter("v_basic",NpgsqlDbType.Numeric) { Value =salaryInfoReq.basic},
                new NpgsqlParameter("v_oa",NpgsqlDbType.Numeric) { Value =salaryInfoReq.oa},
                 new NpgsqlParameter("v_uid",NpgsqlDbType.Integer) { Value =salaryInfoReq.u_id}
            });

                result = await _dataBaseConnector.ExcuteCRUDOperationAsync("public.updatesalarydetails", paramralist);
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        public async Task<int> DeleteSalryDetailsById(int sId)
        {
            int result = 1;
            try
            {
                paramralist = new List<NpgsqlParameter>();
                paramralist.AddRange(new List<NpgsqlParameter> {
                new NpgsqlParameter("v_id",NpgsqlDbType.Integer){Value=sId}
            });

                result = await _dataBaseConnector.ExcuteCRUDOperationAsync("public.deletesalarybyid", paramralist);
                result = 1;
            }
            catch
            {
                result = 0;
            }
            return result;
        }


        public async Task<IList<usersalInfo>> GetUserSalaryDetails()
        {
            List<usersalInfo> salAll = new List<usersalInfo>();
            try
            {
                salAll = await _dataBaseConnector.ExcuteSelectOperationAsync("public.getusersalarydetails", paramralist, salAll);
                return salAll;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}


