using System;
using System.Collections.Generic;
using System.Text;

namespace Joho.Services.Entities.Models
{
   public class SalaryInfoRes
    {
        public int id { get; set; }
        public decimal basic { get; set; }
        public decimal hra { get; set; }
        public decimal da { get; set; }
        public decimal oa { get; set; }
        public decimal gross { get; set; }
        public int u_id { get; set; }
    }
}
