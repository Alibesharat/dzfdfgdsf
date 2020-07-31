using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataService.Models
{
   public class SendDataViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsTeacher { get; set; }

        public string GroupCode { get; set; }

        public string Date { get; set; }
    }


    public class AddUserResultViewModel
    {
        public string UserName { get; set; }
        public string url { get; set; }

        public string Date { get; set; }


        public bool IsSucess { get; set; }

        public string ExMessage { get; set; }
    }
}
