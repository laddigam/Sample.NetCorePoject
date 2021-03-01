using Joho.Services.Entities;
using Joho.Services.Repository.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joho.Services.Repository.Concrete
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataBaseConnector _dataBaseConnector;
        private readonly IConfiguration _configuration;
       
        private ISalaryDetailsRepository _salary;


        private IUserDetailsRepository _user;
        public RepositoryWrapper(DataBaseConnector dataBaseConnector, IConfiguration configuration)
        {
            _configuration = configuration; ;
            _dataBaseConnector = dataBaseConnector;
        }

        public IUserDetailsRepository User
        {
            get
            {
                if(_user==null)
                {
                    _user = new UserDetailsRepository(_dataBaseConnector);
                }
                return _user;
            }
        }


        public ISalaryDetailsRepository Salary
        {
            get
            {
                if (_salary == null)
                {
                    _salary = new SalaryDetailsRepository(_dataBaseConnector);
                }

                return _salary;
            }
        }



     
    
      
    }
}
