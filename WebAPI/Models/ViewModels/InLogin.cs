using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ViewModels
{
    public class InLogin
    {
        public string Account { get; set; }

        public string PWD { get; set; }

        public string IPAddress { get; set; }

        //public bool Rememberme { get; set; }
    }
}
