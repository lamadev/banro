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

        [Route("api/voucherperdepartment")]
        public String getVoucherPerDepartment()
        {
            BanroWebApp.Models.BANROEntities db = new BanroWebApp.Models.BANROEntities();
            List<String> lDict = new List<String>();
            var Query = from ds in db.t_beneficiaires
                        join vouchers in db.t_bon_commandes on ds.C_id equals vouchers.C_id_bene
                        where vouchers.C_datedeb.EndsWith("/" + DateTime.Now.Year)
                        select new { ds, vouchers };


            foreach (var item in Query)
            {
                Dictionary<String,Object> dictionary = new Dictionary<string, object>();
                String typeB = "";
                String idEmployee = "";
                String company = "";
               // String statusVoucher = "";
                String Department = "";
                if (!String.IsNullOrEmpty(item.ds.C_mat))
                {
                    typeB = "Employee";
                }
                if (!String.IsNullOrEmpty(item.ds.C_id_visitor))
                {
                    typeB = "Visitor";
                }
                if (!String.IsNullOrEmpty(item.ds.C_id_partenaire) && String.IsNullOrEmpty(item.ds.C_mat))
                {
                    typeB = "Partner";
                }
                if (!String.IsNullOrEmpty(item.ds.C_id_parent))
                {
                    typeB = "Children";
                }
                if (typeB.Equals("Employee"))
                {
                    company = db.t_succursales.Where(s => s.C_id.Equals(item.ds.C_id_succ)).FirstOrDefault().C_name;
                    int currentID = (int)item.ds.C_id_depart;
                    Department =
                        (
                            db.t_departement.Where(e => e.C_id == currentID) == null ? "" : db.t_departement.Where(e => e.C_id == currentID).FirstOrDefault().C_id_depart
                        );

                    if (!Department.Equals(""))
                    {
                        lDict.Add(Department);
                    }
                        
                    idEmployee = item.ds.C_mat;
                }
                if (typeB.Equals("Partner"))
                {
                    int id = int.Parse(item.ds.C_id_partenaire);
                    var QueryEmployee = (from ds in db.t_beneficiaires
                                         join succ in db.t_succursales on ds.C_id_succ equals succ.C_id
                                         where ds.C_id == id
                                         select new { ds, succ }).FirstOrDefault();
                    if (QueryEmployee != null)
                    {
                        company = QueryEmployee.succ.C_name;
                        idEmployee = QueryEmployee.ds.C_mat;

                        int currentID = (int)QueryEmployee.ds.C_id_depart;
                        Department =
                            (
                                db.t_departement.Where(e => e.C_id == currentID) == null ? "" : db.t_departement.Where(e => e.C_id == currentID).FirstOrDefault().C_id_depart
                            );

                        if (!Department.Equals(""))
                        {
                            //dictionary.Add("dep", Department);
                            //dictionary.Add("voucher", item.vouchers.C_id_bon);
                            lDict.Add(Department);
                        }
                    }

                    


                }
                if (typeB.Equals("Children"))
                {
                    int id = int.Parse(item.ds.C_id_parent);
                    var QueryEmployee = (from ds in db.t_beneficiaires
                                         join succ in db.t_succursales on ds.C_id_succ equals succ.C_id
                                         where ds.C_id == id
                                         select new { ds, succ }).FirstOrDefault();

                    if (QueryEmployee!=null)
                    {
                        int currentID = (int)QueryEmployee.ds.C_id_depart;
                        Department =
                            (
                                db.t_departement.Where(e => e.C_id == currentID) == null ? "" : db.t_departement.Where(e => e.C_id == currentID).FirstOrDefault().C_id_depart
                            );

                        if (!Department.Equals(""))
                        {
                            //dictionary.Add("dep", Department);
                            //dictionary.Add("voucher", item.vouchers.C_id_bon);
                            lDict.Add(Department);
                        }
                        company = QueryEmployee.succ.C_name;
                        idEmployee = QueryEmployee.ds.C_mat;
                    }

                   
                }
               // lDict.Add(dictionary);
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Object> lstDep = new List<Object>();
            List<Object> dataVouchers = new List<Object>();
            List<List<Object>> DataFullGet = new List<List<object>>();
            foreach (var item in db.t_departement)
            {
                int ctr = 0;
                lstDep.Add(item.C_id_depart);
                var sqlDict = from data in lDict
                              where data.Equals(item.C_id_depart)
                              select data;


                dataVouchers.Add(sqlDict.ToList().Count);

                

            }
            DataFullGet.Add(lstDep);
            DataFullGet.Add(dataVouchers);
            return js.Serialize(DataFullGet);
        }
        
        [Route("api/voucherbeneficiairies")]
        public String getVoucherByBeneficiairy()
        {
            BanroWebApp.Models.BANROEntities db = new BanroWebApp.Models.BANROEntities();
            List<String> lDict = new List<String>();
            List<String> lemployee = new List<String>();
            List<String> lspouse = new List<String>();
            List<String> lchildren = new List<String>();
            List<String> lcasual = new List<String>();
            List<String> lcontractor = new List<String>();
            List<String> lvisitor = new List<String>();
            var Query = from ds in db.t_beneficiaires
                        join vouchers in db.t_bon_commandes on ds.C_id equals vouchers.C_id_bene
                        where vouchers.C_datedeb.EndsWith("/" + DateTime.Now.Year)
                        select new { ds, vouchers };


            foreach (var item in Query)
            {
                Dictionary<String, Object> dictionary = new Dictionary<string, object>();
                String typeB = "";
                String idEmployee = "";
                String company = "";
                // String statusVoucher = "";
                String Department = "";
                if (!String.IsNullOrEmpty(item.ds.C_mat))
                {
                    typeB = "Employee";
                }
                if (!String.IsNullOrEmpty(item.ds.C_id_visitor))
                {
                    typeB = "Visitor";
                }
                if (!String.IsNullOrEmpty(item.ds.C_id_partenaire) && String.IsNullOrEmpty(item.ds.C_mat))
                {
                    typeB = "Partner";
                }
                if (!String.IsNullOrEmpty(item.ds.C_id_parent))
                {
                    typeB = "Children";
                }
                if (typeB.Equals("Employee"))
                {
                    company = db.t_succursales.Where(s => s.C_id.Equals(item.ds.C_id_succ)).FirstOrDefault().C_name;
                    lemployee.Add(item.vouchers.C_id_bon.ToString());
                }
                if (typeB.Equals("Partner"))
                {
                    lspouse.Add(item.vouchers.C_id_bon.ToString());
                }
                if (typeB.Equals("Children"))
                {
                    lchildren.Add(item.vouchers.C_id_bon.ToString());

                }
                if (typeB.Equals("Visitor"))
                {
                    lvisitor.Add(item.vouchers.C_id_bon.ToString());
                }
                // lDict.Add(dictionary);
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Object> lstCategory = new List<Object>()
            {
                "Employees",
                "Spouses",
                "Children",
                "Casual",
                "Contractor",
                "Visitor"
            };
            lcasual.Add
                (
                    (
                        db.t_vouchers_casuals.Where(e=>e.C_date_casual.EndsWith("/2018")) == null ? "0" : db.t_vouchers_casuals.Where(e => e.C_date_casual.EndsWith("/2018")).ToList().Count.ToString()
                    )
                );

            lcontractor.Add
                (
                    (
                        db.t_vouchers_contractor.Where(e => e.C_datedeb.EndsWith("/2018")) == null ? "0" : db.t_vouchers_contractor.Where(e => e.C_datedeb.EndsWith("/2018")).ToList().Count.ToString()
                    )
                );

            List<Object> dataVouchers = new List<Object>()
            {
                lemployee.Count,
                lspouse.Count,
                lchildren.Count,
                lcasual.Count,
                lcontractor.Count,
                lvisitor.Count
            };
            List<List<Object>> DataFullGet = new List<List<object>>()
            {
                lstCategory,
                dataVouchers
            };
            return js.Serialize(DataFullGet);
        }
        
        
    }
}