using System;
using System.Collections.Generic;
using System.Text;

namespace Joho.Services.Entities.Models
{
   public  class UserInfoReq
    {
        public int id { get; set; }
        public string uname { get; set; }
        public string email { get; set; }
        public int age { get; set; }
        public string  address { get; set; }
        public string designation { get; set; }
    }
}
