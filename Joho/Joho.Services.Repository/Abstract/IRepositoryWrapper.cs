using System;
using System.Collections.Generic;
using System.Text;


namespace Joho.Services.Repository.Abstract
{
    public interface IRepositoryWrapper
    {
       
        
        IUserDetailsRepository User { get; }
        ISalaryDetailsRepository Salary { get; }
    }
}
