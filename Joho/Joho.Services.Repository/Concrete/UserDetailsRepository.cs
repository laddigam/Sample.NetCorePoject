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
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private DataBaseConnector _dataBaseConnector;
        List<NpgsqlParameter> paramralist;
        public UserDetailsRepository(DataBaseConnector dataBaseConnector)

        {
            _dataBaseConnector = dataBaseConnector;
        }

        public async Task<IEnumerable<UserInfoRes>> GetUserDetailsList(int userId)
        {
            try
            {
                List<UserInfoRes> destination = new List<UserInfoRes>();

                var param1 = new NpgsqlParameter("@v_id", NpgsqlDbType.Integer) { Value = userId };
                paramralist = new List<NpgsqlParameter>();
                paramralist.Add(param1);
                List<UserInfoRes> result = await _dataBaseConnector.ExcuteSelectOperationAsync("getuserdetailsbyid", paramralist, destination);
                return result;
            }

            catch (Exception e)
            {
                return null;
            }
        }

            public async Task<IList<UserInfoRes>> GetAllUserDetails()
            {
                List<UserInfoRes> userAll = new List<UserInfoRes>();
                try
                {
                    userAll = await _dataBaseConnector.ExcuteSelectOperationAsync("public.getuserdetails", paramralist, userAll);
                    return userAll;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
    


        public async Task<int> SaveuserDetails(UserInfoReq userInfoReq)
        {
            int result = 1;
            try
            {
                paramralist = new List<NpgsqlParameter>();
                paramralist.AddRange(new List<NpgsqlParameter> {
                new NpgsqlParameter("v_uname",NpgsqlDbType.Text) { Value =userInfoReq.uname},
                new NpgsqlParameter("v_email",NpgsqlDbType.Text) { Value =userInfoReq.email},
                new NpgsqlParameter("v_age",NpgsqlDbType.Integer) { Value =userInfoReq.age},
                new NpgsqlParameter("v_address",NpgsqlDbType.Text) { Value =userInfoReq.address},
                new NpgsqlParameter("v_designation",NpgsqlDbType.Text) { Value =userInfoReq.designation}
            });

                result = await _dataBaseConnector.ExcuteCRUDOperationAsync("saveuserdetails", paramralist);
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

   
            public async Task<int> UpdateUserDetailsById(UserInfoReq userInfoReq)
        {
            int result = 1;
            try
            {
                paramralist = new List<NpgsqlParameter>();
                paramralist.AddRange(new List<NpgsqlParameter> {
                new NpgsqlParameter("v_id",NpgsqlDbType.Integer){Value=userInfoReq.id},
                new NpgsqlParameter("v_uname",NpgsqlDbType.Text) { Value =userInfoReq.uname},
                new NpgsqlParameter("v_email",NpgsqlDbType.Text) { Value =userInfoReq.email},
                new NpgsqlParameter("v_age",NpgsqlDbType.Integer) { Value =userInfoReq.age},
                new NpgsqlParameter("v_address",NpgsqlDbType.Text) { Value =userInfoReq.address},
                new NpgsqlParameter("v_designation",NpgsqlDbType.Text) { Value =userInfoReq.designation}
            });

                result = await _dataBaseConnector.ExcuteCRUDOperationAsync("public.updateuserDetails", paramralist);
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        
            public async Task<int> DeleteUserDetailsById(int userId)
        {
            int result = 1;
            try
            {
                paramralist = new List<NpgsqlParameter>();
                paramralist.AddRange(new List<NpgsqlParameter> {
                new NpgsqlParameter("v_id",NpgsqlDbType.Integer){Value=userId}
            });

                result = await _dataBaseConnector.ExcuteCRUDOperationAsync("public.deleteuserbyid", paramralist);
                result = 1;
            }
            catch
            {
                result = 0;
            }
            return result;
        }


    }
    }






   

