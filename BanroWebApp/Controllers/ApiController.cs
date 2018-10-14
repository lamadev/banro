using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BanroWebApp.Controllers
{
    public class ApiController : Controller
    {
        BanroWebApp.Models.BANROEntities dbContext = new Models.BANROEntities();
        // GET: View
        public ActionResult Index()
        {
            return View();
        }

        [Route("api/employeecharts")]
        public String getEmployeeByDepartments()
        {
           

            List<Dictionary<String, Object>> ViewDictionary = new List<Dictionary<string, object>>();
            List<List<Object>> Array = new List<List<object>>();
            List<Object> departArray = new List<Object>();
            List<Object> countEmployee = new List<Object>();
            foreach (var item in dbContext.t_departement)
            {
                departArray.Add(item.C_id_depart);
            }
            foreach (var item in dbContext.t_departement)
            {
                var QueryEmp = from emp in dbContext.t_beneficiaires
                               where emp.C_id_depart==item.C_id
                               select emp;
     
                countEmployee.Add(QueryEmp.ToList().Count);
            }
          
            Array.Add(departArray);
            Array.Add(countEmployee);
            JavaScriptSerializer ser=new JavaScriptSerializer();
            return ser.Serialize(Array);
            

            
        }
        [Route("api/voucherschart")]
        public String getVouchersByFacilities()
        {
            List<List<Object>> Array = new List<List<object>>();
            List<Object> facilities = new List<object>();
            List<Object> counterVouchers = new List<object>();
            

            foreach (var item in dbContext.t_centre_soins)
            {
                var query = from ds in dbContext.t_bon_commandes
                            where ds.C_datedeb.EndsWith("/2018") && ds.C_id_centre == item.C_id_centre
                            select ds;

                counterVouchers.Add(query.ToList().Count);
            }

            foreach (var item in dbContext.t_centre_soins)
            {
                facilities.Add(item.C_name);
            }
            Array.Add(facilities);
            Array.Add(counterVouchers);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(Array);
        }
        
        
        
    }
}