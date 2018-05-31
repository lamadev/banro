using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanroWebApp.Models
{
    public class Authenticate
    {
        public int id { set; get; }
        public String username { set; get; }
        public String password { set; get; }
        public String Succursale { set; get; }
        public String Priority { set; get; }
        public String nameSuccursale { set; get; }
        public int idEmpolyee { set; get; }
    }
}