using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanroWebApp.Models;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Text;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using System.Data;
using System.Globalization;
using System.Threading;

namespace BanroWebApp.Controllers
{
   [Authorize]
    public class HomeController : Controller
    {
        BANROEntities dbContext = new BANROEntities();
        public void LoadSuccursale()
        {
            ViewData["departements"] = new SelectList(dbContext.t_succursales.ToList(), "C_id", "C_name");
            t_departement succ = new t_departement();
        }

        public ActionResult ViewAge()
        {
            return View();
        }
        public void LoadMonthYear()
        { 

           
            List<String> lstDateFilter = new List<string>();
            List<String> lstYear = new List<string>();
            //var QueryMonth = from BCmd in dbContext.t_bon_commandes
            //                 join TFacture in dbContext.t_factures on BCmd.C_id_bon equals TFacture.C_id_bon
            //                 select BCmd.C_datedeb;

            //lstDateFilter = QueryMonth.Distinct().ToList();
            //foreach (var item in lstDateFilter)
            //{
            //    String currentYear = item.Split('/')[2];
            //    lstYear.Add(currentYear);
            //}
            for (int i = 2015; i <=DateTime.Now.Year; i++)
            {
                lstYear.Add(i.ToString());
            }
            lstYear = lstYear.Distinct().ToList();
            ViewData["MonthList"] = new SelectList(new MonthList().getMonths(), "id", "name");
            ViewData["YearList"] = new SelectList(lstYear);


        }
        public ActionResult TesterPage()
        {
            return View();
        }

        [HttpPost]
        public Object TesterPage(Models.t_test test)
        {
            dbContext.t_test.Add(test);
            dbContext.SaveChanges();
            return String.Format("ID :{0}", test.id);
        }
        public void CategoryMVI()
        {
            List<String> lst = new List<string>();
            lst.Add("Monthly");
            lst.Add("Weekly");
            lst.Add("Daily");
            lst.Add("Choose");
            ViewData["CategoryMVI"] = new SelectList(lst);
        }
        public void LoadCategory()
        {
            List<Models.Category> lstCateg = new List<Models.Category>{
                new Models.Category{
                    Id="Emp",
                    Label="Employe"
                },

                new Models.Category{
                    Id="Enf",
                    Label="Enfant"
                },
                new Models.Category{
                    Id="Part",
                    Label="Partenaire"
                },
                new Models.Category{
                    Id="Vis",
                    Label="Visiteur"
                }
            };
            ViewData["listCateg"] = new SelectList(dbContext.t_categories.ToList(), "C_id_categ", "C_id_categ");
        }
        public void LoadGenre()
        {
            List<Genre> lstGenre = new List<Genre>
            {
                new Genre{
                    ID="none",
                    Name="--Select Gender--"
                },
                new Genre{
                    ID="M",
                    Name="Male"
                },
                new Genre{
                    ID="F",
                    Name="Female"
                }
            };
            ViewData["Sex"] = new SelectList(lstGenre, "ID", "Name");
        }
       [Route("logout")]
        public void logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/");

            }
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/");
        }
        
       [AllowAnonymous]
       [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated && Session["userinfo"]!=null)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return Redirect("~/Home/Dashboard");
            }
            return View();
        }

       [AllowAnonymous]
        [HttpPost]
     
        public ActionResult Index(Models.t_logger logger,string returnUrl)
        {
            if (ModelState.IsValid)
            {

                logger.password = HomeController.CryptoMD5(logger.password);

                var Query = (from log in dbContext.t_logger
                        join employee in dbContext.t_beneficiaires on log.C_employeeId equals employee.C_id
                        join company in dbContext.t_succursales on log.C_idSucc equals company.C_id
                        where log.C_username.Equals(logger.C_username) && log.password.Equals(logger.password) && log.C_status_system.Equals("1")
                             select new {log,company,employee }).FirstOrDefault();
                if (Query != null)
                {
                    FormsAuthentication.SetAuthCookie(Query.employee.C_name, false);
                    Authenticate auth = new Authenticate
                    {
                        id = Query.log.C_id,
                        username = Query.employee.C_name,
                        password = Query.log.password,
                        Succursale = Query.log.C_idSucc,
                        Priority = Query.log.C_priority.ToLower(),
                        nameSuccursale = Query.company.C_name,
                        idEmpolyee = Query.employee.C_id
                        

                    };


                   
                    Session.Add("userinfo", auth);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        if (returnUrl.Contains("logout"))
                        {
                            return Redirect("~/Home/Dashboard");
                        }
                        return Redirect(returnUrl);
                    }
                    if (auth.Priority.ToLower().Equals("administrator"))
                    {
                        string Old1 = (DateTime.Now.Year - 18).ToString();
                        string Old2 = (DateTime.Now.Year - 25).ToString();
                        BANROEntities ctx = new BANROEntities();
                        string Month = (DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());
                        string Day = (DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                        string DayMonth = Day + "/" + Month + "/";
                        var QueryOld = from ds in ctx.t_beneficiaires
                                       where !ds.C_id_parent.Equals(null) && (ds.C_datenais.EndsWith(Old1) || ds.C_datenais.EndsWith(Old2)) && ds.C_datenais.StartsWith(DayMonth)
                                       select ds;

                        foreach (var item in QueryOld)
                        {
                            if (!item.Equals("active"))
                            {
                                item.C_statusChild = "inactive";

                            }

                        }
                        ctx.SaveChanges();

                        var QueryOld2 = from ds in ctx.t_beneficiaires
                                       where !ds.C_id_parent.Equals(null) && ds.C_datenais.EndsWith(Old2) && ds.C_datenais.StartsWith(DayMonth)
                                       select ds;

                        foreach (var item in QueryOld)
                        {
                            if (!item.Equals("active"))
                            {
                                item.C_statusChild = "inactive";

                            }

                        }
                        ctx.SaveChanges();

                    }
                    else
                    { 
                        string Old1 = (DateTime.Now.Year - 18).ToString();
                        string Old2 = (DateTime.Now.Year - 25).ToString();

                        string Month = (DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());
                        string Day = (DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                        string DayMonth = Day + "/" + Month + "/";
                        var QueryOld = from ds in dbContext.t_beneficiaires
                                       where !ds.C_id_parent.Equals(null) && (ds.C_datenais.EndsWith(Old1) || ds.C_datenais.EndsWith(Old2)) && ds.C_datenais.StartsWith(DayMonth)
                                       select ds;
                        if (QueryOld.ToList().Count>0)
                        {
                            var QueryMonth = from ds in QueryOld
                                             where ds.C_datenais.StartsWith(DayMonth)
                                             select ds;
                            if (QueryMonth.ToList().Count>0)
                            {
                                foreach (var item in QueryMonth)
                                {
                                    int id = int.Parse(item.C_id_parent);
                                    var QueyEmployee = from ds in dbContext.t_beneficiaires
                                                       where ds.C_id.Equals(id) && ds.C_id_succ.Equals(auth.Succursale)
                                                       select ds;
                                    if (QueyEmployee.ToList().Count > 0)
                                    {
                                        item.C_statusChild = "inactive";
                                    }

                                }
                                dbContext.SaveChanges();        
                            }
                        }
                        
                    }
                  
                    T_logs logs = new T_logs()
                    {
                        C_user = auth.username,
                        C_company = auth.nameSuccursale,
                        C_action = "Logged",
                        C_date = DateTime.Now.ToShortDateString(),
                        C_time = DateTime.Now.ToShortTimeString(),
                        C_object = "Login User",
                        C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                    };
                    dbContext.T_logs.Add(logs);
                    dbContext.SaveChanges();
                    return Redirect("~/Home/Dashboard");
                }
                else
                {
                    ModelState.AddModelError("username", "Authentification incorrect");
                    ViewBag.errorMessage = "404";
                }
            }

            //View();

            return View();
        }
       private String HashablePassWord(String pwd)
       {
          
           using (System.Security.Cryptography.MD5CryptoServiceProvider md5=new System.Security.Cryptography.MD5CryptoServiceProvider())
           {
               StringBuilder hash=new StringBuilder();
               byte[] bt = md5.ComputeHash(new System.Text.UTF8Encoding().GetBytes(pwd));
               for (int i = 0; i < bt.Length; i++)
               {
                   hash.Append(bt[i].ToString("x2"));
               }
               return hash.ToString();
           }
       }
       private void CounterDatas(String priority,String Succursale)
       {
           var countSuccursal = from succursale in dbContext.t_succursales
                                select succursale;
           var countDepartments = from departement in dbContext.t_departement
                                  select departement;

           var countEmployed = from employed in dbContext.t_beneficiaires
                               where !employed.C_id_succ.Equals(null)
                               select employed;

           var countHealthCenter = from healthCenter in dbContext.t_centre_soins
                                   select healthCenter;

           var countPartnersAndChilds = from PartnersChilds in dbContext.t_beneficiaires
                                        where PartnersChilds.C_id_succ.Equals(null) && (!PartnersChilds.C_id_parent.Equals(null) || !PartnersChilds.C_id_partenaire.Equals(null))
                                        select PartnersChilds;
           switch (priority)
           {
               case "user":
                   if (countDepartments!=null)
                   {
                       countDepartments = countDepartments.Where(x => x.C_id_succ.Equals(Succursale));
                       Session.Add("count_depart", countDepartments.ToList().Count);
                   }
                   break;
               case "administrator":
                   if (countSuccursal!=null)
                   {
                       Session.Add("count_succ", countSuccursal.ToList().Count);

                   }
                   else
                   {
                       Session.Add("count_succ", 0);
                   }
                   if (countDepartments!=null)
                   {
                       Session.Add("count_depart", countDepartments.ToList().Count);
                   }
                   else
                   {
                       Session.Add("count_depart", 0);

                   }
                   if (countEmployed!=null)
                   {
                       Session.Add("count_succ", countEmployed.ToList().Count);

                   }
                   else
                   {
                       Session.Add("count_succ", 0);
                   }
                   if (countHealthCenter!=null)
                   {
                       Session.Add("count_healthCenter", countHealthCenter.ToList().Count);
                   }
                   else
                   {
                       Session.Add("count_healthCenter", 0);
                   }
                   if (countPartnersAndChilds!=null)
                   {
                       Session.Add("countPartnerChilds", countPartnersAndChilds.ToList().Count);
                   }
                   else
                   {
                       Session.Add("countPartnerChilds", 0);
                   }
                   break;
               default:
                   break;
           }
       }

        public ActionResult Dashboard()
        {
           // ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Welcome()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult AddSuccursale()
        {
            
            return View();
        }

        public ActionResult ViewEmployed()
        {
            this.LoadGenre();
            return View();
        }
        //public ActionResult SearchSuccursale()
        //{
        //    return View();
        //}

        public ActionResult ViewChildren()
        {
            this.LoadGenre();
            return View();
        }

        public ActionResult ViewPartner()
        {
            this.LoadGenre();
            return View();
        }
        public ActionResult ViewVisitor()
        {
            this.LoadGenre();
            return View();
        }
        public void SuccuSearch( Models.t_succursales succ)
        {
            if (succ != null)
            {
                var db = (from ds in dbContext.t_succursales
                          where ds.C_id.Equals(succ.C_id)
                          select ds).FirstOrDefault();

                if (db != null)
                {
                    ViewBag.donnees = "OK";
                    ViewBag.Name_Succ = db.C_name;
                    ViewBag.Adresse_Succ = db.C_adresse;
                    ViewBag.Phone_Succ = db.C_phone;
                    ViewBag.Id = db.C_id;
                }
                else
                {
                    ViewBag.donnees = "NO";
                }
                if (!String.IsNullOrEmpty(succ.C_name))
                {
                    //db.C_name = succ.C_name;
                    //db.C_adresse = succ.C_adresse;
                    //db.C_phone = succ.C_phone;
                    //dbContext.SaveChanges();
                    
                }

            }
        }
       [HttpPost]
        public ActionResult SearchSuccursale(t_succursales company)
        {
            var db = from ds in dbContext.t_succursales
                     where ds.C_id.Equals(company.C_id)
                     select ds;
            if (db!=null)
            {
                foreach (var item in db)
                {
                    item.C_id = company.C_id;
                    item.C_name = company.C_name;
                    item.C_adresse = company.C_adresse;
                    item.C_phone = company.C_phone;
                    
                }
                dbContext.SaveChanges();
                ViewBag.message = "Data saved";
            }
            return View();
        }
        public ActionResult SearchSuccursale()
        {
            if (!String.IsNullOrEmpty(Request.Params["namesucc"]))
            {
                String nameSucc = Request.Params["namesucc"].ToString();
                var db = (from ds in dbContext.t_succursales
                          where ds.C_name.StartsWith(nameSucc)
                          select ds).FirstOrDefault();

                if (db != null)
                {
                    ViewBag.donnees = "OK";
                    ViewBag.Id_Succ = db.C_id;
                    ViewBag.Name_Succ = db.C_name;
                    ViewBag.Adresse_Succ = db.C_adresse;
                    ViewBag.Phone_Succ = db.C_phone;
                    ViewBag.Id = db.C_id;

                }

            }
            else
            {
                var db = from ds in dbContext.t_succursales
                         select new { ID = ds.C_id, NOM = ds.C_name, PHONE = ds.C_phone, ADRESSE = ds.C_adresse };

                JavaScriptSerializer serial = new JavaScriptSerializer();
                ViewBag.ListSuccurales = db.ToList();

            }
       
            return View();
        }
        
        public ActionResult AddDepartement()
        {

            this.LoadSuccursale();
            return View();
        }
        public ActionResult AddBeneficiaire()
        {
            this.LoadSuccursale();
            this.LoadGenre();
            this.LoadCategory();
            return View();
        }
        public Object SearchDepartement(Object dep)
        {
            if (!String.IsNullOrEmpty(Request.Params["idsucc"]))
            {
                String id = Request.Params["idsucc"].ToString();
                var db = from ds in dbContext.t_departement
                          where ds.t_succursales.C_name.StartsWith(id)
                         select ds ;

                if (db != null)
                {
                    ViewBag.id_succ = db.FirstOrDefault().C_id_succ;
                    ViewData["search_depart"] = new SelectList(db.ToList(), "C_id", "C_id_depart");

                }

            }
            return View();
        }
        public ActionResult ViewDepartments()
        {
            return View();
        }

        public ActionResult ViewFacilities()
        {
            return View();
        }

        public ActionResult ViewEmployeeList()
        {
            return View();

        }
        public ActionResult AddNewEmployee()
        {
            return View();
        }
        public ActionResult ViewVouchers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ViewVouchers(t_factures invoice)
        {
            invoice.C_datefacture = String.Format("{0}/{1}/{2}", DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString());
            invoice.C_timefacture = DateTime.Now.TimeOfDay.ToString();
            invoice.C_fileupload = invoice.C_id_bon.ToString();
            dbContext.t_factures.Add(invoice);
            dbContext.SaveChanges();
            return RedirectToAction("ViewVouchers","Home");
        }
        /* Methodes POST 
        [HttpPost]
        public ActionResult AddSuccursale(Models.t_succursales succ)
        {

            dbContext.t_succursales.Add(succ);
            dbContext.SaveChanges();
            return View();
        }
         * */

        [HttpPost]
        public ActionResult AddSuccursale(Models.t_succursales succ)
        {
            Authenticate auth = (Authenticate)Session["userinfo"];
            //JavaScriptSerializer serial=new JavaScriptSerializer();
            //t_succursales succ = serial.Deserialize<t_succursales>(data.ToString());
            t_succursales query = (from ds in dbContext.t_succursales
                                   where ds.C_name.Equals(succ.C_name) || ds.C_company.Equals(succ.C_company)
                                   select ds).FirstOrDefault();

            if (query==null)
            {
                succ.C_id = DateTime.Now.Millisecond.ToString();

                dbContext.t_succursales.Add(succ);
                dbContext.SaveChanges();
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_action = "Add new",
                    C_object = "Subsidiary",
                    C_company = auth.nameSuccursale,
                    C_mat = dbContext.t_beneficiaires.Where(id => id.C_id == auth.idEmpolyee).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();

                ViewBag.result = "Saved";
                return Redirect("SearchSuccursale");
            }
            else
            {
                ViewBag.result = "Error";
            }
           
            return View();
        }

        [HttpPost]
        public ActionResult AddDepartement(Models.t_departement depart)
        {
            this.LoadSuccursale();
            //depart.C_id_succ = depart.t_succursales.C_name;
            //t_departement dep = new t_departement
            //{
            //    C_id_depart = depart.C_id_depart,
            //    C_id_succ = depart.t_succursales.C_name
            //};
            dbContext.t_departement.Add(depart);
            dbContext.SaveChanges();
            Authenticate auth = (Authenticate)Session["userinfo"];
            T_logs logs = new T_logs()
            {
                C_user = auth.username,
                C_date = DateTime.Now.ToShortDateString(),
                C_time = DateTime.Now.ToShortTimeString(),
                C_action = "Add new",
                C_object = "Department",
                C_company = auth.nameSuccursale,
                C_mat = dbContext.t_beneficiaires.Where(id => id.C_id == auth.idEmpolyee).FirstOrDefault().C_mat

            };
            dbContext.T_logs.Add(logs);
            dbContext.SaveChanges();
            ViewBag.resultDepart="Saved";
            return View();
        }

        [HttpPost]
        public ActionResult AddBeneficiaire(Models.t_beneficiaires beneficiaire)
        {
            dbContext.t_beneficiaires.Add(beneficiaire);
            dbContext.SaveChanges();
            ViewBag.resultBene = "Saved";
            return View();
        }

       
      
        // AJAX CODES
       
        [Route("ListDepartement/{code_succ}")]
        public String ListDepartement(string code_succ)
        {
            
            var listDepart = from ds in dbContext.t_departement
                             where ds.C_id_succ.Equals(code_succ)
                             select new { code_Succ = ds.C_id_succ,id_depart=ds.C_id ,code_depart = ds.C_id_depart };
            
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var result = serial.Serialize(listDepart);
            ViewData["listDep"] = new SelectList(listDepart.ToList(), "code_depart", "code_depart");
            return result.ToString();
        }

       [HttpPost]
        public String UpdateCompany()
        {
            return "200";
        }

        [HttpPost]
        public String UpdateSuccursale()
        {
            
                JavaScriptSerializer ser = new JavaScriptSerializer();
            var data = "";
            using (System.IO.StreamReader red=new System.IO.StreamReader(Request.InputStream))
            {
                data = red.ReadToEnd();
            }
            
            Models.t_succursales donnee = ser.Deserialize<Models.t_succursales>(data);
            var sql = (from ds in dbContext.t_succursales
                      where ds.C_id.Equals(donnee.C_id)
                      select ds).FirstOrDefault();
            if (sql!=null)
            {
                sql.C_name = donnee.C_name;
                sql.C_adresse = donnee.C_adresse;
                sql.C_phone = donnee.C_phone;
                sql.C_company = donnee.C_company;
                dbContext.SaveChanges(); 
            }
            Authenticate auth = (Authenticate)Session["userinfo"];
            T_logs logs = new T_logs()
            {
                C_user = auth.username,
                C_company = auth.nameSuccursale,
                C_action = "Update Company",
                C_date = DateTime.Now.ToShortDateString(),
                C_time = DateTime.Now.ToShortTimeString(),
                C_object = "Subsidiary",
                C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

            };
            dbContext.T_logs.Add(logs);
            dbContext.SaveChanges();

            return "200";
        }
        [HttpPost]
        public String UpdateDepartement()
        {
            var data = "";
            using (System.IO.StreamReader Reader=new System.IO.StreamReader(Request.InputStream))
            {
                data=Reader.ReadToEnd();
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Models.t_departement departement =ser.Deserialize<Models.t_departement>(data);
            var db = (from ds in dbContext.t_departement
                      where ds.C_id.Equals(departement.C_id)
                      select ds).FirstOrDefault();

            if (db!=null)
            {
                db.C_id_depart = departement.C_id_depart;
                dbContext.SaveChanges();
            }
            Authenticate auth = (Authenticate)Session["userinfo"];
            T_logs logs = new T_logs()
            {
                C_user = auth.username,
                C_company = auth.nameSuccursale,
                C_action = "Update Depatement",
                C_date = DateTime.Now.ToShortDateString(),
                C_time = DateTime.Now.ToShortTimeString(),
                C_object = "Department",
                C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.id)).FirstOrDefault().C_mat

            };
            dbContext.T_logs.Add(logs);
            dbContext.SaveChanges();

            return "200";
        }
        [HttpPost]
        public String AddEmployed()
        {
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var data = "";
            using (System.IO.StreamReader strm = new System.IO.StreamReader(Request.InputStream))
            {
                data = strm.ReadToEnd();
            }
            //var base64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAQQAAAC0CAYAAABytVLLAAAK4klEQVR4Xu2bB4sVyxZGyyxizmLAhAFzzvrbzVkUc44ooo45p3e/hjr0OzOjnzr3eR7fKrhczsw+3bXX7loVehzW19f3vdAgAAEI/ENgGELgOYAABCoBhMCzAAEIdAggBB4GCEAAIfAMQAAC/QmwQuCpgAAEWCHwDEAAAqwQeAYgAIEfEGDLwOMBAQiwZeAZgAAE2DLwDEAAAmwZeAYgAAGHAGcIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAh9Fih37x5Uy5cuFA+fvxYRowYUSZPnlyWLl1axo4d2+np58+fy9WrV8uzZ8/Kt2/fyvjx48uyZcvKpEmTOjHv378v58+fL/r/9+/fy8SJE8vq1avL6NGjfyvj+/fvl7t375bZs2c3/Wm3V69elevXrzf3UtO91J92n4e6P7+VBF/6KQGE8FNE/7uA169fl+PHjw94w+3btzcD/+vXr+XgwYPly5cv/eI2b97cCESD7+jRo40s2m3kyJFl165dZdSoUb+UlK534sSJIhHNnDmzrF27tvP9vr6+cubMmX7XGzZsWNm9e3cZM2bMkPfnlzpP8C8RQAi/hOvfDT558mR5+fJlcxMNbA3At2/fNp81+2/ZsqXcvHmz3L59u/nZtGnTigb548ePm88zZswo69atK5cuXSoPHz7s/EzyeP78efN5yZIlZdGiRVYi9+7da+6lftQ2a9assmbNms7ndp8lC8XWe82dO7esXLlyyPpjdZqgPyKAEP4I39B9Wcv6I0eONLOpVgJaEehnhw4darYPWuprxtVMrW2FPu/Zs6doJpYAJJIpU6aU5cuXd74zbty4smPHjmalcODAgWZ1oeX8qlWryuXLlxuZDB8+vNlK6DraqkgeitOgl3yqWAYTglYikpbuvWnTpqbP+/fvb64jQeg6NYfB+rN169ahA8mV/ogAQvgjfEP3ZQ1CDfZPnz6V+fPnl8WLFzcX1/agCkGS0ODSANdqYOrUqUXbjAkTJpR58+Y1g1rfrzFz5sxpBr/asWPHOiLRtkH3qqsPSURikCTU6uDW7yUa/U5nFpr9B1shaBsi+ej+urb6qDy0GvlZfyQ63YP29wkghL9fg0F7oK2ADgbVNLNv3Lixmem7zwb0e8lAwtCevca0hXDlypXy4MGDzkpDA1crku5raWDu27evOdBstyqUbiFotaLfdTf1R9dRc/qDEHrjQUQIvVGHfr3QjKyT/dq09NaZQVsI+qyVxYsXL5owbTV0sFhjVqxY0awc1G7cuFHu3LnTiKMe9mk7oO1Gu23YsKG5T3cbTAi3bt0q+m+gpsNHrWLc/vRoKaK6hRB6rNwfPnxo3jS0D/LWr19fpk+f3uzL6+Bqz9RaouvVn84EtGw/fPhwM/O33wicO3euPHnypHkVqC2DxKCmVcK7d+86QtEqY6A2kBDa/dH5gM4Q1G8dNEpUOufYuXNnp89Of3qsHHHdQQg9VHINJg3m+kpRe3nN2HU53R6A+luAhQsXNr2/du1a0RuBum2QUCSE9pahHv7Vw0ld8+nTp+Xs2bP/RaC+unRWCFqZnDp1qglt90evIfU6slsIP+tPD5UitisIoYdKXweSuqQ9vLYJmmnVNNg1w9bVgM4Ktm3b1gx8zd6SRZ39NUNrxaDvaDVQD/p0nfpqUvE6sKzXrxh0OKi3F917+oFWCFrNSGB6s6Dtit4W6F51hSMh7N27t9PnH/Wnh8oQ3RWE0CPlH2yA1u5JEDqk08zbPavXGP114IIFC8qjR4/KxYsXB8ysnhG05aPv6XCwvmJsz+T1IgMJQSLQ4Nd3B2p6y6A3GE5/eqQM8d1ACD3yCLS3AwN1qb3319sCvTVotzr46s8GOuyrh4ztpb6uq32+7l9fD+oaWn3odWa3ELploW3O6dOn+0lBf5Sk+9Wzih/1p0dKQDe0Ev1nxvkOif8/AvrbBP2dgAacDvS0hehuWtLXf8ugJf3v/jsGh476oj5pq6F76YDzb/bH6TMx/QkgBJ4KCECgQwAh8DBAAAIIgWcAAhBgy8AzAAEI/IAAWwYeDwhAgC0DzwAEIMCWgWcAAhBgy8AzAAEIOAQ4Q3AoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHwH8AFmb1VN5FaGoAAAAASUVORK5CYII=";
            //base64 = base64.ToString().Substring((base64.ToString().IndexOf(",")+1));
            //byte[] img = Convert.FromBase64String(base64);
            //System.IO.MemoryStream sMemory = new System.IO.MemoryStream(img, 0, img.Length);
            //System.Drawing.Image image = System.Drawing.Image.FromStream(sMemory);
            //image.Save(Server.MapPath("~/Images/Lama22.jpg"));
            serial.MaxJsonLength = Int32.MaxValue;
            Employed employed = serial.Deserialize<Employed>(data);
            Models.t_beneficiaires bene = new t_beneficiaires()
            {
                C_name = employed.name,
                C_phone = employed.phone,
                C_id_succ=employed.ID_Succursale,
                C_id_depart=int.Parse(employed.ID_Departement),
                C_sex=employed.sex,
               C_picture=employed.getImageURL(employed.picture,Server.MapPath("~/Images")),
                C_datenais=employed.datenaiss,
                C_statusmaritalk=employed.CivilStatus,
                C_mat=employed.Matricule,
                C_status_system="1"
                
            };
            dbContext.t_beneficiaires.Add(bene);
            dbContext.SaveChanges();

            if (String.IsNullOrEmpty(employed.partner.ID_Departement))
            {
                employed.partner.ID_Departement = (0).ToString();
            }
            if (String.IsNullOrEmpty(employed.partner.ID_Succursale))
            {
                employed.partner.ID_Succursale = null;
            }
            if (!String.IsNullOrEmpty(employed.partner.name))
            {
                var Partner = new t_beneficiaires
                    {
                        C_name = employed.partner.name,
                        C_phone = employed.partner.phone,
                        C_id_succ = employed.partner.ID_Succursale,
                        C_id_depart = int.Parse(employed.partner.ID_Departement),
                        C_sex = employed.partner.sex,
                        C_picture = employed.partner.getImageURL(employed.partner.picture, Server.MapPath("~/Images")),
                        C_datenais = employed.datenaiss,
                        C_id_partenaire = bene.C_id.ToString(),
                        C_statusmaritalk="Married",
                        C_status_system="1"
                    };
                dbContext.t_beneficiaires.Add(Partner);
                dbContext.SaveChanges();

                var query = (from ds in dbContext.t_beneficiaires
                             where ds.C_id.Equals(bene.C_id)
                             select ds).FirstOrDefault();
                if (query != null)
                {
                    query.C_id_partenaire = Partner.C_id.ToString();
                    dbContext.SaveChanges();
                }
            }



            if (employed.Childs != null)
            {
                foreach (var item in employed.Childs)
                {
                    var children = new Models.t_beneficiaires
                    {
                        C_name = item.name,
                        C_sex = item.sex,
                        C_datenais = item.datenais,
                        C_id_parent = bene.C_id.ToString(),
                        C_picture = item.getImageURL(item.picture, Server.MapPath("~/Images")),
                        C_statusChild=item.status,
                        C_status_system="1"
                    };
                    dbContext.t_beneficiaires.Add(children);
                    dbContext.SaveChanges();
                }
            }
            return "200";
        }

        [HttpGet]
        
        public String getListSuccursales()
        {
            var data = String.Empty;
           
                var db = from ds in dbContext.t_succursales
                         select new { id = ds.C_id, name = ds.C_name, phone = ds.C_phone, address = ds.C_adresse,idCompany=ds.C_company };
                JavaScriptSerializer serial = new JavaScriptSerializer();
                data = serial.Serialize(db.ToList());
                //HttpContext.Cache["listSuccursales"] = data.ToString();
            
            

            return data.ToString();
        }
        [Route("getter/{data}")]
        public String Message(string data)
        {
            return String.Format("My data is :{0}", data);
        }
        [Route("Parents/{nameParent}")]
        public String getParentList(String nameParent)
        {
            var Query = from beneficiaire in dbContext.t_beneficiaires
                        join entreprise in dbContext.t_succursales
                        on beneficiaire.C_id_succ equals entreprise.C_id
                        join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                       where beneficiaire.C_name.Contains(nameParent)
                        select new { _name = beneficiaire.C_name, _phone = beneficiaire.C_phone,
                            _succursale=entreprise.C_name
                            ,_photo=beneficiaire.C_picture,_departement=departement.C_id_depart,_idparent=beneficiaire.C_id,_sex=beneficiaire.C_sex};
            
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var data= serial.Serialize(Query.ToList());
           return data.ToString();
        }
        [HttpPost]
        public String AddChildren()
        {
            using (System.IO.StreamReader sReader=new System.IO.StreamReader(Request.InputStream))
            {
                var data = sReader.ReadToEnd();
                JavaScriptSerializer serial = new JavaScriptSerializer();
                Models.Children child = serial.Deserialize<Models.Children>(data);
                var Query = from ds in dbContext.t_beneficiaires
                            where ds.C_name.Equals(child.name)
                            select ds;

                if (Query.ToList().Count>0)
                {
                    int idParent=int.Parse(Query.FirstOrDefault().C_id_parent);
                    var beneFound = from ds in dbContext.t_beneficiaires
                                    join succursale in dbContext.t_succursales on ds.C_id_succ equals succursale.C_id
                                    join departement in dbContext.t_departement on ds.C_id_depart equals departement.C_id
                                    where ds.C_id.Equals(idParent)
                                                        select new{
                                                            _name=ds.C_name,
                                                            _phone=ds.C_phone,
                                                            _sex=ds.C_sex,
                                                            _picture=ds.C_picture,
                                                            _succursale=succursale.C_name,
                                                            _departement=departement.C_id_depart
                                                        };
                    var _result=serial.Serialize(beneFound.FirstOrDefault());
                    return _result.ToString();
                }
                else
                {
                    Models.t_beneficiaires beneChild = new t_beneficiaires
                    {
                        C_name=child.name,
                        C_sex=child.sex,
                        C_picture = child.getImageURL(child.picture, Server.MapPath("~/Images")),
                        C_id_parent=child.parent,
                        C_datenais=child.datenais
                    };
                    dbContext.t_beneficiaires.Add(beneChild);
                    dbContext.SaveChanges();
                    return "200";

                }
            }
        }
        [HttpPost]
        public String UpdateChildren()
        {
            using (System.IO.StreamReader sReader=new System.IO.StreamReader(Request.InputStream))
            {
                JavaScriptSerializer serial = new JavaScriptSerializer();
                Models.Children child = serial.Deserialize<Models.Children>(sReader.ReadToEnd());
                var Query = (from ds in dbContext.t_beneficiaires
                             where ds.C_name.Equals(child.name)
                             select ds).FirstOrDefault();

                Query.C_id_parent = child.parent;
                dbContext.SaveChanges();
                Authenticate auth = (Authenticate)Session["userinfo"];
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_company = auth.nameSuccursale,
                    C_action = "Update Children",
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_object = "Beneficiairy",
                    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();

                return "200";
            }
        }

        [Route("conjointlist/{nameconjoint}")]
        public String getConjointList(String nameconjoint)
        {
            List<Models.Partner> ListPartner = new List<Partner>();
            var Query = from beneficiaire in dbContext.t_beneficiaires
                        join entreprise in dbContext.t_succursales
                        on beneficiaire.C_id_succ equals entreprise.C_id
                        join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                        where beneficiaire.C_name.Contains(nameconjoint) && beneficiaire.C_id_partenaire.Equals(null)
                        select new
                        {
                            beneficiaire=beneficiaire,
                            succursale=entreprise,
                            departement=departement
                     

                        };


            foreach (var item in Query)
            {
                List<Models.Children> listChilds = new List<Children>();
                string idConjoint=item.beneficiaire.C_id.ToString();
                var QueryChildren = from ds in dbContext.t_beneficiaires
                                    where ds.C_id_parent.Equals(idConjoint)
                                    select ds;

                foreach (var itemChild in QueryChildren)
                {
                    Models.Children child = new Children()
                    {
                        id=itemChild.C_id.ToString(),
                        name=itemChild.C_name,
                        sex=itemChild.C_sex,
                        datenais=itemChild.C_datenais,
                        picture=itemChild.C_picture,
                        parent=item.beneficiaire.C_id.ToString(),
                        status=itemChild.C_statusChild
                    };
                    listChilds.Add(child);
                    
                }

                Models.Partner conjoint = new Partner()
                {
                    id=item.beneficiaire.C_id.ToString(),
                    name=item.beneficiaire.C_name,
                    sex=item.beneficiaire.C_sex,
                    phone=item.beneficiaire.C_phone,
                    ID_Succursale=item.succursale.C_name,
                    ID_Departement=item.departement.C_id_depart,
                    picture=item.beneficiaire.C_picture,
                    CivilStatus=item.beneficiaire.C_statusmaritalk,
                    Childs=listChilds
                };
                ListPartner.Add(conjoint);
            }
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var data = serial.Serialize(ListPartner);
            return data.ToString();
        }

        [HttpPost]
        public String AddConjoint()
        {
            using (System.IO.StreamReader sReader = new System.IO.StreamReader(Request.InputStream))
            {
                var data = sReader.ReadToEnd();
                JavaScriptSerializer serial = new JavaScriptSerializer();
                Models.Conjoint partner = serial.Deserialize<Models.Conjoint>(data);
                if (String.IsNullOrEmpty(partner.idDepartement))
                {
                    partner.idDepartement = (0).ToString();
                }
                if (String.IsNullOrEmpty(partner.idSuccursale))
                {
                    partner.idSuccursale = null;
                }
                Models.t_beneficiaires bene = new t_beneficiaires
                {
                    C_name = partner.name,
                    C_sex = partner.sex,
                    C_phone = partner.phone,
                    C_picture = partner.getImageURL(partner.picture, Server.MapPath("~/Images/")),
                    C_datenais = partner.datenais,
                    C_id_partenaire = partner.conjoint,
                    C_id_succ = partner.idSuccursale,
                    C_id_depart = int.Parse(partner.idDepartement)


                };
                dbContext.t_beneficiaires.Add(bene);
                dbContext.SaveChanges();
                int idEmployed = int.Parse(bene.C_id_partenaire);
                var Query = (from ds in dbContext.t_beneficiaires
                             where ds.C_id.Equals(idEmployed)
                             select ds).FirstOrDefault();

                Query.C_id_partenaire = bene.C_id.ToString();
                dbContext.SaveChanges();
                return "200";
            }
        }
        public String getListDepartments()
        {
            var Query = from ds in dbContext.t_departement
                        select new { id=ds.C_id,name=ds.C_id_depart,idSucc=ds.C_id_succ };
            JavaScriptSerializer serial = new JavaScriptSerializer();

            return serial.Serialize(Query.ToList());
            
        }

        [HttpPost]
        public String AddVisitor()
        {
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                using (System.IO.StreamReader sReader = new System.IO.StreamReader(Request.InputStream))
                {
                    var data = sReader.ReadToEnd();
                    JavaScriptSerializer serial = new JavaScriptSerializer();
                    Models.Visitor visiteur = serial.Deserialize<Models.Visitor>(data);
                    Models.t_beneficiaires bene = new t_beneficiaires
                    {
                        C_name = visiteur.name,
                        C_sex = visiteur.sex,
                        C_phone = visiteur.Phone,
                        C_picture = visiteur.getImageURL(visiteur.picture, Server.MapPath("~/Images/")),
                        C_id_visitor = visiteur.idVisitor,
                        C_motif_visit = visiteur.Cause,
                        C_company_visitor = visiteur.CompanyVisitor,
                        C_company_visited=auth.Succursale,
                        C_status_system="1"
                    };
                    dbContext.t_beneficiaires.Add(bene);
                    dbContext.SaveChanges();
                }
            
                
            }
            return "200";
        }
        [HttpPost]
        public String UpdateEmployed()
        {
            using (System.IO.StreamReader sReader=new System.IO.StreamReader(Request.InputStream))
            {
                Employed employed = new Employed();
                JavaScriptSerializer serial = new JavaScriptSerializer();
                employed= serial.Deserialize<Employed>(sReader.ReadToEnd());
                int idEmployed = int.Parse(employed.id);
                var QueryEmployed = (from ds in dbContext.t_beneficiaires
                                     where ds.C_id.Equals(idEmployed)
                                     select ds).FirstOrDefault();


                
                if (employed.Childs.Count > 0)
                {
                    foreach (var child in employed.Childs)
                    {
                        if (String.IsNullOrEmpty(child.id))
                        {
                            Models.t_beneficiaires beneChild = new t_beneficiaires()
                            {
                                C_name=child.name,
                                C_sex=child.sex,
                                C_datenais=child.datenais,
                                C_picture = child.getImageURL(child.picture, Server.MapPath("~/Images")),
                                C_id_parent=employed.id,
                                C_statusChild=child.status,
                                C_status_system="1"
                            };
                            dbContext.t_beneficiaires.Add(beneChild);
                            dbContext.SaveChanges();
                            Authenticate auth = (Authenticate)Session["userinfo"];
                            T_logs logs = new T_logs()
                            {
                                C_user = auth.username,
                                C_company = auth.nameSuccursale,
                                C_action = "Add New Children",
                                C_date = DateTime.Now.ToShortDateString(),
                                C_time = DateTime.Now.ToShortTimeString(),
                                C_object = "Beneficiairy",
                                C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                            };
                            dbContext.T_logs.Add(logs);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            int idChild = int.Parse(child.id);
                            var QueryChild = (from ds in dbContext.t_beneficiaires
                                              where ds.C_id.Equals(idChild)
                                              select ds).FirstOrDefault();

                            QueryChild.C_name = child.name;
                            QueryChild.C_sex = child.sex;
                            QueryChild.C_datenais = child.datenais;
                            QueryChild.C_id_parent = child.parent;
                            QueryChild.C_statusChild = child.status;
                            QueryChild.C_status_system = child.account_system;
                            if (child.picture.Contains(','))
                            {
                                QueryChild.C_picture = child.getImageURL(child.picture, Server.MapPath("~/Images"));
                            }
                            else
                            {
                                QueryChild.C_picture = child.picture;
                            }
                            dbContext.SaveChanges();
                            Authenticate auth = (Authenticate)Session["userinfo"];
                            T_logs logs = new T_logs()
                            {
                                C_user = auth.username,
                                C_company = auth.nameSuccursale,
                                C_action = "Update Children",
                                C_date = DateTime.Now.ToShortDateString(),
                                C_time = DateTime.Now.ToShortTimeString(),
                                C_object = "Beneficiairy",
                                C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                            };
                            dbContext.T_logs.Add(logs);
                            dbContext.SaveChanges();
                        }
                        
                    }
                }
                else
                {
                    if (employed.partner.Childs!=null)
                    {
                        foreach (var itemChild in employed.partner.Childs)
                        {
                            int idChild = int.Parse(itemChild.id);
                            var QueryChild = (from ds in dbContext.t_beneficiaires
                                              where ds.C_id.Equals(idChild)
                                              select ds).FirstOrDefault();

                            QueryChild.C_name = itemChild.name;
                            QueryChild.C_sex = itemChild.sex;
                            QueryChild.C_datenais = itemChild.datenais;
                            QueryChild.C_id_parent = itemChild.parent;
                            QueryChild.C_statusChild = itemChild.status;
                            QueryChild.C_status_system = itemChild.account_system;
                            if (itemChild.picture.Contains(','))
                            {
                                QueryChild.C_picture = itemChild.getImageURL(itemChild.picture, Server.MapPath("~/Images"));
                            }
                            else
                            {
                                QueryChild.C_picture = itemChild.picture;
                            }
                            dbContext.SaveChanges();
                            Authenticate auth = (Authenticate)Session["userinfo"];
                            T_logs logs = new T_logs()
                            {
                                C_user = auth.username,
                                C_company = auth.nameSuccursale,
                                C_action = "Update Children",
                                C_date = DateTime.Now.ToShortDateString(),
                                C_time = DateTime.Now.ToShortTimeString(),
                                C_object = "Beneficiairy",
                                C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                            };
                            dbContext.T_logs.Add(logs);
                            dbContext.SaveChanges();
                        }
                    }
                }
                Models.Partner partner = null;
                Models.t_beneficiaires beneConjoint=null;
                if (!String.IsNullOrEmpty(employed.partner.name))
                {
                    partner = employed.partner;
                    if (string.IsNullOrEmpty(partner.id))
                    {
                        beneConjoint = new t_beneficiaires()
                        {
                            C_name = partner.name,
                            C_mat = partner.Matricule,
                            C_sex = partner.sex,
                            C_phone = partner.phone,
                            C_datenais = partner.datenaiss,
                            C_id_partenaire = employed.id,
                            C_statusmaritalk = "married",
                            C_picture = partner.getImageURL(partner.picture, Server.MapPath("~/Images")),
                            C_status_system = "1",
                        };
                        if (!String.IsNullOrEmpty(partner.ID_Succursale))
                        {
                            beneConjoint.C_id_succ = partner.ID_Succursale;
                        }
                        if (!String.IsNullOrEmpty(partner.ID_Departement))
                        {
                            beneConjoint.C_id_depart = int.Parse(partner.ID_Departement);
                        }
                        dbContext.t_beneficiaires.Add(beneConjoint);
                        dbContext.SaveChanges();
                        Authenticate auth = (Authenticate)Session["userinfo"];
                        T_logs logs = new T_logs()
                        {
                            C_user = auth.username,
                            C_company = auth.nameSuccursale,
                            C_action = "Add New Partner",
                            C_date = DateTime.Now.ToShortDateString(),
                            C_time = DateTime.Now.ToShortTimeString(),
                            C_object = "Beneficiairy",
                            C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                        };
                        dbContext.T_logs.Add(logs);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        partner = employed.partner;
                        int idPartner = int.Parse(partner.id);
                        var QueryPartner = (from ds in dbContext.t_beneficiaires
                                            where ds.C_id.Equals(idPartner)
                                            select ds).FirstOrDefault();

                        QueryPartner.C_name = partner.name;
                        QueryPartner.C_sex = partner.sex;
                        QueryPartner.C_phone = partner.phone;
                        QueryPartner.C_statusmaritalk = "married";
                        QueryPartner.C_id_partenaire = employed.id;
                        QueryPartner.C_status_system = partner.account_system;
                        QueryPartner.C_datenais = partner.datenaiss;
                        if (!String.IsNullOrEmpty(partner.Matricule))
                        {
                            QueryPartner.C_mat = partner.Matricule;
                        }
                        if (!String.IsNullOrEmpty(partner.datenaiss))
                        {
                            QueryPartner.C_datenais = partner.datenaiss;
                        }
                        
                        if (!String.IsNullOrEmpty(partner.ID_Succursale))
                        {
                            QueryPartner.C_id_succ = partner.ID_Succursale;
                                
                        }
                        if (!String.IsNullOrEmpty(partner.ID_Departement))
                        {
                            QueryPartner.C_id_depart = int.Parse(partner.ID_Departement);
                            
                        }
                        if (partner.picture.Contains(','))
                        {
                            QueryPartner.C_picture = partner.getImageURL(partner.picture, Server.MapPath("~/Images"));
                        }
                        else
                        {
                            QueryPartner.C_picture = partner.picture;
                        }
                        dbContext.SaveChanges();
                        Authenticate auth = (Authenticate)Session["userinfo"];
                        T_logs logs = new T_logs()
                        {
                            C_user = auth.username,
                            C_company = auth.nameSuccursale,
                            C_action = "Update Partner",
                            C_date = DateTime.Now.ToShortDateString(),
                            C_time = DateTime.Now.ToShortTimeString(),
                            C_object = "Beneficiairy",
                            C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                        };
                        dbContext.T_logs.Add(logs);
                        dbContext.SaveChanges();

                    }
                   
                }
               
                if (employed != null)
                {
                    if (!String.IsNullOrEmpty(employed.ID_Succursale))
                    {
                        QueryEmployed.C_id_succ = employed.ID_Succursale;

                    }
                    QueryEmployed.C_mat = employed.Matricule;
                    QueryEmployed.C_name = employed.name;
                    QueryEmployed.C_sex = employed.sex;
                    QueryEmployed.C_phone = employed.phone;
                    QueryEmployed.C_datenais = employed.datenaiss;
                //    QueryEmployed.C_id_succ = employed.ID_Succursale;
                    QueryEmployed.C_id_depart = int.Parse(employed.ID_Departement);
                    QueryEmployed.C_status_system = employed.account_system;
                    if (employed.picture.Contains(','))
                    {
                        QueryEmployed.C_picture = employed.getImageURL(employed.picture, Server.MapPath("~/Images"));

                    }

                    if (beneConjoint!=null)
                    {
                        QueryEmployed.C_id_partenaire = beneConjoint.C_id.ToString();
                        QueryEmployed.C_statusmaritalk = "married";
                    }
                    else
                    {
                        if (partner!=null)
                        {
                            QueryEmployed.C_id_partenaire = partner.id;

                        }
                        else
                        {
                            QueryEmployed.C_statusmaritalk = "single";
                        }
                    }
                    dbContext.SaveChanges();
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    T_logs logs = new T_logs()
                    {
                        C_user = auth.username,
                        C_company = auth.nameSuccursale,
                        C_action = "Update Employee",
                        C_date = DateTime.Now.ToShortDateString(),
                        C_time = DateTime.Now.ToShortTimeString(),
                        C_object = "Beneficiairy",
                        C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                    };
                    dbContext.T_logs.Add(logs);
                    dbContext.SaveChanges();
                    int idEm = int.Parse(employed.id);
                    if (employed.account_system.Equals("0"))
                    {
                        var QueryStatusDependent = from ds in dbContext.t_beneficiaires
                                                   where ds.C_id_partenaire.Equals(employed.id) || ds.C_id_parent.Equals(employed.id)
                                                   select ds;

                        foreach (var itemStatus in QueryStatusDependent)
                        {
                            itemStatus.C_status_system = employed.account_system;

                        }
                        dbContext.SaveChanges();
                    }
                    
                }
                return "200";
            }
        }
      
        public ActionResult HealthCenter()
        {
            return View();
        }

        public ActionResult SearchHealths()
        {
            return View();
        }

        public String getListHealthTab(String name)
        {
            List<CenterHelath> Center_Health = new List<CenterHelath>(); 
            var Query = from ds in dbContext.t_centre_soins
                        //where ds.C_status_system.Equals("1")
                        select ds;

            JavaScriptSerializer serial = new JavaScriptSerializer();
            foreach (var item in Query)
            {
                CenterHelath centerHealth = new CenterHelath
                {
                    C_id_centre = item.C_id_centre,
                    adresse = item.adresse,
                    C_name = item.C_name,
                    C_phone = item.C_phone,
                    C_status = (item.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                };
                Center_Health.Add(centerHealth);
            }

            return serial.Serialize(Center_Health);
        }

        public String getListHealth(String name)
        {
            List<CenterHelath> Center_Health = new List<CenterHelath>();
            var Query = from ds in dbContext.t_centre_soins
                        where ds.C_status_system.Equals("1")
                        select ds;

            JavaScriptSerializer serial = new JavaScriptSerializer();
            foreach (var item in Query)
            {
                CenterHelath centerHealth= new CenterHelath
                {
                   C_id_centre=item.C_id_centre,
                  adresse=item.adresse,
                   C_name=item.C_name,
                   C_phone=item.C_phone,
                   C_status=(item.C_status_system.Equals("1")?"Enabled":"Disabled")
                   
                };
                Center_Health.Add(centerHealth);
            }

            return serial.Serialize(Center_Health);
        }

        [HttpPost]
        public ActionResult HealthCenter(Models.t_centre_soins health_center)
        {
            String name=health_center.C_name;
            var Query = (from ds in dbContext.t_centre_soins
                         where ds.C_name.Equals(name)
                         select ds).FirstOrDefault();
            if (Query==null)
            {
                health_center.C_status_system = "1";
                dbContext.t_centre_soins.Add(health_center);
                dbContext.SaveChanges();
                JavaScriptSerializer serial = new JavaScriptSerializer();
                ViewBag.queryMessage = "200";
                Authenticate auth = (Authenticate)Session["userinfo"];
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_company = auth.nameSuccursale,
                    C_action = "Add New",
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_object = " Medical Facility",
                    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();
            }
            else
            {
                ViewBag.queryMessage = "202";
            }
          
            return View();
        }
        public String AddHealthBeneficiaire()
        {
            using (System.IO.StreamReader sReader=new System.IO.StreamReader(Request.InputStream))
            {
                var data = sReader.ReadToEnd();
                JavaScriptSerializer serial = new JavaScriptSerializer();
                Models.t_centre_soins center_health = serial.Deserialize<Models.t_centre_soins>(data);
                dbContext.t_centre_soins.Add(center_health);
                dbContext.SaveChanges();
                Authenticate auth = (Authenticate)Session["userinfo"];
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_company = auth.nameSuccursale,
                    C_action = "Add Medical facility",
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_object = "Medical_facility",
                    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.id)).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();
                return "200";
            }
        }

        [Route("getListEmployed/{name}")]
        public String SearchEmployed(String name)
        {
            int idFind = 0;
            String data = "";

            if (int.TryParse(name,out idFind))
            {
                String idEmployed = "";
                List<Models.Employed> ListEmployed = new List<Employed>();
                var Query = from beneficiaire in dbContext.t_beneficiaires
                            join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                            join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                            where beneficiaire.C_mat.Equals(name) && !beneficiaire.C_id_succ.Equals(null)
                            select new { employed = beneficiaire, succursal = succursale, department = departement };


                foreach (var item in Query)
                {
                    idEmployed = item.employed.C_id.ToString();
                    List<Models.Children> ListChilds = new List<Children>();
                    var Childs = from ds in dbContext.t_beneficiaires
                                 where ds.C_id_parent.Equals(idEmployed)
                                 select new { id = ds.C_id, name = ds.C_name, sex = ds.C_sex, datenais = ds.C_datenais, picture = ds.C_picture, parent = ds.C_id_parent, status = ds.C_statusChild,status_account=ds.C_status_system };

                    foreach (var child in Childs)
                    {
                        Models.Children Children = new Children
                        {
                            id = child.id.ToString(),
                            name = child.name,
                            datenais = child.datenais.ToString(),
                            sex = child.sex,
                            parent = item.employed.C_id.ToString(),
                            picture = child.picture,
                            status = child.status,
                            account_system = (child.status_account.Equals("1") ? "Enabled":"Disabled")
                        };
                        ListChilds.Add(Children);
                    }

                    var Partner = (from beneficiaire in dbContext.t_beneficiaires
                                   join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                   join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                   where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                   orderby beneficiaire.C_id descending
                                   select new { employed = beneficiaire, succursal = succursale, department = departement }).FirstOrDefault();

                    if (Partner != null)
                    {
                       
                        List<Models.Children> listChildren = new List<Children>();
                        String idPartner = Partner.employed.C_id.ToString();
                        var ChildsPartner = from ds in dbContext.t_beneficiaires
                                            where ds.C_id_parent.Equals(idPartner)
                                            select ds;


                        foreach (var children in ChildsPartner)
                        {
                            Models.Children childFocusable = new Children()
                            {
                                id = children.C_id.ToString(),
                                name = children.C_name,
                                sex = children.C_sex,
                                datenais = children.C_datenais,
                                picture = children.C_picture,
                                status = children.C_statusChild,
                                parent = idPartner,
                                account_system = (children.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                            };
                            listChildren.Add(childFocusable);
                        }
                        var Employed = Query.FirstOrDefault();
                        Models.Partner partner = new Partner
                        {
                            id = Partner.employed.C_id.ToString(),
                            Matricule = Partner.employed.C_mat,
                            name = Partner.employed.C_name,
                            sex = Partner.employed.C_sex,
                            phone = Partner.employed.C_phone,
                            datenaiss = Partner.employed.C_datenais.ToString(),
                            ID_Succursale = Partner.succursal.C_name,
                            ID_Departement = Partner.department.C_id_depart,
                            Childs = listChildren,
                            CivilStatus = "Marié",
                            account_system=Partner.employed.C_status_system,
                            partner = new Partner()
                            {
                                id = idEmployed,
                                name = Employed.employed.C_name,
                                sex = Employed.employed.C_sex,
                                phone = Employed.employed.C_phone,
                                CivilStatus = "Marié",
                                picture = Employed.employed.C_picture,
                                ID_Succursale = Employed.employed.C_id_succ,
                                ID_Departement = Employed.employed.C_id_depart.ToString(),
                                datenaiss = Employed.employed.C_datenais,
                                account_system = (Employed.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                            },
                            picture = Partner.employed.C_picture
                        };

                        Models.Employed employed = new Employed
                        {
                            id = idEmployed,
                            Matricule = item.employed.C_mat,
                            name = item.employed.C_name,
                            sex = item.employed.C_sex,
                            phone = item.employed.C_phone,
                            ID_Succursale = item.succursal.C_name,
                            ID_Departement = item.department.C_id_depart,
                            datenaiss = item.employed.C_datenais.ToString(),
                            picture = item.employed.C_picture,
                            Childs = ListChilds,
                            partner = partner,
                            CivilStatus = item.employed.C_statusmaritalk,
                            account_system = (item.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                        };
                        ListEmployed.Add(employed);

                    }
                    else
                    {

                        var Conjoint = (from beneficiaire in dbContext.t_beneficiaires
                                        where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                        select beneficiaire).FirstOrDefault();

                        if (Conjoint != null)
                        {
                            Models.Partner partner = new Partner
                            {
                                id = Conjoint.C_id.ToString(),
                                name = Conjoint.C_name,
                                sex = Conjoint.C_sex,
                                phone = Conjoint.C_phone,
                                datenaiss = Conjoint.C_datenais.ToString(),
                                ID_Succursale = null,
                                ID_Departement = null,
                                Childs = new List<Children>(),
                                partner = new Partner(),
                                picture = Conjoint.C_picture,
                                account_system = (Conjoint.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                            };
                            Models.Employed employed = new Employed
                            {
                                id = idEmployed,
                                Matricule = item.employed.C_mat,
                                name = item.employed.C_name,
                                sex = item.employed.C_sex,
                                phone = item.employed.C_phone,
                                ID_Succursale = item.succursal.C_name,
                                ID_Departement = item.department.C_id_depart,
                                datenaiss = item.employed.C_datenais.ToString(),
                                picture = item.employed.C_picture,
                                Childs = ListChilds,
                                partner = partner,
                                CivilStatus = item.employed.C_statusmaritalk,
                             account_system = (item.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                            };
                            ListEmployed.Add(employed);
                        }
                        else
                        {

                            Models.Employed employed = new Employed
                            {
                                id = idEmployed,
                                Matricule = item.employed.C_mat,
                                name = item.employed.C_name,
                                sex = item.employed.C_sex,
                                phone = item.employed.C_phone,
                                ID_Succursale = item.succursal.C_name,
                                ID_Departement = item.department.C_id_depart,
                                datenaiss = item.employed.C_datenais.ToString(),
                                picture = item.employed.C_picture,
                                Childs = ListChilds,
                                partner = null,
                                CivilStatus = item.employed.C_statusmaritalk,
                                account_system = (item.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                            };
                            ListEmployed.Add(employed);
                        }

                    }


                }
                JavaScriptSerializer serial = new JavaScriptSerializer();
                data = serial.Serialize(ListEmployed);
            }
            else
            {
                String idEmployed = "";
                List<Models.Employed> ListEmployed = new List<Employed>();
                var Query = from beneficiaire in dbContext.t_beneficiaires
                            join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                            join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                            where beneficiaire.C_name.Contains(name) && !beneficiaire.C_id_succ.Equals(null)
                            select new { employed = beneficiaire, succursal = succursale, department = departement };


                foreach (var item in Query)
                {
                    idEmployed = item.employed.C_id.ToString();
                    List<Models.Children> ListChilds = new List<Children>();
                    var Childs = from ds in dbContext.t_beneficiaires
                                 where ds.C_id_parent.Equals(idEmployed)
                                 select new { id = ds.C_id, name = ds.C_name, sex = ds.C_sex, datenais = ds.C_datenais, picture = ds.C_picture, parent = ds.C_id_parent, status = ds.C_statusChild,account_system=ds.C_status_system };

                    foreach (var child in Childs)
                    {
                        Models.Children Children = new Children
                        {
                            id = child.id.ToString(),
                            name = child.name,
                            datenais = child.datenais.ToString(),
                            sex = child.sex,
                            parent = item.employed.C_id.ToString(),
                            picture = child.picture,
                            status = child.status,
                            account_system = (child.account_system.Equals("1") ? "Enabled" : "Disabled")
                        };
                        ListChilds.Add(Children);
                    }

                    var Partner = (from beneficiaire in dbContext.t_beneficiaires
                                   join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                   join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                   where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                   orderby beneficiaire.C_id descending
                                   select new { employed = beneficiaire, succursal = succursale, department = departement }).FirstOrDefault();

                    if (Partner != null)
                    {
                        List<Models.Children> listChildren = new List<Children>();
                        String idPartner = Partner.employed.C_id.ToString();
                        var ChildsPartner = from ds in dbContext.t_beneficiaires
                                            where ds.C_id_parent.Equals(idPartner)
                                            select ds;


                        foreach (var children in ChildsPartner)
                        {
                            Models.Children childFocusable = new Children()
                            {
                                id = children.C_id.ToString(),
                                name = children.C_name,
                                sex = children.C_sex,
                                datenais = children.C_datenais,
                                picture = children.C_picture,
                                status = children.C_statusChild,
                                parent = idPartner,
                                account_system = (children.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                            };
                            listChildren.Add(childFocusable);
                        }
                        var Employed = Query.FirstOrDefault();
                        Models.Partner partner = new Partner
                        {
                            id = Partner.employed.C_id.ToString(),
                            Matricule = Partner.employed.C_mat,
                            name = Partner.employed.C_name,
                            sex = Partner.employed.C_sex,
                            phone = Partner.employed.C_phone,
                            datenaiss = Partner.employed.C_datenais.ToString(),
                            ID_Succursale = Partner.succursal.C_name,
                            ID_Departement = Partner.department.C_id_depart,
                            Childs = listChildren,
                            CivilStatus = "Marié",
                            account_system = (Partner.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled"),
                            partner = new Partner()
                            {
                                id = idEmployed,
                                name = Employed.employed.C_name,
                                sex = Employed.employed.C_sex,
                                phone = Employed.employed.C_phone,
                                CivilStatus = "Marié",
                                picture = Employed.employed.C_picture,
                                ID_Succursale = Employed.employed.C_id_succ,
                                ID_Departement = Employed.employed.C_id_depart.ToString(),
                                datenaiss = Employed.employed.C_datenais,
                                account_system=Employed.employed.C_status_system
                            },
                            picture = Partner.employed.C_picture
                        };

                        Models.Employed employed = new Employed
                        {
                            id = idEmployed,
                            Matricule = item.employed.C_mat,
                            name = item.employed.C_name,
                            sex = item.employed.C_sex,
                            phone = item.employed.C_phone,
                            ID_Succursale = item.succursal.C_name,
                            ID_Departement = item.department.C_id_depart,
                            datenaiss = item.employed.C_datenais.ToString(),
                            picture = item.employed.C_picture,
                            Childs = ListChilds,
                            partner = partner,
                            CivilStatus = item.employed.C_statusmaritalk,
                            account_system = (item.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                        };
                        ListEmployed.Add(employed);

                    }
                    else
                    {

                        var Conjoint = (from beneficiaire in dbContext.t_beneficiaires
                                        where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                        select beneficiaire).FirstOrDefault();

                        if (Conjoint != null)
                        {
                            Models.Partner partner = new Partner
                            {
                                id = Conjoint.C_id.ToString(),
                                name = Conjoint.C_name,
                                sex = Conjoint.C_sex,
                                phone = Conjoint.C_phone,
                                datenaiss = Conjoint.C_datenais.ToString(),
                                ID_Succursale = null,
                                ID_Departement = null,
                                Childs = new List<Children>(),
                                partner = new Partner(),
                                picture = Conjoint.C_picture,
                                account_system = (Conjoint.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                            };
                            Models.Employed employed = new Employed
                            {
                                id = idEmployed,
                                Matricule = item.employed.C_mat,
                                name = item.employed.C_name,
                                sex = item.employed.C_sex,
                                phone = item.employed.C_phone,
                                ID_Succursale = item.succursal.C_name,
                                ID_Departement = item.department.C_id_depart,
                                datenaiss = item.employed.C_datenais.ToString(),
                                picture = item.employed.C_picture,
                                Childs = ListChilds,
                                partner = partner,
                                CivilStatus = item.employed.C_statusmaritalk,
                                account_system = (item.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                            };
                            ListEmployed.Add(employed);
                        }
                        else
                        {

                            Models.Employed employed = new Employed
                            {
                                id = idEmployed,
                                Matricule = item.employed.C_mat,
                                name = item.employed.C_name,
                                sex = item.employed.C_sex,
                                phone = item.employed.C_phone,
                                ID_Succursale = item.succursal.C_name,
                                ID_Departement = item.department.C_id_depart,
                                datenaiss = item.employed.C_datenais.ToString(),
                                picture = item.employed.C_picture,
                                Childs = ListChilds,
                                partner = null,
                                CivilStatus = item.employed.C_statusmaritalk,
                                account_system = (item.employed.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                            };
                            ListEmployed.Add(employed);
                        }

                    }


                }
                JavaScriptSerializer serial = new JavaScriptSerializer();
                data = serial.Serialize(ListEmployed);
            }
           
            return data.ToString();
        }

        public ActionResult AddBonCommand()
        {
            var Query = from ds in dbContext.t_sickness
                        orderby ds.C_Name
                        select ds;
            ViewData["sicks"] = new SelectList(Query.ToList(), "C_Name", "C_Name");
            return View();
           
        }
        
        public ActionResult ViewEmployedCommand()
        {
            return View();
        }

        [HttpPost]
        public String setBonCommand()
        {
            using (System.IO.StreamReader sReader=new System.IO.StreamReader(Request.InputStream))
            {
                JavaScriptSerializer serial = new JavaScriptSerializer();
                var data = sReader.ReadToEnd();
                Models.t_bon_commandes bCommand = serial.Deserialize<Models.t_bon_commandes>(data);
          
                dbContext.t_bon_commandes.Add(bCommand);
                dbContext.SaveChanges();
                String categorieBenef = "";
                var Query = (from ds in dbContext.t_beneficiaires
                             where ds.C_id == bCommand.C_id_bene
                             select ds).FirstOrDefault();
                //if (!String.IsNullOrEmpty(Query.C_mat))
                //{
                //    categorieBenef = "Employee";
                //}
                //else if (!String.IsNullOrEmpty(Query.C_id_partenaire) && String.IsNullOrEmpty(Query.C_mat))
                //{
                //    categorieBenef = "Spouse";
                //}
                //if (!String.IsNullOrEmpty(Query.C_id_parent))
                //{
                //    categorieBenef = "Children";
                //}
                //if (!String.IsNullOrEmpty(Query.C_id_visitor))
                //{
                //    categorieBenef = "Visitor";
                //}

                
                //Authenticate auth = (Authenticate)Session["userinfo"];
                //if (auth!=null)
                //{
                //    T_logs logs = new T_logs()
                //    {
                //        C_user = auth.username,
                //        C_company = auth.nameSuccursale,
                //        C_action = "Add New",
                //        C_date = DateTime.Now.ToShortDateString(),
                //        C_time = DateTime.Now.ToShortTimeString(),
                //        C_object = "Voucher Beneficiairy ",
                //        C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.id)).FirstOrDefault().C_mat

                //    };
                //    dbContext.T_logs.Add(logs);
                //    dbContext.SaveChanges();
                //}
               

                return bCommand.C_id_bon.ToString();
            }
        }
       
        [Route("getlistforcommand/{name}")]
        //[AllowAnonymous]
        public String SearchEmployedForCommand(string name,string category)
        {
            string response = string.Empty;
            if (!String.IsNullOrEmpty(Request.Params["category"]))
            {

                switch (Request.Params["category"].ToLower())
                {
                    case "employee":
                        response=this.getEmployedForCommand(name);
                        break;
                    case "spouse":
                        response = this.getPartnerForCommand(name);
                        break;
                    case "child":
                        response = this.getChildsForCommand(name);
                        break;
                    case "visitor":
                        response = this.getVisitorForCommand(name);
                        break;
                    default:
                        break;
                }
                                           
            }
            return response;
        }
        private String getVisitorForCommand(String name)
        {
            String data = "";
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    List<Visitor> lstVisitor = new List<Visitor>();
                    var QueryVisitor = from ds in dbContext.t_beneficiaires
                                       join company in dbContext.t_succursales
                                       on ds.C_id_visitor equals company.C_id
                                       where ds.C_name.Contains(name) && ds.C_id_visitor.Equals(auth.Succursale)
                                       select new Visitor
                                       { 
                                           Uid= ds.C_id,
                                           name=ds.C_name,
                                           sex=ds.C_sex,
                                           ComapnyName=company.C_name,
                                           idVisitor=ds.C_id_visitor,
                                           picture=ds.C_picture,
                                           Phone=ds.C_phone,
                                           CompanyVisitor=ds.C_company_visitor,
                                           status=(ds.C_status_system.Equals("1")?"Enabled":"Disabled")
                                       };
                    if (QueryVisitor.ToList().Count>0)
                    {
                        foreach (var item in QueryVisitor)
                        {
                            lstVisitor.Add(item);
                        }
                        JavaScriptSerializer serial = new JavaScriptSerializer();
                        data = serial.Serialize(lstVisitor); 
                    }
                }
                else
                {
                    List<Visitor> lstVisitor = new List<Visitor>();
                    var QueryVisitor = from ds in dbContext.t_beneficiaires
                                       join company in dbContext.t_succursales
                                       on ds.C_id_visitor equals company.C_id
                                       where ds.C_name.Contains(name)
                                       select new Visitor
                                       {
                                           Uid = ds.C_id,
                                           name = ds.C_name,
                                           sex = ds.C_sex,
                                           ComapnyName = company.C_name,
                                           idVisitor = ds.C_id_visitor,
                                           picture = ds.C_picture,
                                           Phone = ds.C_phone,
                                           CompanyVisitor = ds.C_company_visitor,
                                           status = (ds.C_status_system.Equals("1") ? "Enabled" : "Disabled"),
                                           account_system= (ds.C_status_system.Equals("1") ? "Enabled" : "Disabled")

                                       };
                    if (QueryVisitor.ToList().Count > 0)
                    {
                        foreach (var item in QueryVisitor)
                        {
                            lstVisitor.Add(item);
                        }
                        JavaScriptSerializer serial = new JavaScriptSerializer();
                        data = serial.Serialize(lstVisitor);
                    }
                }
            }
            return data;
        }
        private String getEmployedForCommand(String name)
        {
            String idEmployed = "";
            int idFind = 0;
            String data = "";
            if (int.TryParse(name,out idFind))
            {
                List<Models.Employed> ListEmployed = new List<Employed>();
                if (Session["userinfo"] != null)
                {
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    if (auth.Priority.Equals("user"))
                    {
                        var Query = from beneficiaire in dbContext.t_beneficiaires
                                    join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                    join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                    where beneficiaire.C_mat.Equals(name) && beneficiaire.C_id_succ.Equals(auth.Succursale)
                                    select new { employed = beneficiaire, succursal = succursale, department = departement };


                        foreach (var item in Query)
                        {
                            idEmployed = item.employed.C_id.ToString();
                            List<Models.Children> ListChilds = new List<Children>();
                            var Childs = from ds in dbContext.t_beneficiaires
                                         where ds.C_id_parent.Equals(idEmployed)
                                         select new { id = ds.C_id, name = ds.C_name, sex = ds.C_sex, datenais = ds.C_datenais, picture = ds.C_picture, parent = ds.C_id_parent, status = ds.C_statusChild,status_system=ds.C_status_system };

                            foreach (var child in Childs)
                            {
                                Models.Children Children = new Children
                                {
                                    id = child.id.ToString(),
                                    name = child.name,
                                    datenais = child.datenais.ToString(),
                                    sex = child.sex,
                                    parent = item.employed.C_id.ToString(),
                                    picture = child.picture,
                                    status = child.status,
                                    account_system=(child.status_system.Equals("1")?"Enabled":"Disabled")
                                    
                                };
                                ListChilds.Add(Children);
                            }

                            var Partner = (from beneficiaire in dbContext.t_beneficiaires
                                           join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                           join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                           where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                           orderby beneficiaire.C_id descending
                                           select new { employed = beneficiaire, succursal = succursale, department = departement }).FirstOrDefault();

                            if (Partner != null)
                            {
                                List<Models.Children> listChildren = new List<Children>();
                                String idPartner = Partner.employed.C_id.ToString();
                                var ChildsPartner = from ds in dbContext.t_beneficiaires
                                                    where ds.C_id_parent.Equals(idPartner)
                                                    select ds;


                                foreach (var children in ChildsPartner)
                                {
                                    Models.Children childFocusable = new Children()
                                    {
                                        id = children.C_id.ToString(),
                                        name = children.C_name,
                                        sex = children.C_sex,
                                        datenais = children.C_datenais,
                                        picture = children.C_picture,
                                        status = children.C_statusChild,
                                        parent = idPartner,
                                        account_system= (children.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    };
                                    listChildren.Add(childFocusable);
                                }
                                var Employed = Query.FirstOrDefault();
                                Models.Partner partner = new Partner
                                {
                                    id = Partner.employed.C_id.ToString(),
                                    name = Partner.employed.C_name,
                                    sex = Partner.employed.C_sex,
                                    phone = Partner.employed.C_phone,
                                    datenaiss = Partner.employed.C_datenais.ToString(),
                                    ID_Succursale = Partner.succursal.C_name,
                                    ID_Departement = Partner.department.C_id_depart,
                                    Childs = listChildren,
                                    CivilStatus = "Marié",
                                    Matricule = Partner.employed.C_mat,
                                    account_system=(Partner.employed.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                    partner = new Partner()
                                    {
                                        id = idEmployed,
                                        name = Employed.employed.C_name,
                                        sex = Employed.employed.C_sex,
                                        phone = Employed.employed.C_phone,
                                        CivilStatus = "Marié",
                                        picture = Employed.employed.C_picture,
                                        ID_Succursale = Employed.employed.C_id_succ,
                                        ID_Departement = Employed.employed.C_id_depart.ToString(),
                                        datenaiss = Employed.employed.C_datenais,
                                        Matricule = Employed.employed.C_mat,
                                        account_system=(Employed.employed.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    },
                                    picture = Partner.employed.C_picture
                                };

                                Models.Employed employed = new Employed
                                {
                                    id = idEmployed,
                                    name = item.employed.C_name,
                                    sex = item.employed.C_sex,
                                    phone = item.employed.C_phone,
                                    ID_Succursale = item.succursal.C_name,
                                    ID_Departement = item.department.C_id_depart,
                                    datenaiss = item.employed.C_datenais.ToString(),
                                    picture = item.employed.C_picture,
                                    Childs = ListChilds,
                                    partner = partner,
                                    CivilStatus = item.employed.C_statusmaritalk,
                                    Matricule = item.employed.C_mat,
                                    account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                };
                                ListEmployed.Add(employed);

                            }
                            else
                            {

                                var Conjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                                select beneficiaire).FirstOrDefault();

                                if (Conjoint != null)
                                {
                                    Models.Partner partner = new Partner
                                    {
                                        id = Conjoint.C_id.ToString(),
                                        name = Conjoint.C_name,
                                        sex = Conjoint.C_sex,
                                        phone = Conjoint.C_phone,
                                        datenaiss = Conjoint.C_datenais.ToString(),
                                        ID_Succursale = null,
                                        ID_Departement = null,
                                        Childs = new List<Children>(),
                                        partner = new Partner(),
                                        picture = Conjoint.C_picture,
                                        Matricule = Conjoint.C_mat,
                                        account_system=(Conjoint.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    };
                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = partner,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }
                                else
                                {

                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = null,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }

                            }


                        }
                    }
                    else //PRIORITY
                    {
                        var Query = from beneficiaire in dbContext.t_beneficiaires
                                    join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                    join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                    where beneficiaire.C_mat.Equals(name)
                                    select new { employed = beneficiaire, succursal = succursale, department = departement };


                        foreach (var item in Query)
                        {
                            idEmployed = item.employed.C_id.ToString();
                            List<Models.Children> ListChilds = new List<Children>();
                            var Childs = from ds in dbContext.t_beneficiaires
                                         where ds.C_id_parent.Equals(idEmployed)
                                         select new { id = ds.C_id, name = ds.C_name, sex = ds.C_sex, datenais = ds.C_datenais, picture = ds.C_picture, parent = ds.C_id_parent, status = ds.C_statusChild,status_system=ds.C_status_system };

                            foreach (var child in Childs)
                            {
                                Models.Children Children = new Children
                                {
                                    id = child.id.ToString(),
                                    name = child.name,
                                    datenais = child.datenais.ToString(),
                                    sex = child.sex,
                                    parent = item.employed.C_id.ToString(),
                                    picture = child.picture,
                                    status = child.status,
                                    account_system=(child.status_system.Equals("1")?"Enabled":"Disabled")
                                };
                                ListChilds.Add(Children);
                            }

                            var Partner = (from beneficiaire in dbContext.t_beneficiaires
                                           join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                           join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                           where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                           orderby beneficiaire.C_id descending
                                           select new { employed = beneficiaire, succursal = succursale, department = departement }).FirstOrDefault();

                            if (Partner != null)
                            {
                                List<Models.Children> listChildren = new List<Children>();
                                String idPartner = Partner.employed.C_id.ToString();
                                var ChildsPartner = from ds in dbContext.t_beneficiaires
                                                    where ds.C_id_parent.Equals(idPartner)
                                                    select ds;


                                foreach (var children in ChildsPartner)
                                {
                                    Models.Children childFocusable = new Children()
                                    {
                                        id = children.C_id.ToString(),
                                        name = children.C_name,
                                        sex = children.C_sex,
                                        datenais = children.C_datenais,
                                        picture = children.C_picture,
                                        status = children.C_statusChild,
                                        parent = idPartner,
                                        account_system=(children.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    };
                                    listChildren.Add(childFocusable);
                                }
                                var Employed = Query.FirstOrDefault();
                                Models.Partner partner = new Partner
                                {
                                    id = Partner.employed.C_id.ToString(),
                                    name = Partner.employed.C_name,
                                    sex = Partner.employed.C_sex,
                                    phone = Partner.employed.C_phone,
                                    datenaiss = Partner.employed.C_datenais.ToString(),
                                    ID_Succursale = Partner.succursal.C_name,
                                    ID_Departement = Partner.department.C_id_depart,
                                    Childs = listChildren,
                                    CivilStatus = "Marié",
                                    Matricule = Partner.employed.C_mat,
                                    account_system=(Partner.employed.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                    partner = new Partner()
                                    {
                                        id = idEmployed,
                                        name = Employed.employed.C_name,
                                        sex = Employed.employed.C_sex,
                                        phone = Employed.employed.C_phone,
                                        CivilStatus = "Marié",
                                        picture = Employed.employed.C_picture,
                                        ID_Succursale = Employed.employed.C_id_succ,
                                        ID_Departement = Employed.employed.C_id_depart.ToString(),
                                        datenaiss = Employed.employed.C_datenais,
                                        Matricule = Employed.employed.C_mat,
                                        account_system=(Employed.employed.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    },
                                    picture = Partner.employed.C_picture
                                };

                                Models.Employed employed = new Employed
                                {
                                    id = idEmployed,
                                    name = item.employed.C_name,
                                    sex = item.employed.C_sex,
                                    phone = item.employed.C_phone,
                                    ID_Succursale = item.succursal.C_name,
                                    ID_Departement = item.department.C_id_depart,
                                    datenaiss = item.employed.C_datenais.ToString(),
                                    picture = item.employed.C_picture,
                                    Childs = ListChilds,
                                    partner = partner,
                                    CivilStatus = item.employed.C_statusmaritalk,
                                    Matricule = item.employed.C_mat,
                                    account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                };
                                ListEmployed.Add(employed);

                            }
                            else
                            {

                                var Conjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                                select beneficiaire).FirstOrDefault();

                                if (Conjoint != null)
                                {
                                    Models.Partner partner = new Partner
                                    {
                                        id = Conjoint.C_id.ToString(),
                                        name = Conjoint.C_name,
                                        sex = Conjoint.C_sex,
                                        phone = Conjoint.C_phone,
                                        datenaiss = Conjoint.C_datenais.ToString(),
                                        ID_Succursale = null,
                                        ID_Departement = null,
                                        Childs = new List<Children>(),
                                        partner = new Partner(),
                                        picture = Conjoint.C_picture,
                                        Matricule = Conjoint.C_mat,
                                        account_system=(Conjoint.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = partner,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }
                                else
                                {

                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = null,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }

                            }


                        }
                    }
                }
                JavaScriptSerializer serial = new JavaScriptSerializer();
                data = serial.Serialize(ListEmployed);
                
            }
            else
            {
                List<Models.Employed> ListEmployed = new List<Employed>();
                if (Session["userinfo"] != null)
                {
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    if (auth.Priority.Equals("user"))
                    {
                        var Query = from beneficiaire in dbContext.t_beneficiaires
                                    join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                    join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                    where beneficiaire.C_name.Contains(name) && beneficiaire.C_id_succ.Equals(auth.Succursale)
                                    select new { employed = beneficiaire, succursal = succursale, department = departement };


                        foreach (var item in Query)
                        {
                            idEmployed = item.employed.C_id.ToString();
                            List<Models.Children> ListChilds = new List<Children>();
                            var Childs = from ds in dbContext.t_beneficiaires
                                         where ds.C_id_parent.Equals(idEmployed)
                                         select new { id = ds.C_id, name = ds.C_name, sex = ds.C_sex, datenais = ds.C_datenais, picture = ds.C_picture, parent = ds.C_id_parent, status = ds.C_statusChild,status_system=ds.C_status_system };

                            foreach (var child in Childs)
                            {
                                Models.Children Children = new Children
                                {
                                    id = child.id.ToString(),
                                    name = child.name,
                                    datenais = child.datenais.ToString(),
                                    sex = child.sex,
                                    parent = item.employed.C_id.ToString(),
                                    picture = child.picture,
                                    status = child.status,
                                    account_system=(child.status_system.Equals("1")?"Enabled":"Disabled")
                                };
                                ListChilds.Add(Children);
                            }

                            var Partner = (from beneficiaire in dbContext.t_beneficiaires
                                           join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                           join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                           where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                           orderby beneficiaire.C_id descending
                                           select new { employed = beneficiaire, succursal = succursale, department = departement }).FirstOrDefault();

                            if (Partner != null)
                            {
                                List<Models.Children> listChildren = new List<Children>();
                                String idPartner = Partner.employed.C_id.ToString();
                                var ChildsPartner = from ds in dbContext.t_beneficiaires
                                                    where ds.C_id_parent.Equals(idPartner)
                                                    select ds;


                                foreach (var children in ChildsPartner)
                                {
                                    Models.Children childFocusable = new Children()
                                    {
                                        id = children.C_id.ToString(),
                                        name = children.C_name,
                                        sex = children.C_sex,
                                        datenais = children.C_datenais,
                                        picture = children.C_picture,
                                        status = children.C_statusChild,
                                        parent = idPartner,
                                        account_system=(children.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    };
                                    listChildren.Add(childFocusable);
                                }
                                var Employed = Query.FirstOrDefault();
                                Models.Partner partner = new Partner
                                {
                                    id = Partner.employed.C_id.ToString(),
                                    name = Partner.employed.C_name,
                                    sex = Partner.employed.C_sex,
                                    phone = Partner.employed.C_phone,
                                    datenaiss = Partner.employed.C_datenais.ToString(),
                                    ID_Succursale = Partner.succursal.C_name,
                                    ID_Departement = Partner.department.C_id_depart,
                                    Childs = listChildren,
                                    CivilStatus = "Marié",
                                    Matricule = Partner.employed.C_mat,
                                    account_system=(Partner.employed.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                    partner = new Partner()
                                    {
                                        id = idEmployed,
                                        name = Employed.employed.C_name,
                                        sex = Employed.employed.C_sex,
                                        phone = Employed.employed.C_phone,
                                        CivilStatus = "Marié",
                                        picture = Employed.employed.C_picture,
                                        ID_Succursale = Employed.employed.C_id_succ,
                                        ID_Departement = Employed.employed.C_id_depart.ToString(),
                                        datenaiss = Employed.employed.C_datenais,
                                        Matricule = Employed.employed.C_mat,
                                        account_system=(Employed.employed.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    },
                                    picture = Partner.employed.C_picture
                                };

                                Models.Employed employed = new Employed
                                {
                                    id = idEmployed,
                                    name = item.employed.C_name,
                                    sex = item.employed.C_sex,
                                    phone = item.employed.C_phone,
                                    ID_Succursale = item.succursal.C_name,
                                    ID_Departement = item.department.C_id_depart,
                                    datenaiss = item.employed.C_datenais.ToString(),
                                    picture = item.employed.C_picture,
                                    Childs = ListChilds,
                                    partner = partner,
                                    CivilStatus = item.employed.C_statusmaritalk,
                                    Matricule = item.employed.C_mat,
                                    account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                };
                                ListEmployed.Add(employed);

                            }
                            else
                            {

                                var Conjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                                select beneficiaire).FirstOrDefault();

                                if (Conjoint != null)
                                {
                                    Models.Partner partner = new Partner
                                    {
                                        id = Conjoint.C_id.ToString(),
                                        name = Conjoint.C_name,
                                        sex = Conjoint.C_sex,
                                        phone = Conjoint.C_phone,
                                        datenaiss = Conjoint.C_datenais.ToString(),
                                        ID_Succursale = null,
                                        ID_Departement = null,
                                        Childs = new List<Children>(),
                                        partner = new Partner(),
                                        picture = Conjoint.C_picture,
                                        Matricule = Conjoint.C_mat,
                                        account_system = (Conjoint.C_status_system.Equals("1") ? "Enabled":"Disabled")
                                    };
                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = partner,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }
                                else
                                {

                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = null,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }

                            }


                        }
                    }
                    else //PRIORITY
                    {
                        var Query = from beneficiaire in dbContext.t_beneficiaires
                                    join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                    join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                    where beneficiaire.C_name.Contains(name)
                                    select new { employed = beneficiaire, succursal = succursale, department = departement };


                        foreach (var item in Query)
                        {
                            idEmployed = item.employed.C_id.ToString();
                            List<Models.Children> ListChilds = new List<Children>();
                            var Childs = from ds in dbContext.t_beneficiaires
                                         where ds.C_id_parent.Equals(idEmployed)
                                         select new { id = ds.C_id, name = ds.C_name, sex = ds.C_sex, datenais = ds.C_datenais, picture = ds.C_picture, parent = ds.C_id_parent, status = ds.C_statusChild,status_system=ds.C_status_system };

                            foreach (var child in Childs)
                            {
                                Models.Children Children = new Children
                                {
                                    id = child.id.ToString(),
                                    name = child.name,
                                    datenais = child.datenais.ToString(),
                                    sex = child.sex,
                                    parent = item.employed.C_id.ToString(),
                                    picture = child.picture,
                                    status = child.status,
                                    account_system=(child.status_system.Equals("1")?"Enabled":"Disabled")
                                };
                                ListChilds.Add(Children);
                            }

                            var Partner = (from beneficiaire in dbContext.t_beneficiaires
                                           join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                           join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                           where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                           orderby beneficiaire.C_id descending
                                           select new { employed = beneficiaire, succursal = succursale, department = departement }).FirstOrDefault();

                            if (Partner != null)
                            {
                                List<Models.Children> listChildren = new List<Children>();
                                String idPartner = Partner.employed.C_id.ToString();
                                var ChildsPartner = from ds in dbContext.t_beneficiaires
                                                    where ds.C_id_parent.Equals(idPartner)
                                                    select ds;


                                foreach (var children in ChildsPartner)
                                {
                                    Models.Children childFocusable = new Children()
                                    {
                                        id = children.C_id.ToString(),
                                        name = children.C_name,
                                        sex = children.C_sex,
                                        datenais = children.C_datenais,
                                        picture = children.C_picture,
                                        status = children.C_statusChild,
                                        parent = idPartner,
                                        account_system=(children.C_status_system.Equals("1")?"Enabled":"Disabled")
                                        
                                    };
                                    listChildren.Add(childFocusable);
                                }
                                var Employed = Query.FirstOrDefault();
                                Models.Partner partner = new Partner
                                {
                                    id = Partner.employed.C_id.ToString(),
                                    name = Partner.employed.C_name,
                                    sex = Partner.employed.C_sex,
                                    phone = Partner.employed.C_phone,
                                    datenaiss = Partner.employed.C_datenais.ToString(),
                                    ID_Succursale = Partner.succursal.C_name,
                                    ID_Departement = Partner.department.C_id_depart,
                                    Childs = listChildren,
                                    CivilStatus = "Marié",
                                    Matricule = Partner.employed.C_mat,
                                    account_system=(Partner.employed.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                    partner = new Partner()
                                    {
                                        id = idEmployed,
                                        name = Employed.employed.C_name,
                                        sex = Employed.employed.C_sex,
                                        phone = Employed.employed.C_phone,
                                        CivilStatus = "Marié",
                                        picture = Employed.employed.C_picture,
                                        ID_Succursale = Employed.employed.C_id_succ,
                                        ID_Departement = Employed.employed.C_id_depart.ToString(),
                                        datenaiss = Employed.employed.C_datenais,
                                        Matricule = Employed.employed.C_mat,
                                        account_system=(Employed.employed.C_status_system.Equals("1")?"Enabled":"Disabled")
                                    },
                                    picture = Partner.employed.C_picture
                                };

                                Models.Employed employed = new Employed
                                {
                                    id = idEmployed,
                                    name = item.employed.C_name,
                                    sex = item.employed.C_sex,
                                    phone = item.employed.C_phone,
                                    ID_Succursale = item.succursal.C_name,
                                    ID_Departement = item.department.C_id_depart,
                                    datenaiss = item.employed.C_datenais.ToString(),
                                    picture = item.employed.C_picture,
                                    Childs = ListChilds,
                                    partner = partner,
                                    CivilStatus = item.employed.C_statusmaritalk,
                                    Matricule = item.employed.C_mat,
                                    account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                };
                                ListEmployed.Add(employed);

                            }
                            else
                            {

                                var Conjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                where beneficiaire.C_id_partenaire.Equals(idEmployed)
                                                select beneficiaire).FirstOrDefault();

                                if (Conjoint != null)
                                {
                                    Models.Partner partner = new Partner
                                    {
                                        id = Conjoint.C_id.ToString(),
                                        name = Conjoint.C_name,
                                        sex = Conjoint.C_sex,
                                        phone = Conjoint.C_phone,
                                        datenaiss = Conjoint.C_datenais.ToString(),
                                        ID_Succursale = null,
                                        ID_Departement = null,
                                        Childs = new List<Children>(),
                                        partner = new Partner(),
                                        picture = Conjoint.C_picture,
                                        Matricule = Conjoint.C_mat,
                                        account_system=(Conjoint.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = partner,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }
                                else
                                {

                                    Models.Employed employed = new Employed
                                    {
                                        id = idEmployed,
                                        name = item.employed.C_name,
                                        sex = item.employed.C_sex,
                                        phone = item.employed.C_phone,
                                        ID_Succursale = item.succursal.C_name,
                                        ID_Departement = item.department.C_id_depart,
                                        datenaiss = item.employed.C_datenais.ToString(),
                                        picture = item.employed.C_picture,
                                        Childs = ListChilds,
                                        partner = null,
                                        CivilStatus = item.employed.C_statusmaritalk,
                                        Matricule = item.employed.C_mat,
                                        account_system=(item.employed.C_status_system.Equals("1")?"Enabled":"Disabled")

                                    };
                                    ListEmployed.Add(employed);
                                }

                            }


                        }
                    }
                }
                JavaScriptSerializer serial = new JavaScriptSerializer();
                data = serial.Serialize(ListEmployed);
            }

            return data.ToString();
        }
        
        private String getPartnerForCommand(String name)
        {
           
            //bool controlIterator = false;
            
            List<Models.Employed> ConjointList = new List<Employed>();

            if (Session["userinfo"] != null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    var Partner = (from beneficiaire in dbContext.t_beneficiaires

                                   join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                   join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                   where beneficiaire.C_name.Contains(name) && !beneficiaire.C_id_partenaire.Equals(null) && beneficiaire.C_mat.Equals(null)
                                   select new { beneficiaire, succursale, departement });

                    if (Partner.ToList().Count > 0)
                    {
                        ConjointList.Clear();
                        foreach (var item in Partner)
                        {
                            if (!String.IsNullOrEmpty(item.beneficiaire.C_id_partenaire))
                            {

                                int idConjoint = int.Parse(item.beneficiaire.C_id_partenaire);
                                var QueryConjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                     join succursale in dbContext.t_succursales
                                                         on beneficiaire.C_id_succ equals succursale.C_id
                                                     join departement in dbContext.t_departement
                                                         on beneficiaire.C_id_depart equals departement.C_id
                                                     where beneficiaire.C_id.Equals(idConjoint) && beneficiaire.C_id_succ.Equals(auth.Succursale)
                                                     select new { beneficiaire, succursale, departement }).FirstOrDefault();

                                if (QueryConjoint != null)
                                {

                                    string idConjointChild = QueryConjoint.beneficiaire.C_id.ToString();
                                    List<Models.Children> ChildsPartner = new List<Children>();
                                    List<Models.Children> ChildsConjoint = new List<Children>();
                                    var QueryChild = from ds in dbContext.t_beneficiaires
                                                     where ds.C_id_parent.Equals(idConjointChild)
                                                     select ds;

                                    if (QueryChild.ToList().Count > 0)
                                    {
                                        foreach (var itemChilds in QueryChild)
                                        {
                                            Models.Children child = new Children()
                                            {
                                                id = itemChilds.C_id.ToString(),
                                                name = itemChilds.C_name,
                                                sex = itemChilds.C_sex,
                                                status = itemChilds.C_statusChild,
                                                datenais = itemChilds.C_datenais,
                                                picture = itemChilds.C_picture,
                                                account_system=(itemChilds.C_status_system.Equals("1")?"Enabled":"Disabled")
                                                
                                            };
                                            ChildsPartner.Add(child);
                                        }

                                    }
                                    else
                                    {
                                        string idItem = item.beneficiaire.C_id.ToString();
                                        QueryChild = from ds in dbContext.t_beneficiaires
                                                     where ds.C_id_parent.Equals(idItem)

                                                     select ds;
                                        foreach (var itemChilds in QueryChild)
                                        { 
                                            Models.Children child = new Children()
                                            {
                                                id = itemChilds.C_id.ToString(),
                                                name = itemChilds.C_name,
                                                sex = itemChilds.C_sex,
                                                status = itemChilds.C_statusChild,
                                                datenais = itemChilds.C_datenais,
                                                picture = itemChilds.C_picture,
                                                account_system= (itemChilds.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                                            };
                                            ChildsConjoint.Add(child);
                                        }
                                    } 
                                    Models.Employed employed = new Employed()
                                    {
                                        id = QueryConjoint.beneficiaire.C_id.ToString(),
                                        name = QueryConjoint.beneficiaire.C_name,
                                        sex = QueryConjoint.beneficiaire.C_sex,
                                        phone = QueryConjoint.beneficiaire.C_phone,
                                        CivilStatus = QueryConjoint.beneficiaire.C_statusmaritalk,
                                        datenaiss = QueryConjoint.beneficiaire.C_datenais,
                                        ID_Succursale = QueryConjoint.succursale.C_name,
                                        ID_Departement = QueryConjoint.departement.C_id_depart,
                                        Childs = ChildsPartner,
                                        picture = QueryConjoint.beneficiaire.C_picture,
                                        Matricule=QueryConjoint.beneficiaire.C_mat,
                                        account_system=(QueryConjoint.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                        partner = new Partner
                                        {
                                            id = item.beneficiaire.C_id.ToString(),
                                            name = item.beneficiaire.C_name,
                                            sex = item.beneficiaire.C_sex,
                                            phone = item.beneficiaire.C_phone,
                                            CivilStatus = item.beneficiaire.C_statusmaritalk,
                                            datenaiss = item.beneficiaire.C_datenais,
                                            picture = item.beneficiaire.C_picture,
                                            Childs = ChildsConjoint,
                                            ID_Succursale = item.succursale.C_name,
                                            ID_Departement = item.departement.C_id_depart,
                                            Matricule=item.beneficiaire.C_mat,
                                            account_system=(item.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled")
                                            

                                        }
                                    };

                                    ConjointList.Add(employed);
                                }
                                else
                                {

                                }

                            }

                        }
                    }
                    else
                    {
                        ConjointList.Clear();

                        var Partner2 = from ds in dbContext.t_beneficiaires
                                       where ds.C_name.Contains(name) && !ds.C_id_partenaire.Equals(null) && ds.C_mat.Equals(null)
                                       select new { beneficiaire = ds };

                        if (Partner2.ToList().Count > 0)
                        {
                            foreach (var item in Partner2)
                            {
                                if (!String.IsNullOrEmpty(item.beneficiaire.C_id_partenaire))
                                {

                                    int idConjoint = int.Parse(item.beneficiaire.C_id_partenaire);
                                    var QueryConjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                         join succursale in dbContext.t_succursales
                                                             on beneficiaire.C_id_succ equals succursale.C_id
                                                         join departement in dbContext.t_departement
                                                             on beneficiaire.C_id_depart equals departement.C_id
                                                         where beneficiaire.C_id.Equals(idConjoint)
                                                         select new { beneficiaire, succursale, departement }).FirstOrDefault();

                                    if (QueryConjoint != null)
                                    {

                                        string idConjointChild = QueryConjoint.beneficiaire.C_id.ToString();
                                        List<Models.Children> ChildsPartner = new List<Children>();
                                        List<Models.Children> ChildsConjoint = new List<Children>();
                                        var QueryChild = from ds in dbContext.t_beneficiaires
                                                         where ds.C_id_parent.Equals(idConjointChild)
                                                         select ds;

                                        if (QueryChild.ToList().Count > 0)
                                        {
                                            foreach (var itemChilds in QueryChild)
                                            {
                                                Models.Children child = new Children()
                                                {
                                                    id = itemChilds.C_id.ToString(),
                                                    name = itemChilds.C_name,
                                                    sex = itemChilds.C_sex,
                                                    status = itemChilds.C_statusChild,
                                                    datenais = itemChilds.C_datenais,
                                                    picture = itemChilds.C_picture,
                                                    account_system = (itemChilds.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                                                };
                                                ChildsPartner.Add(child);
                                            }

                                        }
                                        else
                                        {
                                            string idItem = item.beneficiaire.C_id.ToString();
                                            QueryChild = from ds in dbContext.t_beneficiaires
                                                         where ds.C_id_parent.Equals(idItem)

                                                         select ds;
                                            foreach (var itemChilds in QueryChild)
                                            {
                                                Models.Children child = new Children()
                                                {
                                                    id = itemChilds.C_id.ToString(),
                                                    name = itemChilds.C_name,
                                                    sex = itemChilds.C_sex,
                                                    status = itemChilds.C_statusChild,
                                                    datenais = itemChilds.C_datenais,
                                                    picture = itemChilds.C_picture,
                                                    account_system = (itemChilds.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                                                };
                                                ChildsConjoint.Add(child);
                                            }
                                        }
                                        Models.Employed employed = new Employed()
                                        {
                                            id = QueryConjoint.beneficiaire.C_id.ToString(),
                                            name = QueryConjoint.beneficiaire.C_name,
                                            sex = QueryConjoint.beneficiaire.C_sex,
                                            phone = QueryConjoint.beneficiaire.C_phone,
                                            CivilStatus = QueryConjoint.beneficiaire.C_statusmaritalk,
                                            datenaiss = QueryConjoint.beneficiaire.C_datenais,
                                            ID_Succursale = QueryConjoint.succursale.C_name,
                                            ID_Departement = QueryConjoint.departement.C_id_depart,
                                            Childs = ChildsPartner,
                                            picture = QueryConjoint.beneficiaire.C_picture,
                                            Matricule = QueryConjoint.beneficiaire.C_mat,
                                            account_system=(QueryConjoint.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                            partner = new Partner
                                            {
                                                id = item.beneficiaire.C_id.ToString(),
                                                name = item.beneficiaire.C_name,
                                                sex = item.beneficiaire.C_sex,
                                                phone = item.beneficiaire.C_phone,
                                                CivilStatus = item.beneficiaire.C_statusmaritalk,
                                                datenaiss = item.beneficiaire.C_datenais,
                                                picture = item.beneficiaire.C_picture,
                                                Childs = ChildsConjoint,
                                                Matricule=item.beneficiaire.C_mat,
                                                account_system=(item.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled")


                                            }
                                        };

                                        ConjointList.Add(employed);
                                    }
                                    else
                                    {

                                    }

                                }

                            }
                        }
                    }
                }
                else
                {
                    var Partner = (from beneficiaire in dbContext.t_beneficiaires

                                   join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                   join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                   where beneficiaire.C_name.Contains(name) && !beneficiaire.C_id_partenaire.Equals(null) && beneficiaire.C_mat.Equals(null)
                                   select new { beneficiaire, succursale, departement });

                    if (Partner.ToList().Count > 0)
                    {
                        ConjointList.Clear();
                        foreach (var item in Partner)
                        {
                            if (!String.IsNullOrEmpty(item.beneficiaire.C_id_partenaire))
                            {

                                int idConjoint = int.Parse(item.beneficiaire.C_id_partenaire);
                                var QueryConjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                     join succursale in dbContext.t_succursales
                                                         on beneficiaire.C_id_succ equals succursale.C_id
                                                     join departement in dbContext.t_departement
                                                         on beneficiaire.C_id_depart equals departement.C_id
                                                     where beneficiaire.C_id.Equals(idConjoint)
                                                     select new { beneficiaire, succursale, departement }).FirstOrDefault();

                                if (QueryConjoint != null)
                                {

                                    string idConjointChild = QueryConjoint.beneficiaire.C_id.ToString();
                                    List<Models.Children> ChildsPartner = new List<Children>();
                                    List<Models.Children> ChildsConjoint = new List<Children>();
                                    var QueryChild = from ds in dbContext.t_beneficiaires
                                                     where ds.C_id_parent.Equals(idConjointChild)
                                                     select ds;

                                    if (QueryChild.ToList().Count > 0)
                                    {
                                        foreach (var itemChilds in QueryChild)
                                        {
                                            Models.Children child = new Children()
                                            {
                                                id = itemChilds.C_id.ToString(),
                                                name = itemChilds.C_name,
                                                sex = itemChilds.C_sex,
                                                status = itemChilds.C_statusChild,
                                                datenais = itemChilds.C_datenais,
                                                picture = itemChilds.C_picture,
                                                account_system = (itemChilds.C_status_system.Equals("1") ? "Enabled" : "Disabled")
                                            };
                                            ChildsPartner.Add(child);
                                        }

                                    }
                                    else
                                    {
                                        string idItem = item.beneficiaire.C_id.ToString();
                                        QueryChild = from ds in dbContext.t_beneficiaires
                                                     where ds.C_id_parent.Equals(idItem)

                                                     select ds;
                                        foreach (var itemChilds in QueryChild)
                                        {
                                            Models.Children child = new Children()
                                            {
                                                id = itemChilds.C_id.ToString(),
                                                name = itemChilds.C_name,
                                                sex = itemChilds.C_sex,
                                                status = itemChilds.C_statusChild,
                                                datenais = itemChilds.C_datenais,
                                                picture = itemChilds.C_picture,
                                                account_system=(itemChilds.C_status_system.Equals("1")?"Enabled":"Disabled")

                                            };
                                            ChildsConjoint.Add(child);
                                        }
                                    }
                                    Models.Employed employed = new Employed()
                                    {
                                        id = QueryConjoint.beneficiaire.C_id.ToString(),
                                        name = QueryConjoint.beneficiaire.C_name,
                                        sex = QueryConjoint.beneficiaire.C_sex,
                                        phone = QueryConjoint.beneficiaire.C_phone,
                                        CivilStatus = QueryConjoint.beneficiaire.C_statusmaritalk,
                                        datenaiss = QueryConjoint.beneficiaire.C_datenais,
                                        ID_Succursale = QueryConjoint.succursale.C_name,
                                        ID_Departement = QueryConjoint.departement.C_id_depart,
                                        Childs = ChildsPartner,
                                        picture = QueryConjoint.beneficiaire.C_picture,
                                        Matricule=QueryConjoint.beneficiaire.C_mat,
                                        account_system=(QueryConjoint.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled"),
                                        partner = new Partner
                                        {
                                            id = item.beneficiaire.C_id.ToString(),
                                            name = item.beneficiaire.C_name,
                                            sex = item.beneficiaire.C_sex,
                                            phone = item.beneficiaire.C_phone,
                                            CivilStatus = item.beneficiaire.C_statusmaritalk,
                                            datenaiss = item.beneficiaire.C_datenais,
                                            picture = item.beneficiaire.C_picture,
                                            Childs = ChildsConjoint,
                                            ID_Succursale = item.succursale.C_name,
                                            ID_Departement = item.departement.C_id_depart,
                                            Matricule=item.beneficiaire.C_mat,
                                            account_system=(item.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled")
                                            

                                        }
                                    };

                                    ConjointList.Add(employed);
                                }
                                else
                                {

                                }

                            }

                        }
                    }
                    else
                    {
                        ConjointList.Clear();

                        var Partner2 = from ds in dbContext.t_beneficiaires
                                       where ds.C_name.Contains(name) && !ds.C_id_partenaire.Equals(null) && ds.C_mat.Equals(null)
                                       select new { beneficiaire = ds };

                        if (Partner2.ToList().Count > 0)
                        {
                            foreach (var item in Partner2)
                            {
                                if (!String.IsNullOrEmpty(item.beneficiaire.C_id_partenaire))
                                {

                                    int idConjoint = int.Parse(item.beneficiaire.C_id_partenaire);
                                    var QueryConjoint = (from beneficiaire in dbContext.t_beneficiaires
                                                         join succursale in dbContext.t_succursales
                                                             on beneficiaire.C_id_succ equals succursale.C_id
                                                         join departement in dbContext.t_departement
                                                             on beneficiaire.C_id_depart equals departement.C_id
                                                         where beneficiaire.C_id.Equals(idConjoint)
                                                         select new { beneficiaire, succursale, departement }).FirstOrDefault();

                                    if (QueryConjoint != null)
                                    {

                                        string idConjointChild = QueryConjoint.beneficiaire.C_id.ToString();
                                        List<Models.Children> ChildsPartner = new List<Children>();
                                        List<Models.Children> ChildsConjoint = new List<Children>();
                                        var QueryChild = from ds in dbContext.t_beneficiaires
                                                         where ds.C_id_parent.Equals(idConjointChild)
                                                         select ds;

                                        if (QueryChild.ToList().Count > 0)
                                        {
                                            foreach (var itemChilds in QueryChild)
                                            {
                                                Models.Children child = new Children()
                                                {
                                                    id = itemChilds.C_id.ToString(),
                                                    name = itemChilds.C_name,
                                                    sex = itemChilds.C_sex,
                                                    status = itemChilds.C_statusChild,
                                                    datenais = itemChilds.C_datenais,
                                                    picture = itemChilds.C_picture,
                                                    account_system=(itemChilds.C_status_system.Equals("1")?"Enabled":"Disabled")
                                                };
                                                ChildsPartner.Add(child);
                                            }

                                        }
                                        else
                                        {
                                            string idItem = item.beneficiaire.C_id.ToString();
                                            QueryChild = from ds in dbContext.t_beneficiaires
                                                         where ds.C_id_parent.Equals(idItem)

                                                         select ds;
                                            foreach (var itemChilds in QueryChild)
                                            {
                                                Models.Children child = new Children()
                                                {
                                                    id = itemChilds.C_id.ToString(),
                                                    name = itemChilds.C_name,
                                                    sex = itemChilds.C_sex,
                                                    status = itemChilds.C_statusChild,
                                                    datenais = itemChilds.C_datenais,
                                                    picture = itemChilds.C_picture,
                                                    account_system=(itemChilds.C_status_system.Equals("1")?"Enabled":"Disabled")
                                                };
                                                ChildsConjoint.Add(child);
                                            }
                                        }
                                        Models.Employed employed = new Employed()
                                        {
                                            id = QueryConjoint.beneficiaire.C_id.ToString(),
                                            name = QueryConjoint.beneficiaire.C_name,
                                            sex = QueryConjoint.beneficiaire.C_sex,
                                            phone = QueryConjoint.beneficiaire.C_phone,
                                            CivilStatus = QueryConjoint.beneficiaire.C_statusmaritalk,
                                            datenaiss = QueryConjoint.beneficiaire.C_datenais,
                                            ID_Succursale = QueryConjoint.succursale.C_name,
                                            ID_Departement = QueryConjoint.departement.C_id_depart,
                                            Childs = ChildsPartner,
                                            picture = QueryConjoint.beneficiaire.C_picture,
                                            account_system = QueryConjoint.beneficiaire.C_status_system.Equals("1") ? "Enabled":"Disabled",
                                            Matricule=QueryConjoint.beneficiaire.C_mat,
                                            partner = new Partner
                                            {
                                                id = item.beneficiaire.C_id.ToString(),
                                                name = item.beneficiaire.C_name,
                                                sex = item.beneficiaire.C_sex,
                                                phone = item.beneficiaire.C_phone,
                                                CivilStatus = item.beneficiaire.C_statusmaritalk,
                                                datenaiss = item.beneficiaire.C_datenais,
                                                picture = item.beneficiaire.C_picture,
                                                Childs = ChildsConjoint,
                                                Matricule=item.beneficiaire.C_mat,
                                                account_system=item.beneficiaire.C_status_system.Equals("1")?"Enabled":"Disabled"

                                            }
                                        };

                                        ConjointList.Add(employed);
                                    }
                                    else
                                    {

                                    }

                                }

                            }
                        }
                    }
                }
            }

            

            JavaScriptSerializer serial = new JavaScriptSerializer();
            var dataJSON = serial.Serialize(ConjointList);
            return dataJSON.ToString();
                    //return Partner.ToList().Count.ToString();
        }
        private String getChildsForCommand(String name)
        {
            List<Models.Employed> ListEmployed = new List<Employed>();
            List<Models.Children> ListChildren = new List<Children>();
            if (Session["userinfo"]!=null)
            {
                var QueryChilds = from ds in dbContext.t_beneficiaires
                                  where ds.C_name.Contains(name) && !ds.C_id_parent.Equals(null) //&& (ds.C_statusChild.Equals("active") || ds.C_statusChild.Equals("reactive"))
                                  select ds;

                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    if (QueryChilds.ToList().Count > 0)
                    {
                        foreach (var child in QueryChilds)
                        {
                            int idParent = int.Parse(child.C_id_parent);
                            var QueryParent = (from parent in dbContext.t_beneficiaires
                                               join succursale in dbContext.t_succursales on parent.C_id_succ equals succursale.C_id
                                               //join departement in dbContext.t_departement on succursale.C_id equals departement.C_id_succ
                                               where parent.C_id.Equals(idParent) && parent.C_id_succ.Equals(auth.Succursale)
                                               select new { parent, succursale }).FirstOrDefault();
                            int iddep = (int)QueryParent.parent.C_id_depart;
                            String Depart = dbContext.t_departement.Where(dep => dep.C_id == iddep).FirstOrDefault().C_id_depart;
                            string idPartner = idParent.ToString();
                            var QueryPartner = (from ds in dbContext.t_beneficiaires
                                                where ds.C_id_partenaire.Equals(idPartner)
                                                orderby ds.C_id descending
                                                select ds).FirstOrDefault();


                            Models.Children children = new Children
                            {
                                id = child.C_id.ToString(),
                                name = child.C_name,
                                sex = child.C_sex,
                                datenais = child.C_datenais,
                                picture = child.C_picture,
                                status = child.C_statusChild,
                                parent = QueryParent.parent.C_id.ToString(),
                                account_system = child.C_status_system.Equals("1") ? "Enabled" : "Disabled"
                            };
                            ListChildren.Add(children);
                            Models.Employed employed = new Employed
                            {
                                id = QueryParent.parent.C_id.ToString(),
                                name = QueryParent.parent.C_name,
                                sex = QueryParent.parent.C_sex,
                                phone = QueryParent.parent.C_phone,
                                picture = QueryParent.parent.C_picture,
                                CivilStatus = QueryParent.parent.C_statusmaritalk,
                                datenaiss = QueryParent.parent.C_datenais,
                                ID_Succursale = QueryParent.succursale.C_name,
                                ID_Departement = Depart,
                                Childs = ListChildren,
                                Matricule = QueryParent.parent.C_mat,
                                account_system = QueryParent.parent.C_status_system.Equals("1") ? "Enabled" : "Disabled"
                            };
                            ListEmployed.Add(employed);
                        }
                    }
                }
                else
                {
                    if (QueryChilds.ToList().Count > 0)
                    {
                        foreach (var child in QueryChilds)
                        {
                            int idParent = int.Parse(child.C_id_parent);
                            var QueryParent = (from parent in dbContext.t_beneficiaires
                                               join succursale in dbContext.t_succursales on parent.C_id_succ equals succursale.C_id
                                             //  join departement in dbContext.t_departement on succursale.C_id equals departement.C_id_succ
                                               where parent.C_id.Equals(idParent)
                                               select new { parent, succursale}).FirstOrDefault();
                            int iddep =(int) QueryParent.parent.C_id_depart;
                            String Depart = dbContext.t_departement.Where(dep => dep.C_id == iddep).FirstOrDefault().C_id_depart;
                            string idPartner = idParent.ToString();
                            var QueryPartner = (from ds in dbContext.t_beneficiaires
                                                where ds.C_id_partenaire.Equals(idPartner)
                                                orderby ds.C_id descending
                                                select ds).FirstOrDefault();


                            Models.Children children = new Children
                            {
                                id = child.C_id.ToString(),
                                name = child.C_name,
                                sex = child.C_sex,
                                datenais = child.C_datenais,
                                picture = child.C_picture,
                                status = child.C_statusChild,
                                parent = QueryParent.parent.C_id.ToString(),
                                account_system=child.C_status_system.Equals("1")?"Enabled":"Disabled"
                            };
                            ListChildren.Add(children);
                            Models.Employed employed = new Employed
                            {
                                id = QueryParent.parent.C_id.ToString(),
                                name = QueryParent.parent.C_name,
                                sex = QueryParent.parent.C_sex,
                                phone = QueryParent.parent.C_phone,
                                picture = QueryParent.parent.C_picture,
                                CivilStatus = QueryParent.parent.C_statusmaritalk,
                                datenaiss = QueryParent.parent.C_datenais,
                                ID_Succursale = QueryParent.succursale.C_name,
                                ID_Departement = Depart,
                                Childs = ListChildren,
                                Matricule=QueryParent.parent.C_mat,
                                account_system=QueryParent.parent.C_status_system.Equals("1")?"Enabled":"Disabled"
                            };
                            ListEmployed.Add(employed);
                        }
                    }
                }
            }
            

            
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var data = serial.Serialize(ListEmployed);
            return data.ToString();
        }
       [Route("{getlistvoucherdependents}")]
        public String SearchVoucherDependents()
        {
            List<BonCommand> lstVoucher = new List<BonCommand>();
            if (Session["userinfo"]!=null)
            {
                
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                { 
                      int id = int.Parse(auth.Succursale);
                      var Query = from beneficiairies in dbContext.t_beneficiaires
                                  join voucher in dbContext.t_bon_commandes
                                  on beneficiairies.C_id equals voucher.C_id_bene
                                  join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                  join Facturation in dbContext.t_factures on voucher.C_id_bon equals Facturation.C_id_bon
                                  into Facturation
                                  from SubFacture in Facturation.DefaultIfEmpty()
                                  where beneficiairies.C_id_succ.Equals(null) && beneficiairies.C_id_visitor.Equals(null) && SubFacture.C_cout.Equals(null)
                                  select new { beneficiairies, voucher, hospital };

                foreach (var item in Query)
                {
                    t_beneficiaires employee = new t_beneficiaires();
                    if (item.beneficiairies.C_id_partenaire != null) 
                    {
                        int idPart = int.Parse(item.beneficiairies.C_id_partenaire);
                        employee = dbContext.t_beneficiaires.Where(Employee => Employee.C_id.Equals(idPart)).FirstOrDefault();
                    } 
                    else
                    {
                        int idParent = int.Parse(item.beneficiairies.C_id_parent);
                        employee = dbContext.t_beneficiaires.Where(Employee => Employee.C_id.Equals(idParent)).FirstOrDefault();
                    }
                    if (employee.C_id_succ.Equals(auth.Succursale))
                    {
                        
                    BonCommand cmd = new BonCommand
                    {

                        id = item.voucher.C_id_bon,
                        idBene = item.beneficiairies.C_id,
                        idHealth = item.hospital.C_name,
                        nameAuthor = item.beneficiairies.C_name,
                        motif = item.voucher.C_motif,
                        sexeAuthor = item.beneficiairies.C_sex,
                        nameDoctor = item.voucher.C_namedoctor,
                        datecmd = item.voucher.C_datedeb,
                        Employed = new Employed
                        {
                            name = employee.C_name,
                            ID_Succursale = dbContext.t_succursales.Where(company => company.C_id.Equals(employee.C_id_succ)).FirstOrDefault().C_name,
                            ID_Departement = dbContext.t_departement.Where(department => department.C_id == (int?)employee.C_id_depart).FirstOrDefault().C_id_depart
                        }
                    };
                    lstVoucher.Add(cmd);
                    }
                }
                }
                else
                {
                    var Query = from beneficiairies in dbContext.t_beneficiaires
                                join voucher in dbContext.t_bon_commandes
                                on beneficiairies.C_id equals voucher.C_id_bene
                                join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                join Facturation in dbContext.t_factures on voucher.C_id_bon equals Facturation.C_id_bon
                                into Facturation from SubFacture in Facturation.DefaultIfEmpty()
                                where beneficiairies.C_id_succ.Equals(null) && beneficiairies.C_id_visitor.Equals(null) && SubFacture.C_cout.Equals(null)
                                select new { beneficiairies, voucher, hospital };

                    foreach (var item in Query)
                    {
                        t_beneficiaires employee = new t_beneficiaires();
                        if (item.beneficiairies.C_id_partenaire != null)
                        {
                            int idPart = int.Parse(item.beneficiairies.C_id_partenaire);
                            employee = dbContext.t_beneficiaires.Where(Employee => Employee.C_id.Equals(idPart)).FirstOrDefault();
                        }
                        else
                        {
                            int idParent = int.Parse(item.beneficiairies.C_id_parent);
                            employee = dbContext.t_beneficiaires.Where(Employee => Employee.C_id.Equals(idParent)).FirstOrDefault();
                        }
                        BonCommand cmd = new BonCommand
                        {
                          
                            id = item.voucher.C_id_bon,
                            idBene = item.beneficiairies.C_id,
                            idHealth = item.hospital.C_name,
                            nameAuthor =item.beneficiairies.C_name,
                            motif = item.voucher.C_motif,
                            sexeAuthor = item.beneficiairies.C_sex,
                            nameDoctor = item.voucher.C_namedoctor,
                            datecmd = item.voucher.C_datedeb,
                            Employed = new Employed
                            {
                                name = employee.C_name,
                                ID_Succursale = dbContext.t_succursales.Where(company => company.C_id.Equals(employee.C_id_succ)).FirstOrDefault().C_name,
                                ID_Departement = dbContext.t_departement.Where(department => department.C_id == (int?)employee.C_id_depart).FirstOrDefault().C_id_depart
                            }
                        };
                        lstVoucher.Add(cmd);
                    }
                }
            }
            
            JavaScriptSerializer serial = new JavaScriptSerializer();
            return serial.Serialize(lstVoucher).ToString();
        }

       [Route("getlistvouchervisitor")]
       public String SearchVoucherVisitor()
       {
           List<BonCommand> lstVoucher = new List<BonCommand>();
           if (Session["userinfo"]!=null)
           {
               Authenticate auth = (Authenticate)Session["userinfo"];
               if (auth.Priority.Equals("user"))
               {
                   int id = int.Parse(auth.Succursale);
                   var Query = from beneficiairies in dbContext.t_beneficiaires
                               join voucher in dbContext.t_bon_commandes
                               on beneficiairies.C_id equals voucher.C_id_bene
                               join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                               join Facturation in dbContext.t_factures on voucher.C_id_bon equals Facturation.C_id_bon
                               into Facturation
                               from SubFacture in Facturation.DefaultIfEmpty()
                               where beneficiairies.C_id_visitor.Equals(auth.Succursale) && SubFacture.C_cout.Equals(null)   
                               select new { beneficiairies, voucher, hospital };

                   foreach (var item in Query)
                   {
                       
                       BonCommand cmd = new BonCommand
                       {

                           id = item.voucher.C_id_bon,
                           idBene = item.beneficiairies.C_id,
                           idHealth = item.hospital.C_name,
                           nameAuthor = item.beneficiairies.C_name,
                           motif = item.voucher.C_motif,
                           sexeAuthor = item.beneficiairies.C_sex,
                           nameDoctor = item.voucher.C_namedoctor,
                           datecmd = item.voucher.C_datedeb,
                           Employed = new Employed
                           {
                               //name =null,
                               ID_Succursale = dbContext.t_succursales.Where(company => company.C_id.Equals(item.beneficiairies.C_id_visitor)).FirstOrDefault().C_name,
                             //  ID_Departement = dbContext.t_departement.Where(department => department.C_id == (int?)employee.C_id_depart).FirstOrDefault().C_id_depart
                           }
                       };
                       lstVoucher.Add(cmd);
                   }
               }
               else
               {
                   var Query = from beneficiairies in dbContext.t_beneficiaires
                               join voucher in dbContext.t_bon_commandes
                               on beneficiairies.C_id equals voucher.C_id_bene
                               join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                               join Facturation in dbContext.t_factures on voucher.C_id_bon equals Facturation.C_id_bon
                               into Facturation
                               from SubFacture in Facturation.DefaultIfEmpty()
                               where !beneficiairies.C_id_visitor.Equals(null) && SubFacture.C_cout.Equals(null) 
                               select new { beneficiairies, voucher, hospital };

                   foreach (var item in Query)
                   {
                      
                       BonCommand cmd = new BonCommand
                       {

                           id = item.voucher.C_id_bon,
                           idBene = item.beneficiairies.C_id,
                           idHealth = item.hospital.C_name,
                           nameAuthor = item.beneficiairies.C_name,
                           motif = item.voucher.C_motif,
                           sexeAuthor = item.beneficiairies.C_sex,
                           nameDoctor = item.voucher.C_namedoctor,
                           datecmd = item.voucher.C_datedeb,
                        
                           Employed = new Employed
                           {
                              // name = employee.C_name,
                               ID_Succursale = dbContext.t_succursales.Where(company => company.C_id.Equals(item.beneficiairies.C_id_visitor)).FirstOrDefault().C_name,
                             //  ID_Departement = dbContext.t_departement.Where(department => department.C_id == (int?)employee.C_id_depart).FirstOrDefault().C_id_depart
                           }
                       };
                       lstVoucher.Add(cmd);
                   }
               }
           }
           JavaScriptSerializer jSerial = new JavaScriptSerializer();
           var data = jSerial.Serialize(lstVoucher);
           return data.ToString();
       }

       [Route("getlistvouchercasual")]
       public String SearchVoucherCasual()
       {
           List<BonCommand> lstVoucher = new List<BonCommand>();
           if (Session["userinfo"]!=null) 
           {
               Authenticate auth = (Authenticate)Session["userinfo"];
               if (auth.Priority.Equals("user"))
               {
                  
                   var Query = from casual in dbContext.t_vouchers_casuals
                               join company in dbContext.t_succursales
                               on casual.C_id_company equals company.C_id
                               join hospital in dbContext.t_centre_soins
                               on casual.C_id_centre equals hospital.C_id_centre
                               where casual.C_id_company.Equals(auth.Succursale) && casual.C_cout.Equals(null)
                               select new { casual, company, hospital };

                   foreach (var item in Query)
                   {
                       BonCommand voucher = new BonCommand
                       {
                           id=item.casual.C_id_voucher,
                           nameAuthor=item.casual.C_name_casual,
                           datecmd=item.casual.C_date_casual,
                           sexeAuthor=item.casual.C_company_casual,
                           idHealth=item.hospital.C_name,
                           Employed = new Employed
                           {
                               ID_Succursale=auth.nameSuccursale
                           }
                       };
                       lstVoucher.Add(voucher);
                   }

               }
               else
               {
                   var Query = from casual in dbContext.t_vouchers_casuals
                               join company in dbContext.t_succursales
                               on casual.C_id_company equals company.C_id
                               join hospital in dbContext.t_centre_soins
                               on casual.C_id_centre equals hospital.C_id_centre
                               where casual.C_cout.Equals(null)
                               select new { casual, company, hospital };

                   foreach (var item in Query)
                   {
                       BonCommand voucher = new BonCommand
                       {
                           id = item.casual.C_id_voucher,
                           nameAuthor = item.casual.C_name_casual,
                           datecmd = item.casual.C_date_casual,
                           sexeAuthor = item.casual.C_company_casual,
                           idHealth = item.hospital.C_name,
                           Employed = new Employed
                           {
                               ID_Succursale = item.company.C_name
                           }
                       };
                       lstVoucher.Add(voucher);
                   }
               }
           }

           return new JavaScriptSerializer().Serialize(lstVoucher).ToString();
       }

        [Route("getlistvouchercontractor")]
       public String SearchVoucherContractor()
       {
           List<BonCommand> lstVouchers = new List<BonCommand>();
           if (Session["userinfo"]!=null)
           {
               Authenticate auth = (Authenticate)Session["userinfo"];
               if (auth.Priority.Equals("user"))
               {
                   var Query = from company in dbContext.t_succursales
                               join contractor in dbContext.t_contractor
                               on company.C_id equals contractor.C_idSucc
                               join employeeContractor in dbContext.employee_contractor
                               on contractor.C_id equals employeeContractor.C_idContractor
                               join voucherContractor in dbContext.t_vouchers_contractor
                               on employeeContractor.C_id equals voucherContractor.C_id_Employed
                               join Hospital in dbContext.t_centre_soins
                               on voucherContractor.C_id_centre equals Hospital.C_id_centre
                               where company.C_id.Equals(auth.Succursale) && voucherContractor.C_cout.Equals(null)
                               select new { company, contractor, employeeContractor,voucherContractor,Hospital };

                   foreach (var item in Query)
                   {
                       BonCommand voucher = new BonCommand
                       {
                           id=item.voucherContractor.C_id_voucher,
                           idHealth=item.Hospital.C_name,
                           nameAuthor=item.employeeContractor.C_name,
                           datecmd=item.voucherContractor.C_datedeb,
                           sexeAuthor=item.contractor.C_name,
                           Employed = new Employed
                           {
                               ID_Succursale=auth.nameSuccursale
                           }
                       };
                       lstVouchers.Add(voucher);
                   }
               }
               else
               {
                   var Query = from company in dbContext.t_succursales
                               join contractor in dbContext.t_contractor
                               on company.C_id equals contractor.C_idSucc
                               join employeeContractor in dbContext.employee_contractor
                               on contractor.C_id equals employeeContractor.C_idContractor
                               join voucherContractor in dbContext.t_vouchers_contractor
                               on employeeContractor.C_id equals voucherContractor.C_id_Employed
                               join Hospital in dbContext.t_centre_soins
                               on voucherContractor.C_id_centre equals Hospital.C_id_centre
                               where voucherContractor.C_cout.Equals(null)
                               select new { company, contractor, employeeContractor, voucherContractor, Hospital };

                   foreach (var item in Query)
                   {
                       BonCommand voucher = new BonCommand
                       {
                           id = item.voucherContractor.C_id_voucher,
                           idHealth = item.Hospital.C_name,
                           nameAuthor = item.employeeContractor.C_name,
                           datecmd = item.voucherContractor.C_datedeb,
                           sexeAuthor = item.contractor.C_name,
                           Employed = new Employed
                           {
                               ID_Succursale = auth.nameSuccursale
                           }
                       };
                       lstVouchers.Add(voucher);
                   }
               }
           }

           return new JavaScriptSerializer().Serialize(lstVouchers).ToString();
       }

      // [AllowAnonymous]
       [Route("getlistbcommand")]
        public String SearchBonCommand()
        {
            int ctr = 0;
            List<Models.BonCommand> ListBCmd = new List<BonCommand>();
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    var QueryCmd = from beneficiaire in dbContext.t_beneficiaires
                           join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                           join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                           join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                           join centerHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals centerHealth.C_id_centre
                           join facturation in dbContext.t_factures on bonCommand.C_id_bon equals facturation.C_id_bon
                           into facturation from subfacturation in facturation.DefaultIfEmpty()
                           where beneficiaire.C_id_succ.Equals(auth.Succursale) && subfacturation.C_cout.Equals(null)
                           select new { beneficiaire, succursale, departement,bonCommand,centerHealth };

            if (QueryCmd.ToList().Count>0)
            {
                foreach (var bCmd in QueryCmd)
                {
                    BonCommand BCmdUnity = new BonCommand
                    {
                        id=bCmd.bonCommand.C_id_bon,
                        idBene=bCmd.beneficiaire.C_id,
                        idHealth=bCmd.centerHealth.C_name,
                        datecmd=bCmd.bonCommand.C_datedeb,
                        dateValidation=bCmd.bonCommand.C_datefin,
                        approuve=bCmd.bonCommand.C_approuve,
                        motif=bCmd.bonCommand.C_motif,
                        nameDoctor=bCmd.bonCommand.C_namedoctor,
                        Employed = new Employed
                        {
                            id=bCmd.beneficiaire.C_id.ToString(),
                            name=bCmd.beneficiaire.C_name,
                            sex=bCmd.beneficiaire.C_sex,
                            CivilStatus=bCmd.beneficiaire.C_statusmaritalk,
                            phone=bCmd.beneficiaire.C_phone,
                            ID_Succursale=bCmd.succursale.C_name,
                            ID_Departement=bCmd.departement.C_id_depart,
                            picture=bCmd.beneficiaire.C_picture,
                            datenaiss=bCmd.beneficiaire.C_datenais,

                        }

                    };
                    ListBCmd.Add(BCmdUnity);
                }
            }
            else
            {
                var QueryCmd2 = from beneficiaire in dbContext.t_beneficiaires
                          // join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                           //join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                           join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                           join centerHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals centerHealth.C_id_centre
                           where !beneficiaire.C_id_partenaire.Equals(null) && beneficiaire.C_id_succ.Equals(auth.Succursale)
                           select new { beneficiaire,bonCommand, centerHealth };

                foreach (var bCmd in QueryCmd2)
                {
                    string idPartner=bCmd.beneficiaire.C_id.ToString();
                    var QueryEmployed = (from ds in dbContext.t_beneficiaires
                                         join succursale in dbContext.t_succursales on ds.C_id_succ equals succursale.C_id
                                         join departement in dbContext.t_departement on ds.C_id_depart equals departement.C_id
                                        where ds.C_id_partenaire.Equals(idPartner)
                                        select new {ds,succursale,departement}).FirstOrDefault();

                   
                    BonCommand BCmdUnity = new BonCommand
                    {
                        id = bCmd.bonCommand.C_id_bon,
                        idBene = bCmd.beneficiaire.C_id,
                        idHealth = bCmd.centerHealth.C_name,
                        datecmd = bCmd.bonCommand.C_datedeb,
                        dateValidation = bCmd.bonCommand.C_datefin,
                        approuve = bCmd.bonCommand.C_approuve,
                        motif = bCmd.bonCommand.C_motif,
                        nameDoctor = bCmd.bonCommand.C_namedoctor,
                        Partner = new Partner
                        {
                            id = bCmd.beneficiaire.C_id.ToString(),
                            name = bCmd.beneficiaire.C_name,
                            sex = bCmd.beneficiaire.C_sex,
                            CivilStatus = bCmd.beneficiaire.C_statusmaritalk,
                            phone = bCmd.beneficiaire.C_phone,
                            picture = bCmd.beneficiaire.C_picture,
                            datenaiss = bCmd.beneficiaire.C_datenais,
                            partner = new Partner
                            {
                                id=QueryEmployed.ds.C_id.ToString(),
                                name=QueryEmployed.ds.C_name,
                                sex=QueryEmployed.ds.C_sex,
                                phone=QueryEmployed.ds.C_phone,
                                CivilStatus=QueryEmployed.ds.C_statusmaritalk,
                                picture=QueryEmployed.ds.C_picture,
                                datenaiss=QueryEmployed.ds.C_datenais,
                                ID_Succursale=QueryEmployed.succursale.C_name,
                                ID_Departement=QueryEmployed.departement.C_id_depart
                            }

                        }

                    };
                    ListBCmd.Add(BCmdUnity);
                }
               
                if (QueryCmd2.ToList().Count<1)
                {
                    var QueryCmd3 = from beneficiaire in dbContext.t_beneficiaires
                          // join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                           //join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                           join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                           join centerHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals centerHealth.C_id_centre
                           where !beneficiaire.C_id_parent.Equals(null) && beneficiaire.C_id_succ.Equals(auth.Succursale)
                           select new { beneficiaire,bonCommand, centerHealth };

                    ctr=QueryCmd3.ToList().Count;
                    foreach (var bCmd in QueryCmd3)
                    {
                        int idChild = int.Parse(bCmd.beneficiaire.C_id_parent);
                        var QueryEmployed = (from employed in dbContext.t_beneficiaires
                                             join succursale in dbContext.t_succursales on employed.C_id_succ equals succursale.C_id
                                             join departement in dbContext.t_departement on employed.C_id_depart equals departement.C_id
                                             where employed.C_id.Equals(idChild) && employed.C_id_succ.Equals(auth.Succursale)
                                             select new { employed, succursale, departement }).FirstOrDefault();
                        BonCommand BCmdUnity = new BonCommand
                        {
                            id = bCmd.bonCommand.C_id_bon,
                            idBene = bCmd.beneficiaire.C_id,
                            idHealth = bCmd.centerHealth.C_name,
                            datecmd = bCmd.bonCommand.C_datedeb,
                            dateValidation = bCmd.bonCommand.C_datefin,
                            approuve = bCmd.bonCommand.C_approuve,
                            motif = bCmd.bonCommand.C_motif,
                            nameDoctor = bCmd.bonCommand.C_namedoctor,
                            Child = new Children
                            {
                                id = bCmd.beneficiaire.C_id.ToString(),
                                name = bCmd.beneficiaire.C_name,
                                sex = bCmd.beneficiaire.C_sex,
                                status = bCmd.beneficiaire.C_statusChild,
                                picture = bCmd.beneficiaire.C_picture,
                                datenais = bCmd.beneficiaire.C_datenais,


                            },
                            Employed = new Employed
                            {
                                id = QueryEmployed.employed.C_id.ToString(),
                                name = QueryEmployed.employed.C_name,
                                sex = QueryEmployed.employed.C_sex,
                                phone = QueryEmployed.employed.C_phone,
                                CivilStatus = QueryEmployed.employed.C_statusmaritalk,
                                picture = QueryEmployed.employed.C_picture,
                                datenaiss = QueryEmployed.employed.C_datenais,
                                ID_Succursale = QueryEmployed.succursale.C_name,
                                ID_Departement = QueryEmployed.departement.C_id_depart
                            }



                        };
                        ListBCmd.Add(BCmdUnity);
                    }
                }
            }
                }
                else
                {
                    var QueryCmd = from beneficiaire in dbContext.t_beneficiaires
                                   join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                   join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                   join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                                   join centerHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals centerHealth.C_id_centre
                                   join facturation in dbContext.t_factures on bonCommand.C_id_bon equals facturation.C_id_bon
                                   into facturation
                                   from subfacturation in facturation.DefaultIfEmpty()
                                   where subfacturation.C_cout.Equals(null)
                                   select new { beneficiaire, succursale, departement, bonCommand, centerHealth };

                    if (QueryCmd.ToList().Count > 0)
                    {
                        foreach (var bCmd in QueryCmd)
                        {
                            BonCommand BCmdUnity = new BonCommand
                            {
                                id = bCmd.bonCommand.C_id_bon,
                                idBene = bCmd.beneficiaire.C_id,
                                idHealth = bCmd.centerHealth.C_name,
                                datecmd = bCmd.bonCommand.C_datedeb,
                                dateValidation = bCmd.bonCommand.C_datefin,
                                approuve = bCmd.bonCommand.C_approuve,
                                motif = bCmd.bonCommand.C_motif,
                                nameDoctor = bCmd.bonCommand.C_namedoctor,
                                Employed = new Employed
                                {
                                    id = bCmd.beneficiaire.C_id.ToString(),
                                    name = bCmd.beneficiaire.C_name,
                                    sex = bCmd.beneficiaire.C_sex,
                                    CivilStatus = bCmd.beneficiaire.C_statusmaritalk,
                                    phone = bCmd.beneficiaire.C_phone,
                                    ID_Succursale = bCmd.succursale.C_name,
                                    ID_Departement = bCmd.departement.C_id_depart,
                                    picture = bCmd.beneficiaire.C_picture,
                                    datenaiss = bCmd.beneficiaire.C_datenais,

                                }

                            };
                            ListBCmd.Add(BCmdUnity);
                        }
                    }
                    else
                    {
                        var QueryCmd2 = from beneficiaire in dbContext.t_beneficiaires
                                        // join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                        //join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                        join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                                        join centerHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals centerHealth.C_id_centre
                                        where !beneficiaire.C_id_partenaire.Equals(null)
                                        select new { beneficiaire, bonCommand, centerHealth };

                        foreach (var bCmd in QueryCmd2)
                        {
                            string idPartner = bCmd.beneficiaire.C_id.ToString();
                            var QueryEmployed = (from ds in dbContext.t_beneficiaires
                                                 join succursale in dbContext.t_succursales on ds.C_id_succ equals succursale.C_id
                                                 join departement in dbContext.t_departement on ds.C_id_depart equals departement.C_id
                                                 where ds.C_id_partenaire.Equals(idPartner)
                                                 select new { ds, succursale, departement }).FirstOrDefault();
                            if (!String.IsNullOrEmpty(bCmd.beneficiaire.C_name))
                            {
                                                          BonCommand BCmdUnity = new BonCommand
                            {
                                id = bCmd.bonCommand.C_id_bon,
                                idBene = bCmd.beneficiaire.C_id,
                                idHealth = bCmd.centerHealth.C_name,
                                datecmd = bCmd.bonCommand.C_datedeb,
                                dateValidation = bCmd.bonCommand.C_datefin,
                                approuve = bCmd.bonCommand.C_approuve,
                                motif = bCmd.bonCommand.C_motif,
                                nameDoctor = bCmd.bonCommand.C_namedoctor,
                                Partner = new Partner
                                {
                                    id = bCmd.beneficiaire.C_id.ToString(),
                                    name = bCmd.beneficiaire.C_name,
                                    sex = bCmd.beneficiaire.C_sex,
                                    CivilStatus = bCmd.beneficiaire.C_statusmaritalk,
                                    phone = bCmd.beneficiaire.C_phone,
                                    picture = bCmd.beneficiaire.C_picture,
                                    datenaiss = bCmd.beneficiaire.C_datenais,
                                    partner = new Partner
                                    {
                                        id = QueryEmployed.ds.C_id.ToString(),
                                        name = QueryEmployed.ds.C_name,
                                        sex = QueryEmployed.ds.C_sex,
                                        phone = QueryEmployed.ds.C_phone,
                                        CivilStatus = QueryEmployed.ds.C_statusmaritalk,
                                        picture = QueryEmployed.ds.C_picture,
                                        datenaiss = QueryEmployed.ds.C_datenais,
                                        ID_Succursale = QueryEmployed.succursale.C_name,
                                        ID_Departement = QueryEmployed.departement.C_id_depart
                                    }

                                }

                            };
                            ListBCmd.Add(BCmdUnity);
  
                            }

                        }

                        if (QueryCmd2.ToList().Count < 1)
                        {
                            var QueryCmd3 = from beneficiaire in dbContext.t_beneficiaires
                                            // join succursale in dbContext.t_succursales on beneficiaire.C_id_succ equals succursale.C_id
                                            //join departement in dbContext.t_departement on beneficiaire.C_id_depart equals departement.C_id
                                            join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                                            join centerHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals centerHealth.C_id_centre
                                            where !beneficiaire.C_id_parent.Equals(null)
                                            select new { beneficiaire, bonCommand, centerHealth };

                            ctr = QueryCmd3.ToList().Count;
                            foreach (var bCmd in QueryCmd3)
                            {
                                int idChild = int.Parse(bCmd.beneficiaire.C_id_parent);
                                var QueryEmployed = (from employed in dbContext.t_beneficiaires
                                                     join succursale in dbContext.t_succursales on employed.C_id_succ equals succursale.C_id
                                                     join departement in dbContext.t_departement on employed.C_id_depart equals departement.C_id
                                                     where employed.C_id.Equals(idChild)
                                                     select new { employed, succursale, departement }).FirstOrDefault();
                                BonCommand BCmdUnity = new BonCommand
                                {
                                    id = bCmd.bonCommand.C_id_bon,
                                    idBene = bCmd.beneficiaire.C_id,
                                    idHealth = bCmd.centerHealth.C_name,
                                    datecmd = bCmd.bonCommand.C_datedeb,
                                    dateValidation = bCmd.bonCommand.C_datefin,
                                    approuve = bCmd.bonCommand.C_approuve,
                                    motif = bCmd.bonCommand.C_motif,
                                    nameDoctor = bCmd.bonCommand.C_namedoctor,
                                    Child = new Children
                                    {
                                        id = bCmd.beneficiaire.C_id.ToString(),
                                        name = bCmd.beneficiaire.C_name,
                                        sex = bCmd.beneficiaire.C_sex,
                                        status = bCmd.beneficiaire.C_statusChild,
                                        picture = bCmd.beneficiaire.C_picture,
                                        datenais = bCmd.beneficiaire.C_datenais,


                                    },
                                    Employed = new Employed
                                    {
                                        id = QueryEmployed.employed.C_id.ToString(),
                                        name = QueryEmployed.employed.C_name,
                                        sex = QueryEmployed.employed.C_sex,
                                        phone = QueryEmployed.employed.C_phone,
                                        CivilStatus = QueryEmployed.employed.C_statusmaritalk,
                                        picture = QueryEmployed.employed.C_picture,
                                        datenaiss = QueryEmployed.employed.C_datenais,
                                        ID_Succursale = QueryEmployed.succursale.C_name,
                                        ID_Departement = QueryEmployed.departement.C_id_depart
                                    }



                                };
                                ListBCmd.Add(BCmdUnity);
                            }
                        }
                    }
                }
            }
            
             
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var data = serial.Serialize(ListBCmd);
            return data.ToString();

        }
        [HttpPost]
        public String getCSV()
        {
            String returnValue = "";
            int idBon = 0;
            decimal cout = 0.0M;

            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {

            using (System.IO.StreamReader sReader=new System.IO.StreamReader(Request.InputStream))
            {
                
                String Ligne="";
                while ((Ligne = sReader.ReadLine())!=null)
                {
                    String[] SplitterData = Ligne.Split(',');
                    idBon = int.Parse(SplitterData[0]);
                    cout = decimal.Parse(SplitterData[1]);
                    var QueryFindBeneficiare = (from beneficiaire in dbContext.t_beneficiaires
                                                join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                                                where bonCommand.C_id_bon.Equals(idBon) && beneficiaire.C_id_succ.Equals(auth.Succursale)
                                                select bonCommand).FirstOrDefault();


                    if (QueryFindBeneficiare != null)
                    {
                        Models.t_factures facture = new t_factures
                        {
                            C_id_bon=idBon,
                            C_datefacture = String.Format("{0}/{1}/{2}", DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()),
                            C_timefacture = String.Format("{0}:{1}:{2}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString()),
                            C_cout = cout
                        };
                        dbContext.t_factures.Add(facture);
                        dbContext.SaveChanges();
                        returnValue = "200";
                    }
                }
            }
                }
                else
                {
                    using (System.IO.StreamReader sReader = new System.IO.StreamReader(Request.InputStream))
                    {

                        String Ligne = "";
                        while ((Ligne = sReader.ReadLine()) != null)
                        {
                            String[] SplitterData = Ligne.Split(',');
                            idBon = int.Parse(SplitterData[0]);
                            cout = decimal.Parse(SplitterData[1]);
                            var QueryFindBeneficiare = (from beneficiaire in dbContext.t_beneficiaires
                                                        join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                                                        where bonCommand.C_id_bon.Equals(idBon)
                                                        select bonCommand).FirstOrDefault();


                            if (QueryFindBeneficiare != null)
                            {
                                Models.t_factures facture = new t_factures
                                {
                                    C_id_bon = idBon,
                                    C_datefacture = String.Format("{0}/{1}/{2}", DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()),
                                    C_timefacture = String.Format("{0}:{1}:{2}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString()),
                                    C_cout = cout
                                };
                                dbContext.t_factures.Add(facture);
                                dbContext.SaveChanges();
                                returnValue = "200";
                            }
                        }
                    }
                }
            }
            return returnValue;
        }
        public ActionResult ReportingSystem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportingSystem(String data)
        {

            return View();
        }
        public ActionResult ViewPrintableCommand()
        {
            return View();
        }
        public ActionResult AddFacture()
        {
            return View();
        }
        [HttpPost]
        public Object AddFacture(Models.t_factures facture)
        {
            try
            {
                Facture factured = new Facture();
                //  facture.C_fileupload = factured.FileConverter(facture.C_fileupload, Server.MapPath("~/factures"));
                //factured.DocumentFacture;
                var data = facture.C_fileupload;
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<FactureCmd> listCout = serialize.Deserialize<List<FactureCmd>>(data);
                if (listCout.Count>0)
                {
                    String Categorie=listCout[0].Categorie;
                    if (Categorie.Equals("Employee") || Categorie.Equals("Dependents") || Categorie.Equals("Visitor"))
                    {
                        foreach (var item in listCout)
                        {

                            facture.C_id_bon = item.id;
                            facture.C_datefacture = String.Format("{0}/{1}/{2}", DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString());
                            facture.C_timefacture = String.Format("{0}:{1}:{2}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString());
                            facture.C_cout = (decimal)item.cout;
                            facture.C_fileupload = "";
                            dbContext.t_factures.Add(facture);
                            dbContext.SaveChanges();
                        }
                    }
                    else if (Categorie.Equals("Casual"))
                    {
                        foreach (var item in listCout)
                           {
                               var QueryVoucherCasualObject = (from ds in dbContext.t_vouchers_casuals
                                                    where ds.C_id_voucher == item.id
                                                    select ds).FirstOrDefault();

                               QueryVoucherCasualObject.C_cout =(decimal?) item.cout;
                               dbContext.SaveChanges();
                              
                           }
                    }
                    else
                    {
                        foreach (var item in listCout)
                        {
                            var QueryContractorObject = (from ds in dbContext.t_vouchers_contractor
                                                         where ds.C_id_voucher == item.id
                                                         select ds).FirstOrDefault();

                            QueryContractorObject.C_cout = (decimal)item.cout;
                            dbContext.SaveChanges();
                        }
                    }
                }
                //Authenticate auth = (Authenticate)Session["userinfo"];
                //T_logs logs = new T_logs()
                //{
                //    C_user = auth.username,
                //    C_date = DateTime.Now.ToShortDateString(),
                //    C_time = DateTime.Now.ToShortTimeString(),
                //    C_action = "Add new",
                //    C_object = "Invoices",
                //    C_company = auth.nameSuccursale,
                //    C_mat = dbContext.t_beneficiaires.Where(id => id.C_id == auth.id).FirstOrDefault().C_mat

                //};
                //dbContext.T_logs.Add(logs);
                //dbContext.SaveChanges();
                ViewBag.messageFacture = "200";
            }
            catch (Exception ex)
            {
                ViewBag.messageFacture = ex.Message;
                
            }
            return View();
        }
        public String getReportingSystem()
        {

            Models.RequestEmployed ReqEmployed = new RequestEmployed();
            List<Employed>lst=ReqEmployed.getListEmployed();
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var data = serial.Serialize(lst);
            return data.ToString();
        }

        private bool StatusChlidren()
        {
            try
            {
                int yearCurrent = DateTime.Now.Year;
                var Query = from ds in dbContext.t_beneficiaires
                            where !ds.C_id_parent.Equals(null)
                            select ds;

                foreach (var item in Query)
                {
                    int yearBorn=int.Parse((item.C_datenais.Split('/')[2].ToString()));

                    if ((yearCurrent - yearBorn) >= 18 || (yearCurrent - yearBorn)<=25)
                    {
                        item.C_statusChild = "desactive";
                    }
                }
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                
                
            }
            return true;
        }
        [AllowAnonymous]
        public ActionResult ViewerBuffer()
        {
            return View();
        }

        [Route("exportation")]
        [HttpPost]

        public String ExportData()
        {
            
            String Linker = String.Empty;
            String FileName = String.Empty;
            using (System.IO.StreamReader Strm = new System.IO.StreamReader(Request.InputStream))
            {
               // this._ExportProcess(Strm.ReadToEnd(), ref Linker,ref FileName, typeExportion.CSV);
                var data = Strm.ReadToEnd();
                FileName = String.Format("{0}.csv", DateTime.Now.Millisecond);
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "/export/" + FileName;
                System.IO.File.AppendAllText(filepath,data);
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = FileName,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());
                HttpContext.Cache["contentType"] = "csv";
                HttpContext.Cache["FileDownload"] = filedata;
                return "exportFile";
           }

        }

        [HttpPost]
       // [AllowAnonymous]
        public String ParserDatas()
        {
            StreamReader sReader = new StreamReader(Request.InputStream);
            var data = sReader.ReadToEnd();
            JavaScriptSerializer serial=new JavaScriptSerializer();
            List<PrintTableCmd> ListPrintTable = serial.Deserialize<List<PrintTableCmd>>(data);
            return ListPrintTable.Count.ToString();
        }

   
        [HttpPost]
        [Route("exportPDF")]
        
        public String exportPDF()
        {
            using (System.IO.StreamReader sReader=new StreamReader(Request.InputStream))
            {
                var data = sReader.ReadToEnd();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<PrintTableCmd> ListPrintcmd = serialize.Deserialize<List<PrintTableCmd>>(data);
                String fileName = String.Format("{0}.pdf", DateTime.Now.Millisecond);
                String realFilename = AppDomain.CurrentDomain.BaseDirectory + "/export/" + fileName;
                String pathLogo = AppDomain.CurrentDomain.BaseDirectory + "Images/logo.png";
                Document _document = new Document(PageSize.A4.Rotate());
                PdfWriter.GetInstance(_document, new System.IO.FileStream(realFilename, FileMode.Create));
                _document.AddCreationDate(); 
                _document.Open();
                Paragraph header_img = new Paragraph();
                Paragraph header_text = new Paragraph();
                header_text.Alignment = Element.ALIGN_CENTER;
                String categ = ListPrintcmd.FirstOrDefault().Category;
                if (categ.Equals("Employee"))
                {
                    
                Phrase text_header = new Phrase("\n\nLIST RECAP OF PURCHASE ORDERS\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK));
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
                img.Alignment = Element.ALIGN_LEFT;
                header_img.Add(img);
                header_text.Add(text_header);
                _document.Add(header_img);
                _document.Add(header_text);
                PdfPTable tableau = new PdfPTable(8);
             //   tableau.PaddingTop = 5f;
                Paragraph pgr = new Paragraph("RECAP OF PURCHASE ORDERS FOR EMPLOYEE");
                pgr.Alignment = Element.ALIGN_CENTER;
                pgr.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                PdfPCell cellule = new PdfPCell(pgr);
                cellule.Colspan = 8;
                cellule.HorizontalAlignment = Element.ALIGN_CENTER;
                tableau.AddCell(cellule);
                tableau.AddCell("ID Bon ");
                tableau.AddCell("DateCommand");
                tableau.AddCell("Name");
                tableau.AddCell("Sex");
                tableau.AddCell("Phone");
                tableau.AddCell("Company");
                tableau.AddCell("Department");
                tableau.AddCell("Hospital");

                foreach (var item in ListPrintcmd)
                {
                    tableau.AddCell(item.idBon);
                    tableau.AddCell(item.datecmd);
                    tableau.AddCell(item.nameEmployed);
                    tableau.AddCell(item.sex);
                    tableau.AddCell(item.phone);
                    tableau.AddCell(item.company);
                    tableau.AddCell(item.department);
                    tableau.AddCell(item.Health);
                }
                
                tableau.TotalWidth = 750;
                tableau.LockedWidth = true;
                _document.Add(tableau);
                _document.CloseDocument();
                }
                if (categ.Equals("Dependents"))
                {
                      Phrase text_header = new Phrase("\n\nLIST RECAP OF PURCHASE ORDERS\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK));
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
                img.Alignment = Element.ALIGN_LEFT;
                header_img.Add(img);
                header_text.Add(text_header);
                _document.Add(header_img);
                _document.Add(header_text);
                PdfPTable tableau = new PdfPTable(8);
             //   tableau.PaddingTop = 5f;
                Paragraph pgr = new Paragraph("RECAP OF PURCHASE ORDERS FOR DEPENDENTS");
                pgr.Alignment = Element.ALIGN_CENTER;
                pgr.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                PdfPCell cellule = new PdfPCell(pgr);
                cellule.Colspan = 8;
                cellule.HorizontalAlignment = Element.ALIGN_CENTER;
                tableau.AddCell(cellule);
                tableau.AddCell("ID Voucher ");
                tableau.AddCell("Date Voucher");
                tableau.AddCell("Name Dependent");
                tableau.AddCell("Sex");
                tableau.AddCell("Name Employee");
                tableau.AddCell("Company");
                tableau.AddCell("Department");
                tableau.AddCell("Hospital");

                foreach (var item in ListPrintcmd)
                {
                    tableau.AddCell(item.idBon);
                    tableau.AddCell(item.datecmd);
                    tableau.AddCell(item.nameAuthor);
                    tableau.AddCell(item.sex);
                    tableau.AddCell(item.nameEmployed);
                    tableau.AddCell(item.company);
                    tableau.AddCell(item.department);
                    tableau.AddCell(item.Health);
                }
                
                tableau.TotalWidth = 750;
                tableau.LockedWidth = true;
                _document.Add(tableau);
                _document.CloseDocument();
                }
                if (categ.Equals("Visitor"))
                {
                    Phrase text_header = new Phrase("\n\nLIST RECAP OF PURCHASE ORDERS\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK));
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
                img.Alignment = Element.ALIGN_LEFT;
                header_img.Add(img);
                header_text.Add(text_header);
                _document.Add(header_img);
                _document.Add(header_text);
                PdfPTable tableau = new PdfPTable(6);
             //   tableau.PaddingTop = 5f;
                Paragraph pgr = new Paragraph("RECAP OF PURCHASE ORDERS FOR VISITORS");
                pgr.Alignment = Element.ALIGN_CENTER;
                pgr.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                PdfPCell cellule = new PdfPCell(pgr);
                cellule.Colspan = 6;
                cellule.HorizontalAlignment = Element.ALIGN_CENTER;
                tableau.AddCell(cellule);
                tableau.AddCell("ID Voucher ");
                tableau.AddCell("Date Voucher");
                tableau.AddCell("Visitor's Name");
                tableau.AddCell("Sex");
                tableau.AddCell("Company");
               
                tableau.AddCell("Hospital");

                foreach (var item in ListPrintcmd)
                {
                    tableau.AddCell(item.idBon);
                    tableau.AddCell(item.datecmd);
                    tableau.AddCell(item.nameAuthor);
                    tableau.AddCell(item.sex);
                    //tableau.AddCell(item.nameEmployed);
                    tableau.AddCell(item.company);
                    //tableau.AddCell(item.department);
                    tableau.AddCell(item.Health);
                }
                
                tableau.TotalWidth = 750;
                tableau.LockedWidth = true;
                _document.Add(tableau);
                _document.CloseDocument();
                }
                if (categ.Equals("Casual"))
                {
                    Phrase text_header = new Phrase("\n\nLIST RECAP OF PURCHASE ORDERS\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK));
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
                    img.Alignment = Element.ALIGN_LEFT;
                    header_img.Add(img);
                    header_text.Add(text_header);
                    _document.Add(header_img);
                    _document.Add(header_text);
                    PdfPTable tableau = new PdfPTable(6);
                    //   tableau.PaddingTop = 5f;
                    Paragraph pgr = new Paragraph("RECAP OF PURCHASE ORDERS FOR CASUAL");
                    pgr.Alignment = Element.ALIGN_CENTER;
                    pgr.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                    PdfPCell cellule = new PdfPCell(pgr);
                    cellule.Colspan = 6;
                    cellule.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableau.AddCell(cellule);
                    tableau.AddCell("ID Voucher ");
                    tableau.AddCell("Date Voucher");
                    tableau.AddCell("Casual's Name");
                    tableau.AddCell("Company's Casual");
                    tableau.AddCell("Hospital");
                    tableau.AddCell("Company Visit");

                   

                    foreach (var item in ListPrintcmd)
                    {
                        tableau.AddCell(item.idBon);
                        tableau.AddCell(item.datecmd);
                        tableau.AddCell(item.nameAuthor);
                        tableau.AddCell(item.sex);
                        //tableau.AddCell(item.nameEmployed);

                        tableau.AddCell(item.Health);
                        tableau.AddCell(item.company);
                        //tableau.AddCell(item.department);
                    }

                    tableau.TotalWidth = 750;
                    tableau.LockedWidth = true;
                    _document.Add(tableau);
                    _document.CloseDocument();
                }

                if (categ.Equals("Contractor"))
                {
                    Phrase text_header = new Phrase("\n\nLIST RECAP OF PURCHASE ORDERS\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK));
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
                    img.Alignment = Element.ALIGN_LEFT;
                    header_img.Add(img);
                    header_text.Add(text_header);
                    _document.Add(header_img);
                    _document.Add(header_text);
                    PdfPTable tableau = new PdfPTable(6);
                    //   tableau.PaddingTop = 5f;
                    Paragraph pgr = new Paragraph("RECAP OF PURCHASE ORDERS FOR CONTRACTOR");
                    pgr.Alignment = Element.ALIGN_CENTER;
                    pgr.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                    PdfPCell cellule = new PdfPCell(pgr);
                    cellule.Colspan = 6;
                    cellule.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableau.AddCell(cellule);
                    tableau.AddCell("ID Voucher ");
                    tableau.AddCell("Date Voucher");
                    tableau.AddCell("Contractor's Name");
                    tableau.AddCell("Company's Contractor");
                    tableau.AddCell("Hospital");
                    tableau.AddCell("Company");



                    foreach (var item in ListPrintcmd)
                    {
                        tableau.AddCell(item.idBon);
                        tableau.AddCell(item.datecmd);
                        tableau.AddCell(item.nameAuthor);
                        tableau.AddCell(item.sex);
                        //tableau.AddCell(item.nameEmployed);

                        tableau.AddCell(item.Health);
                        tableau.AddCell(item.company);
                        //tableau.AddCell(item.department);
                    }

                    tableau.TotalWidth = 750;
                    tableau.LockedWidth = true;
                    _document.Add(tableau);
                    _document.CloseDocument();
                }
                if (categ.Equals("CasualInfo"))
                {
                    Phrase text_header = new Phrase("\n\nLIST RECAP OF PURCHASE ORDERS\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK));
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
                    img.Alignment = Element.ALIGN_LEFT;
                    header_img.Add(img);
                    header_text.Add(text_header);
                    _document.Add(header_img);
                    _document.Add(header_text);
                    PdfPTable tableau = new PdfPTable(7);
                    //   tableau.PaddingTop = 5f;
                    Paragraph pgr = new Paragraph("RECAP OF PURCHASE ORDERS FOR CASUAL VOUCHERS");
                    pgr.Alignment = Element.ALIGN_CENTER;
                    pgr.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                    PdfPCell cellule = new PdfPCell(pgr);
                    cellule.Colspan = 8;
                    cellule.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableau.AddCell(cellule);
                    tableau.AddCell("Voucher ID");
                    tableau.AddCell("Date");
                    tableau.AddCell("Name");
                    tableau.AddCell("Company");
                    tableau.AddCell("Hospital");
                   // tableau.AddCell("Motif");
                    tableau.AddCell("Cause");
                    tableau.AddCell("Company Visited");



                    foreach (var item in ListPrintcmd)
                    {
                        tableau.AddCell(item.idBon);
                        tableau.AddCell(item.datecmd);
                        tableau.AddCell(item.nameAuthor);
                        tableau.AddCell(item.sex);
                        //tableau.AddCell(item.nameEmployed);

                        tableau.AddCell(item.Health);
                      //tableau.AddCell(item.company);
                        tableau.AddCell(item.phone);
                        tableau.AddCell(item.department);
                      
                    }

                    tableau.TotalWidth = 750;
                    tableau.LockedWidth = true;
                    _document.Add(tableau);
                    _document.CloseDocument();
                }

                byte[] _buffer = System.IO.File.ReadAllBytes(realFilename);
                String contentType = MimeMapping.GetMimeMapping(realFilename);
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = true,
                };
                HttpContext.Cache["FileDownload"] = _buffer;
                HttpContext.Cache["contentType"] = "pdf";
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return "exportFile";

            }
        }
      
        public ActionResult exportFile()
        {
            byte[] _file= (byte[])HttpContext.Cache["FileDownload"];
            FileContentResult _fileContentResult = null;
            if (HttpContext.Cache["contentType"].ToString().Equals("csv"))
            {
                _fileContentResult= File(_file, "Application/vnd.ms-excel", "RESULTAT PRINT.csv");
            }
            else if (HttpContext.Cache["contentType"].ToString().Equals("pdf"))
            {
                _fileContentResult = File(_file, "Application/pdf","LIST OF PURCHASE ORDERS.pdf");
            }
            
            return _fileContentResult;
        }

        public ActionResult ExportTable()
        {
            GridView gv = new GridView();
            var Query = from ds in dbContext.t_succursales
                        select ds;

            gv.DataSource = Query.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("Content-disposition", "attachement;filename=MVI.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            return RedirectToAction("generatePDF");
        }
        [AllowAnonymous]
        public ActionResult generatePDF()
        {
           // return View();
            var gv = new GridView();
            gv.DataSource = dbContext.t_beneficiaires.ToList();
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
            return File(bt, "content-disposition");

            
    }
  
        private void _ExportProcess(String data, ref String link,ref String FileName ,typeExportion TypeExport)
        {
            String generateFile = String.Empty;
            switch (TypeExport)
            {
                case typeExportion.CSV:
                    generateFile = String.Format("{0}.csv", DateTime.Now.Millisecond);
                    break;
                case typeExportion.PDF:
                    generateFile = String.Format("{0}.pdf", DateTime.Now.Millisecond);
                    break;
                case typeExportion.WORD:
                    generateFile = String.Format("{0}.docx", DateTime.Now.Millisecond);
                    break;
                default:
                    break;
            }
            FileName = generateFile;
            String path = String.Format("{0}{1}", Server.MapPath("~/export/"), generateFile);
            String HeaderPage = "Id Bon,Date Command,Name,Sex,Phone,Succursal,Departement,Hopital" + "\r" + "\n";
            String fileMaker=HeaderPage + data.ToString();
            System.IO.File.AppendAllText(path, fileMaker, Encoding.UTF8);

            //byte[] bt = System.Text.Encoding.ASCII.GetBytes(data.ToString().ToCharArray());
            link = path;

        }

        // REPORTING SYSTEM PROCESS

        // 1. MCI
        public ActionResult MedicalCrusherIssued()
        {
            this.CategoryMVI();
            return View();
        }
        [HttpPost]
        public ActionResult MedicalCrusherIssued(Models.exportDataMCI dataMCI)
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            byte[] bt = null;
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter=null;
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    ExcelGenerate excelGenerate = null;
                    bt = null;
                    var gv = new GridView();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=MEDICAL_VOUCHERS_ISSUED.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    switch (dataMCI.category)
                    {
                        case "Monthly":
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Monthly");
                            var Query1 = from ds in excelGenerate.__listMVC
                                         where ds.Company.Equals(auth.nameSuccursale)
                                         // select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR =ds.idVoucher,COMMENT=ds.Consultation };
                                         select new Medical_Voucher_Issued { Date = ds.Date, Dependent_ID = ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee, Hospital_Code = ds.HospitalCode, Hospital = ds.Hospital, Consultation = ds.Consultation, Company = ds.Company };



                            List<MVIObject> lstMVI = new List<MVIObject>();

                            gv.DataSource = gv.DataSource = gv.DataSource = Query1.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query1.ToList();
                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());

                            break;
                        case "Daily":
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Daily");
                             var Query2 = from ds in excelGenerate.__listMVC
                                         where ds.Company.Equals(auth.nameSuccursale)
                                          // select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR = ds.idVoucher, COMMENT = ds.Consultation };
                                          select new Medical_Voucher_Issued { Date = ds.Date, Dependent_ID = ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee, Hospital_Code = ds.HospitalCode, Hospital = ds.Hospital, Consultation = ds.Consultation, Company = ds.Company, Department = ds.Department };

                            gv.DataSource = gv.DataSource = Query2.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query2.ToList();
                            //gv.DataSource = Query2.ToList().Count == 0 ? new List<MVIObject>() { new MVIObject { Date = "", DependentID = "", DependentName = "", Gender = "", EmployeeID = "", EmployeeName = "", GenderEmployee = "", Hospital = "", Consultation = "", Company = "", Department = "", Coast = 0.0M } } : Query2.ToList();
                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                            break;
                        case "Choose":
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Choose",dataMCI.date1,dataMCI.date2);
                            var Query3 = from ds in excelGenerate.__listMVC
                                         where ds.Company.Equals(auth.nameSuccursale)
                                         //select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR = ds.idVoucher, COMMENT = ds.Consultation };
                                         select new Medical_Voucher_Issued { Date = ds.Date, Dependent_ID = ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee, Hospital_Code = ds.HospitalCode, Hospital = ds.Hospital, Consultation = ds.Consultation, Company = ds.Company, Department = ds.Department };

                            gv.DataSource = Query3.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query3.ToList();
                            //  gv.DataSource = Query3.ToList().Count == 0 ? new List<MVIObject>() { new MVIObject { Date = "", DependentID = "", DependentName = "", Gender = "", EmployeeID = "", EmployeeName = "", GenderEmployee = "", Hospital = "", Consultation = "", Company = "", Department = "", Coast = 0.0M } } : Query3.ToList();
                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                            break;
                        case "Weekly":
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Weekly");
                             var Query4 = from ds in excelGenerate.__listMVC
                                         where ds.Company.Equals(auth.nameSuccursale)
                                          //    select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR = ds.idVoucher, COMMENT = ds.Consultation };
                                          select new Medical_Voucher_Issued { Date = ds.Date, Dependent_ID = ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee, Hospital_Code = ds.HospitalCode, Hospital = ds.Hospital, Consultation = ds.Consultation, Company = ds.Company,Department=ds.Department };

                            gv.DataSource = Query4.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query4.ToList();

                            //gv.DataSource = Query4.ToList().Count == 0 ? new List<MVIObject>() { new MVIObject { Date = "", DependentID = "", DependentName = "", Gender = "", EmployeeID = "", EmployeeName = "", GenderEmployee = "", Hospital = "", Consultation = "", Company = "", Department = "", Coast = 0.0M } } : Query4.ToList();
                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ExcelGenerate excelGenerate = null;
                    bt = null;
                  

                    var gv = new GridView();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=MEDICAL_VOUCHERS_ISSUED.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    switch (dataMCI.category)
                    {
                        case "Monthly":
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Monthly");
                              var Query1 = from ds in excelGenerate.__listMVC
                                           select new  Medical_Voucher_Issued { Date = ds.Date, Dependent_ID= ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee,Hospital_Code=ds.HospitalCode,Hospital=ds.Hospital,Consultation=ds.Consultation,Company=ds.Company,Department=ds.Department };
                              //  List<MVIObject> lstMVI = new List<MVIObject>();
                            //     var lst = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));

                            gv.DataSource = Query1.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued 
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query1.ToList();
                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());

                            break;
                        case "Daily":
                            //var lst2 = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Daily");
                           var Query2 = from ds in excelGenerate.__listMVC
                                           select new  Medical_Voucher_Issued { Date = ds.Date, Dependent_ID= ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee,Hospital_Code=ds.HospitalCode,Hospital=ds.Hospital,Consultation=ds.Consultation,Company=ds.Company, Department = ds.Department };

                            // select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR = ds.idVoucher, COMMENT = ds.Consultation };

                            List<MVIObject> lstMVI2 = new List<MVIObject>();
                       //     var lst = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));
                            gv.DataSource = gv.DataSource = Query2.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query2.ToList();
                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                            break;
                        case "Choose":
                            //var lst3 = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Choose",dataMCI.date1,dataMCI.date2);
                            var Query3 = from ds in excelGenerate.__listMVC
                                           select new  Medical_Voucher_Issued { Date = ds.Date, Dependent_ID= ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee,Hospital_Code=ds.HospitalCode,Hospital=ds.Hospital,Consultation=ds.Consultation,Company=ds.Company, Department = ds.Department };

                            //select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR = ds.idVoucher, COMMENT = ds.Consultation };

                            List<MVIObject> lstMVI3 = new List<MVIObject>();
                            //     var lst = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));
                            // gv.DataSource = Query3.ToList().Count == 0 ? new List<MVIObject>() { new MVIObject { Date = "", DependentID = "", DependentName = "", Gender = "", EmployeeID = "", EmployeeName = "", GenderEmployee = "", Hospital = "", Consultation = "", Company = "", Department = "", Coast = 0.0M } } : Query3.ToList();
                            gv.DataSource = gv.DataSource = Query3.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query3.ToList();

                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                            break;
                        case "Weekly":
                            //var lst4 = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));
                            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED, "Weekly");
                            var Query4 = from ds in excelGenerate.__listMVC
                                             //select new Medical_Voucher_Issued { DATE = ds.Date, EMPLOYEE_ID_NUMBER = ds.EmployeeID, NAME_OF_PATIENT = ds.DependentName, STATUS_OF_PATIENT = ds.Gender, MEDICAL_FACILITY = ds.Hospital, VOUCHER_GENERATOR = ds.idVoucher, COMMENT = ds.Consultation };
                                         select new Medical_Voucher_Issued { Date = ds.Date, Dependent_ID = ds.DependentID, Dependent_Name = ds.DependentName, Dependent_Gender = ds.Gender, ID_Employee = ds.EmployeeID, Employee_Name = ds.EmployeeName, Gender = ds.GenderEmployee, Hospital_Code = ds.HospitalCode, Hospital = ds.Hospital, Consultation = ds.Consultation, Company = ds.Company,Department=ds.Department };

                            List<MVIObject> lstMVI4 = new List<MVIObject>();
                                  
                       //     var lst = excelGenerate.__listMVC.Where(e => e.Company.Equals(auth.nameSuccursale));
                            //gv.DataSource = Query4.ToList().Count == 0 ? new List<MVIObject>() { new MVIObject { Date = "", DependentID = "", DependentName = "", Gender = "", EmployeeID = "", EmployeeName = "", GenderEmployee = "", Hospital = "", Consultation = "", Company = "", Department = "", Coast = 0.0M } } : Query4.ToList();
                             gv.DataSource = Query4.ToList().Count == 0 ? new List<Medical_Voucher_Issued>() { new Medical_Voucher_Issued
                                {
                                Date = "",
                                Dependent_ID = "",
                                Dependent_Name = "",
                                Dependent_Gender = "",
                                ID_Employee = "",
                                Employee_Name = "",
                                Gender = "",
                                Hospital_Code = "",
                                Hospital = "",
                                Consultation = "",
                                Company = ""
                                }
                    } : Query4.ToList();

                            gv.DataBind();
                            gv.RenderControl(objHtmlTextWriter);
                            Response.Output.Write(objStringWriter.ToString());
                            Response.Flush();
                            Response.End();
                            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
          
            return File(bt, "content-disposition");
        }
      
       

        //PROCESS EXCEL MEDICAL CRUSHER ISSUED
        public ActionResult excelMCI()
        {
            //using (StreamReader sReader=new StreamReader(Request.InputStream))
            //{
            //    //var data = sReader.ReadToEnd();
            //    //byte[] bt = null;
                
            //    //ExcelGenerate excelGenerate = null;
            //    //JavaScriptSerializer serial = new JavaScriptSerializer();
            //    //exportDataMCI exportdata = serial.Deserialize<exportDataMCI>(data);
            //    // var gv = new GridView();
              

            //    //    Response.ClearContent();
            //    //    Response.Buffer = true;
            //    //    Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            //    //    Response.ContentType = "application/ms-excel";

            //    //    Response.Charset = "";
            //    //    StringWriter objStringWriter = new StringWriter();
            //    //    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                   

                    
            
            //    //if (!exportdata.category.Equals("Choose"))
            //    //{
            //    //    switch (exportdata.category)
            //    //    {
            //    //        case "Monthly":
            //    //            excelGenerate = new ExcelGenerate(Categorie.MEDICAL_VOUCHERS_ISSUED);
            //    //                  gv.DataSource = excelGenerate.__listMVC;
            //    //                  gv.DataBind();
            //    //                  gv.RenderControl(objHtmlTextWriter);
            //    //                 Response.Output.Write(objStringWriter.ToString());
            //    //                 Response.Flush();
            //    //                 Response.End();
            //    //                 bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
            //    //           // this.__excelSystemReporting()
            //    //            break;
            //    //        default:
            //    //            break;
            //    //    }
            //    }
                
            //}

            //
            return File("~/export/File.pdf", "Application/pdf");
        }

     

        //2.A. General Report
        public ActionResult GeneralReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GeneralReport(DateFinding dFind)
        {
            var woorkbook = new XLWorkbook();
            DataTable dte = new DataTable();
            dte.Columns.Add("Date");
            dte.Columns.Add("Company");
            dte.Columns.Add("# Vouchers");
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;

            if (ModelState.IsValid)
            {
                ExcelGenerate _excelGenerate = new ExcelGenerate(Categorie.REPORT_OF_MEDICAL_VOUCHERS, "GR",dFind.from,dFind.to);
                byte[] bt = null;
                var gv = new GridView();
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = null;
                if (Session["userinfo"]!=null)
                {
                    Authenticate auth = (Authenticate)Session["userinfo"];

                    if (!auth.Priority.Equals("administrator"))
                    {
                        var Query = from ds in _excelGenerate.__lstRMVGR
                                    where ds.Company.Equals(auth.nameSuccursale)
                                    select ds;

                        foreach (var item in Query)
                        {
                            var row = dte.NewRow();
                            row["Date"] = item.Date;
                            row["Company"] = item.Company;
                            row["# Vouchers"] = item.Vouchers;
                            dte.Rows.Add(row);

                        }
                        woorkbook.AddWorksheet(dte, "GENERAL REPORTS");
                        //gv.DataSource = Query.ToList().Count == 0 ? new List<RMV_General_Report>() { new RMV_General_Report { Date = "", Company = "", Vouchers = 0 } } : Query.ToList();
                        //gv.DataBind();
                        //Response.ClearContent();
                        //Response.Buffer = true;
                        //Response.AddHeader("content-disposition", "attachment; filename=REPORT_MEDICAL_VOUCHERS_GENERAL_REPORT.xls");
                        //Response.ContentType = "application/ms-excel";
                        //Response.Charset = "";
                        //objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                        //gv.RenderControl(objHtmlTextWriter);
                        //Response.Output.Write(objStringWriter.ToString());
                        //Response.Flush();
                        //Response.End();
                        //bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
                    }
                    else
                    {
                        var Query= _excelGenerate.__lstRMVGR;
                        foreach (var item in Query)
                        {
                            var row = dte.NewRow();
                            row["Date"] = item.Date;
                            row["Company"] = item.Company;
                            row["# Vouchers"] = item.Vouchers;
                            dte.Rows.Add(row);

                        }
                        woorkbook.AddWorksheet(dte, "GENERAL REPORTS");
                    }
                }

                MemoryStream strm = new MemoryStream();
                woorkbook.SaveAs(strm);
                return File(strm.GetBuffer(), "content-disposition", "HOSPITAL MEDICAL VOUCHER EMPLOYEES.xlsx");
            } 
            return null;
        }
        // PROCESS EXCEL MEDICAL CRUSHED ISSUED
        public ActionResult excelGeneralReport()
        {
            return View();
        }
        // 2.B. HOSPITAL AND EMPLOYEES VS DEPENDANT

        public ActionResult HED()
        {
            return View();
        }

       [HttpPost]
        public ActionResult HED(DateFinding dFind)
        {

            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            List<HED> lstHed = new List<HED>();
            var workbook = new XLWorkbook();

            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    if (ModelState.IsValid)
                    {

                        DateTime _from = DateTime.Parse(dFind.from);
                        DateTime _to = DateTime.Parse(dFind.to);
                        var ListHospital = from hosto in dbContext.t_centre_soins
                                           select hosto;

                        foreach (var item in ListHospital)
                        {
                            int ctrSpouse = 0;
                            int ctrChildren = 0;
                            int ctrContractor = 0;
                            int ctrVisitor = 0;
                            int ctrCasual = 0;
                            int ctrEmployee = 0;

                            DataTable dte = new DataTable();
                            dte.Columns.Add("Date");
                            dte.Columns.Add("Hospital");
                            dte.Columns.Add("Company");
                            dte.Columns.Add("Employee");
                            dte.Columns.Add("Spouse");
                            dte.Columns.Add("Children");
                            dte.Columns.Add("Visitor");
                            dte.Columns.Add("Contractor");
                            dte.Columns.Add("Casual");
                            dte.Columns.Add("Total");
                            String dateCurrent = "";
                           
                                var QueryEmployee = from employee in dbContext.t_beneficiaires
                                                    join vouchers in dbContext.t_bon_commandes
                                                    on employee.C_id equals vouchers.C_id_bene
                                                    join hospital in dbContext.t_centre_soins
                                                    on vouchers.C_id_centre equals hospital.C_id_centre
                                                    join company in dbContext.t_succursales
                                                    on employee.C_id_succ equals company.C_id
                                                    where employee.C_id_succ.Equals(auth.Succursale) && vouchers.C_id_centre == item.C_id_centre //&& ( )
                                                    orderby vouchers.C_id_bon ascending
                                                    select new { company, vouchers, hospital, employee };


                                foreach (var itemEmployee in QueryEmployee)
                                {



                                    if (DateTime.Parse(itemEmployee.vouchers.C_datedeb) >= _from && DateTime.Parse(itemEmployee.vouchers.C_datedeb) <= _to)
                                    {
                                        ctrEmployee += 1;
                                       dateCurrent = itemEmployee.vouchers.C_datedeb;

                                    }



                                }

                                var CountSpouse = from spouse in dbContext.t_beneficiaires
                                                  join BonCmd in dbContext.t_bon_commandes
                                                  on spouse.C_id equals BonCmd.C_id_bene
                                                  join hosto in dbContext.t_centre_soins
                                                  on BonCmd.C_id_centre equals hosto.C_id_centre
                                                  where spouse.C_mat.Equals(null) && !spouse.C_id_partenaire.Equals(null) && BonCmd.C_id_centre == item.C_id_centre
                                                  select new { spouse, BonCmd, hosto };

                                foreach (var itemSpouse in CountSpouse)
                                {
                                    if (DateTime.Parse(itemSpouse.BonCmd.C_datedeb) >= _from && DateTime.Parse(itemSpouse.BonCmd.C_datedeb) <= _to)
                                    {

                                        int idEmployee = int.Parse(itemSpouse.spouse.C_id_partenaire);
                                        var em = dbContext.t_beneficiaires.Where(e => e.C_id == idEmployee).FirstOrDefault();
                                        if (em.C_id_succ.Equals(auth.Succursale) && itemSpouse.hosto.C_id_centre == item.C_id_centre)
                                        {
                                            ctrSpouse += 1;
                                            dateCurrent = itemSpouse.BonCmd.C_datedeb;
                                        }

                                    }
                                }
                                var CountChild = from child in dbContext.t_beneficiaires
                                                 join BonCmd in dbContext.t_bon_commandes
                                                 on child.C_id equals BonCmd.C_id_bene
                                                 join hosto in dbContext.t_centre_soins
                                                 on BonCmd.C_id_centre equals hosto.C_id_centre
                                                 where child.C_mat.Equals(null) && !child.C_id_parent.Equals(null) && BonCmd.C_id_centre == item.C_id_centre
                                                 select new { child, BonCmd, hosto };

                                foreach (var itemChild in CountChild)
                                {


                                    if (DateTime.Parse(itemChild.BonCmd.C_datedeb) >= _from && DateTime.Parse(itemChild.BonCmd.C_datedeb) <= _to)
                                    {
                                        int idEmployee = int.Parse(itemChild.child.C_id_parent);
                                        var em = dbContext.t_beneficiaires.Where(e => e.C_id == idEmployee).FirstOrDefault();
                                    if (em.C_id_succ.Equals(auth.Succursale))
                                    {
                                        ctrChildren += 1;
                                        dateCurrent = itemChild.BonCmd.C_datedeb;
                                    }
                                        
                                    }

                                }
                                var CountVisitor = from employe in dbContext.t_beneficiaires
                                                   join voucher in dbContext.t_bon_commandes on employe.C_id equals voucher.C_id_bene
                                                   join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                   where !employe.C_id_visitor.Equals(null) && voucher.C_id_centre == item.C_id_centre
                                                   select new { employe, voucher, hospital };


                                foreach (var itemVisitor in CountVisitor)
                                {
                                    if (DateTime.Parse(itemVisitor.voucher.C_datedeb) >= _from && DateTime.Parse(itemVisitor.voucher.C_datedeb) <= _to)
                                    {
                                        String idCompany = itemVisitor.employe.C_id_visitor;
                                        if (idCompany.Equals(auth.Succursale))
                                        {
                                            ctrVisitor += 1;
                                            dateCurrent = itemVisitor.voucher.C_datedeb;
                                        }
                                    }
                                }
                                var CountContractor = from contractor in dbContext.t_contractor
                                                      join employee in dbContext.employee_contractor on contractor.C_id equals employee.C_idContractor
                                                      join Company in dbContext.t_succursales on contractor.C_idSucc equals Company.C_id
                                                      join voucher in dbContext.t_vouchers_contractor on employee.C_id equals voucher.C_id_Employed
                                                      join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                      where hospital.C_id_centre == item.C_id_centre && contractor.C_idSucc.Equals(auth.Succursale)
                                                      select new { employee, Company, voucher };

                                foreach (var itemContractor in CountContractor)
                                {
                                    if (DateTime.Parse(itemContractor.voucher.C_datedeb) >= _from && DateTime.Parse(itemContractor.voucher.C_datedeb) <= _to)
                                    {
                                        ctrContractor += 1;
                                        dateCurrent = itemContractor.voucher.C_datedeb;
                                    }
                                }

                                var CountCasual = from casual in dbContext.t_vouchers_casuals
                                                  join company in dbContext.t_succursales on casual.C_id_company equals company.C_id
                                                  join hospital in dbContext.t_centre_soins on casual.C_id_centre equals hospital.C_id_centre
                                                  where company.C_id.Equals(auth.Succursale) && hospital.C_id_centre == item.C_id_centre
                                                  select new { casual, company, hospital };


                                foreach (var itemCasual in CountCasual)
                                {
                                    if (DateTime.Parse(itemCasual.casual.C_date_casual) >= _from && DateTime.Parse(itemCasual.casual.C_date_casual) <= _to)
                                    {
                                        ctrCasual += 1;
                                        dateCurrent = itemCasual.casual.C_date_casual;
                                    }
                                }

                                var row = dte.NewRow();
                                row["Date"] = dateCurrent;
                                row["Hospital"] = item.C_name;
                                row["Company"] = auth.nameSuccursale;
                                row["Employee"] = ctrEmployee;
                                row["Spouse"] = ctrSpouse;
                                row["Children"] = ctrChildren;
                                row["Visitor"] = ctrVisitor;
                                row["Contractor"] = ctrContractor;
                                row["Casual"] = ctrCasual;
                                row["Visitor"] = ctrVisitor;
                                row["Total"] = +ctrSpouse + ctrContractor + ctrChildren + ctrCasual + ctrEmployee + ctrVisitor;
                                dte.Rows.Add(row);
                                ctrEmployee = 0;
                                ctrCasual = 0; ctrChildren = 0; ctrContractor = 0; ctrSpouse = 0; ctrVisitor = 0;

                            
                            var ws = workbook.Worksheets.Add(dte, item.C_name);
                            dte.Clear();
                        }
                    }
                
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                       


                        DateTime _from = DateTime.Parse(dFind.from);
                        DateTime _to =DateTime.Parse(dFind.to);
                        var ListHospital = from hosto in dbContext.t_centre_soins
                                           select hosto;

                        foreach (var item in ListHospital)
                        {
                            int ctrSpouse = 0;
                            int ctrChildren = 0;
                            int ctrContractor = 0;
                            int ctrVisitor = 0;
                            int ctrCasual = 0;
                            int ctrEmployee = 0;
                            String dateCurrent = "";

                            DataTable dte = new DataTable();
                            dte.Columns.Add("Date");
                            dte.Columns.Add("Hospital");
                            dte.Columns.Add("Company");
                            dte.Columns.Add("Employee");
                            dte.Columns.Add("Spouse");
                            dte.Columns.Add("Children");
                            dte.Columns.Add("Visitor");
                            dte.Columns.Add("Contractor");
                            dte.Columns.Add("Casual");
                            dte.Columns.Add("Total");
                            foreach (var itemCompany in dbContext.t_succursales.ToList())
                            {



                                var QueryEmployee = from employee in dbContext.t_beneficiaires
                                                    join vouchers in dbContext.t_bon_commandes
                                                    on employee.C_id equals vouchers.C_id_bene
                                                    join hospital in dbContext.t_centre_soins
                                                    on vouchers.C_id_centre equals hospital.C_id_centre
                                                    join company in dbContext.t_succursales
                                                    on employee.C_id_succ equals company.C_id
                                                    where employee.C_id_succ.Equals(itemCompany.C_id) && vouchers.C_id_centre == item.C_id_centre //&& ( )
                                                    orderby vouchers.C_id_bon ascending
                                                    select new { company, vouchers, hospital, employee };

                               
                                foreach (var itemEmployee in QueryEmployee)
                                {


                         
                                    if (DateTime.Parse(itemEmployee.vouchers.C_datedeb)>=_from && DateTime.Parse(itemEmployee.vouchers.C_datedeb)<=_to)
                                    {
                                        ctrEmployee += 1;
                                       

                                    }



                                }

                                var CountSpouse = from spouse in dbContext.t_beneficiaires
                                                  join BonCmd in dbContext.t_bon_commandes
                                                  on spouse.C_id equals BonCmd.C_id_bene
                                                  join hosto in dbContext.t_centre_soins
                                                  on BonCmd.C_id_centre equals hosto.C_id_centre
                                                  where spouse.C_mat.Equals(null) && !spouse.C_id_partenaire.Equals(null) && BonCmd.C_id_centre == item.C_id_centre
                                                  select new { spouse, BonCmd, hosto };

                                foreach (var itemSpouse in CountSpouse)
                                {
                                    if (DateTime.Parse(itemSpouse.BonCmd.C_datedeb) >= _from && DateTime.Parse(itemSpouse.BonCmd.C_datedeb) <= _to)
                                    {
                                        dateCurrent = itemSpouse.BonCmd.C_datedeb;
                                        int idEmployee = int.Parse(itemSpouse.spouse.C_id_partenaire);
                                        var em = dbContext.t_beneficiaires.Where(e => e.C_id == idEmployee).FirstOrDefault();
                                        if (em.C_id_succ.Equals(itemCompany.C_id) && itemSpouse.hosto.C_id_centre == item.C_id_centre)
                                        {
                                            ctrSpouse += 1;
                                        }

                                    }
                                }
                                var CountChild = from child in dbContext.t_beneficiaires
                                                  join BonCmd in dbContext.t_bon_commandes
                                                  on child.C_id equals BonCmd.C_id_bene
                                                  join hosto in dbContext.t_centre_soins
                                                  on BonCmd.C_id_centre equals hosto.C_id_centre
                                                  where child.C_mat.Equals(null) && !child.C_id_parent.Equals(null) && BonCmd.C_id_centre == item.C_id_centre
                                                  select new { child, BonCmd, hosto };

                                foreach (var itemChild in CountChild)
                                {
                                  
                                        
                                        if (DateTime.Parse(itemChild.BonCmd.C_datedeb) >= _from && DateTime.Parse(itemChild.BonCmd.C_datedeb) <= _to)
                                        {
                                            int idEmployee = int.Parse(itemChild.child.C_id_parent);
                                            var em = dbContext.t_beneficiaires.Where(e => e.C_id == idEmployee).FirstOrDefault();
                                        if (em.C_id_succ.Equals(itemCompany.C_id))
                                        {
                                            ctrChildren += 1;
                                        }
                                            
                                        }
                                    
                                }
                                var CountVisitor = from employe in dbContext.t_beneficiaires
                                                   join voucher in dbContext.t_bon_commandes on employe.C_id equals voucher.C_id_bene
                                                   join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                   where !employe.C_id_visitor.Equals(null) && voucher.C_id_centre==item.C_id_centre
                                                   select new { employe, voucher, hospital };


                                foreach (var itemVisitor in CountVisitor)
                                {
                                    if (DateTime.Parse(itemVisitor.voucher.C_datedeb) >= _from && DateTime.Parse(itemVisitor.voucher.C_datedeb) <= _to)
                                    {
                                        String idCompany = itemVisitor.employe.C_id_visitor;
                                        if (idCompany.Equals(itemCompany.C_id))
                                        {
                                            ctrVisitor += 1;
                                        }
                                    }
                                }
                                var CountContractor = from contractor in dbContext.t_contractor
                                                      join employee in dbContext.employee_contractor on contractor.C_id equals employee.C_idContractor
                                                      join Company in dbContext.t_succursales on contractor.C_idSucc equals Company.C_id
                                                      join voucher in dbContext.t_vouchers_contractor on employee.C_id equals voucher.C_id_Employed
                                                      join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                      where hospital.C_id_centre==item.C_id_centre && contractor.C_idSucc.Equals(itemCompany.C_id)
                                                      select new { employee, Company, voucher };

                                foreach (var itemContractor in CountContractor)
                                {
                                    if (DateTime.Parse(itemContractor.voucher.C_datedeb) >= _from && DateTime.Parse(itemContractor.voucher.C_datedeb) <= _to)
                                    {
                                        ctrContractor += 1;
                                    }
                                }

                                var CountCasual = from casual in dbContext.t_vouchers_casuals
                                                  join company in dbContext.t_succursales on casual.C_id_company equals company.C_id
                                                  join hospital in dbContext.t_centre_soins on casual.C_id_centre equals hospital.C_id_centre
                                                  where company.C_id.Equals(itemCompany.C_id) && hospital.C_id_centre == item.C_id_centre
                                                  select new { casual, company, hospital };


                                foreach (var itemCasual in CountCasual)
                                {
                                    if (DateTime.Parse(itemCasual.casual.C_date_casual) >= _from && DateTime.Parse(itemCasual.casual.C_date_casual) <= _to)
                                    {
                                        ctrCasual += 1;
                                    }
                                }

                                var row = dte.NewRow();
                               row["Date"] = dateCurrent;
                               row["Hospital"] = item.C_name;
                                row["Company"] = itemCompany.C_name;
                                row["Employee"] = ctrEmployee;
                                row["Spouse"] = ctrSpouse;
                                row["Children"] = ctrChildren;
                                row["Visitor"] = ctrVisitor;
                                row["Contractor"] = ctrContractor;
                                row["Casual"] = ctrCasual;
                                row["Visitor"] = ctrVisitor;
                                row["Total"] = +ctrSpouse + ctrContractor + ctrChildren + ctrCasual + ctrEmployee+ctrVisitor;
                                dte.Rows.Add(row);
                                ctrEmployee = 0;
                                ctrCasual = 0; ctrChildren = 0; ctrContractor = 0; ctrSpouse = 0; ctrVisitor = 0;

                            }
                            var ws = workbook.Worksheets.Add(dte, item.C_name);
                            dte.Clear();
                        }
                    }
                }
            }
            MemoryStream strm = new MemoryStream();
            workbook.SaveAs(strm);
            return File(strm.GetBuffer(), "content-disposition", "HOSPITAL_AND_EMPLOYEE_DEPENDENTS.xlsx"); 
        }
        public ActionResult excelHED()
        {
            return View(
                );
        }

        // 2.C. FOR DEPARTEMENT
        public ActionResult DepartementReporting()
        {
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    ViewData["departements"] = new SelectList(dbContext.t_succursales.Where(company=>company.C_id.Equals(auth.Succursale)), "C_id", "C_name");
                    t_departement succ = new t_departement();
                }
                else
                {
                    ViewData["departements"] = new SelectList(dbContext.t_succursales.ToList(), "C_id", "C_name");
                    t_departement succ = new t_departement();
                }
            }
            
           
            return View();
        }

       [HttpPost]
        public Object DepartementReporting(RMV_Department rmvdep)
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            int idSucc=int.Parse(rmvdep.Company);
           Models.t_succursales Succ = (dbContext.t_succursales.Where(e => e.C_id.Equals(rmvdep.Company))).FirstOrDefault();
            int size = rmvdep.dataList.Length;
            string str = rmvdep.dataList.Substring(1, (size - 2));
            String[] SplitterDepart = str.Split(',');
            String Line = ""; 
            foreach (var item in SplitterDepart)
            {
                Line= item.Substring(1, (item.Length - 2));

            }
            for (int i = 0; i < SplitterDepart.Length; i++)
            {
                SplitterDepart[i]=SplitterDepart[i].Substring(1,SplitterDepart[i].Length-2);
            }
            String Fulldata=String.Empty;
            DateTime from=DateTime.Parse(rmvdep.from);
            DateTime to=DateTime.Parse(rmvdep.to);
            ExcelGenerate _excelGenerate = null;
            ExcelGenerate.company = rmvdep.Company;
            ExcelGenerate.department = "";
            Models.RequestEmployed ReqEmployed = new RequestEmployed();
            List<Employed> LstQueryFull = ReqEmployed.getListEmployed();
            List<BonCommand> lstCmd = new List<BonCommand>();
            List<RMV_Department> lstRMVD = new List<RMV_Department>();
            if (SplitterDepart.Length == 1)
            {
                String depart1 = SplitterDepart[0];

                int totalVouchers = 0;
                t_departement depart = dbContext.t_departement.Where(d => d.C_id_depart.Equals(depart1)).FirstOrDefault();
                var QueryBasicDate = from b in dbContext.t_beneficiaires
                                     join v in dbContext.t_bon_commandes on b.C_id equals v.C_id_bene
                                     //  where b.C_id_succ.Equals(Succ.C_id) && b.C_id_depart == depart.C_id
                                     select v.C_datedeb;


                foreach (var item in QueryBasicDate.Distinct())
                {

                    if (DateTime.Parse(item) >= from && DateTime.Parse(item) <= to)
                    {


                        int? id = (int?)depart.C_id;
                        var QueryEmployee = from b in dbContext.t_beneficiaires
                                            join v in dbContext.t_bon_commandes on b.C_id equals v.C_id_bene
                                            where b.C_id_succ.Equals(Succ.C_id) && b.C_id_depart == id && v.C_datedeb.Equals(item)
                                            select b;

                        totalVouchers += QueryEmployee.ToList().Count;
                        var spouse21 = from spouse in dbContext.t_beneficiaires
                                       join vouchers in dbContext.t_bon_commandes on spouse.C_id equals vouchers.C_id_bene
                                       where !spouse.C_id_partenaire.Equals(null) && vouchers.C_datedeb.Equals(item) && spouse.C_mat.Equals(null)
                                       select new { vouchers, spouse };


                        foreach (var itemEm in spouse21)
                        {
                            int i = int.Parse(itemEm.spouse.C_id_partenaire);
                            int? depId = (int?)depart.C_id;
                            var obj = dbContext.t_beneficiaires.Where(e => e.C_id == i && e.C_id_depart == depId && e.C_id_succ.Equals(Succ.C_id)).FirstOrDefault();
                            if (obj != null)
                            {
                                totalVouchers += 1;
                            }
                        }

                        var children = from child in dbContext.t_beneficiaires
                                       join vouchers in dbContext.t_bon_commandes on child.C_id equals vouchers.C_id_bene
                                       where !child.C_id_parent.Equals(null) && vouchers.C_datedeb.Equals(item)
                                       select new { vouchers, child };


                        foreach (var itemEm in children)
                        {
                            int i = int.Parse(itemEm.child.C_id_parent);
                            int? depId = (int?)depart.C_id;
                            var obj = dbContext.t_beneficiaires.Where(e => e.C_id == i && e.C_id_depart == depId).FirstOrDefault();
                            if (obj != null)
                            {
                                totalVouchers += 1;
                            }
                        }


                        if (totalVouchers > 0)
                        {
                            RMV_Department rmvdepDependents = new RMV_Department
                            {
                                Date = item,
                                Company = Succ.C_name,
                                Department = depart.C_id_depart,
                                Vouchers = (totalVouchers).ToString()
                            };
                            lstRMVD.Add(rmvdepDependents);
                            totalVouchers = 0;
                        }

                    }


                }
                

            }
            else
            {
                foreach (var splitter in SplitterDepart)
                {
                    int totalVouchers = 0;
                    t_departement depart = dbContext.t_departement.Where(d => d.C_id_depart.Equals(splitter)).FirstOrDefault();
                    var QueryBasicDate = from b in dbContext.t_beneficiaires
                                         join v in dbContext.t_bon_commandes on b.C_id equals v.C_id_bene
                                       //  where b.C_id_succ.Equals(Succ.C_id) && b.C_id_depart == depart.C_id
                                         select v.C_datedeb;


                    foreach (var item in QueryBasicDate.Distinct())
                    {
                        
                        if (DateTime.Parse(item) >= from && DateTime.Parse(item) <= to)
                        {

                         

                                var QueryEmployee = from b in dbContext.t_beneficiaires
                                                 join v in dbContext.t_bon_commandes on b.C_id equals v.C_id_bene
                                                 where b.C_id_succ.Equals(Succ.C_id) && b.C_id_depart == depart.C_id && v.C_datedeb.Equals(item)
                                                 select b;

                                totalVouchers += QueryEmployee.ToList().Count;
                                var spouse21 = from spouse in dbContext.t_beneficiaires
                                               join vouchers in dbContext.t_bon_commandes on spouse.C_id equals vouchers.C_id_bene
                                               where !spouse.C_id_partenaire.Equals(null) && vouchers.C_datedeb.Equals(item)
                                               select new { vouchers, spouse };


                                foreach (var itemEm in spouse21)
                                {
                                    int i = int.Parse(itemEm.spouse.C_id_partenaire);
                                    int? depId = (int?)depart.C_id;
                                    var obj = dbContext.t_beneficiaires.Where(e => e.C_id == i && e.C_id_depart==depId).FirstOrDefault();
                                    if (obj!=null)
                                    {
                                        totalVouchers += 1;
                                    }
                                }

                                var children = from child in dbContext.t_beneficiaires
                                               join vouchers in dbContext.t_bon_commandes on child.C_id equals vouchers.C_id_bene
                                               where !child.C_id_parent.Equals(null) && vouchers.C_datedeb.Equals(item)
                                               select new { vouchers, child };


                                foreach (var itemEm in children)
                                {
                                    int i = int.Parse(itemEm.child.C_id_parent);
                                    int? depId = (int?)depart.C_id;
                                    var obj = dbContext.t_beneficiaires.Where(e => e.C_id == i && e.C_id_depart == depId).FirstOrDefault();
                                    if (obj != null)
                                    {
                                        totalVouchers += 1;
                                    }
                                }


                            if (totalVouchers>0)
                            {
                                RMV_Department rmvdepDependents = new RMV_Department
                                {
                                    Date = item,
                                    Company = Succ.C_name,
                                    Department = depart.C_id_depart,
                                    Vouchers = (totalVouchers).ToString()
                                };
                                lstRMVD.Add(rmvdepDependents);
                                totalVouchers = 0;
                            }
                            
                        }
                          

                    }
                    var spouse2 = from spouse in dbContext.t_beneficiaires
                                  join vouchers in dbContext.t_bon_commandes on spouse.C_id equals vouchers.C_id_bene
                                  where !spouse.C_id_partenaire.Equals(null)
                                  select vouchers.C_datedeb;

                    foreach (var itemDate in spouse2.Distinct())
                    {
                        if (DateTime.Parse(itemDate)>=from && DateTime.Parse(itemDate)<=to)
                        {
                            

                            if (true)
                            {

                            }
                        }
                    }
                }
            }
                               
               
          //  _excelGenerate = new ExcelGenerate(Categorie.REPORT_OF_MEDICAL_VOUCHERS, "RMVDep", rmvdep.from, rmvdep.to);

            //foreach (var departement in SplitterDepart)
            //{
            //    ExcelGenerate.department = departement;
            //   _excelGenerate= new ExcelGenerate(Categorie.REPORT_OF_MEDICAL_VOUCHERS, "RMVDep", rmvdep.from, rmvdep.to);


            byte[] bt = null;
            var gv = new GridView();
            var customQuery = from ds in lstRMVD
                              select new {Date=ds.Date,Company=ds.Company,Department=ds.Department,Vouchers=ds.Vouchers };

            gv.DataSource = customQuery.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=REPORT_MEDICAL_VOUCHERS_DEPARTMENTS.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());

            return File(bt, "content-disposition");
            //}
            
            
        }
        public ActionResult excelDepartementReporting()
        {
            return View();
        }
        //3.FOR SUCCURSAL
        public ActionResult SuccursaleReporting()
        {
            this.LoadMonthYear();
            return View();
        }

       [HttpPost]
        public ActionResult  SuccursaleReporting(DatePreviousActual dte)
        {

            {
                String[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemnber", "December" };
                String from = dte.From, to = DateTime.Now.Month.ToString();

                CultureInfo cInfo = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = cInfo;
                var workbook = new XLWorkbook();
                DataTable dt = new DataTable();
                dt.Columns.Add("Company");
                dt.Columns.Add("Previous Month");
                dt.Columns.Add("Actual Month");
                dt.Columns.Add("Hospital");
                if (Session["userinfo"] != null)
                {
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    if (auth.Priority.Equals("user"))
                    {
                        int ctrDependecies = 0;
                        int ctrEmployee = 0;

                        var MonthActual = DateTime.Now.Month.ToString();
                        string MonthPrevious = from + "/";
                         MonthActual = to + "/";
                        String year = DateTime.Now.Year.ToString();

                        


                        if (from.Equals(to))
                        {
                            var QueryEmployeeHospitals = from ds in dbContext.t_beneficiaires
                                                     join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                     join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                     where voucher.C_datedeb.StartsWith(MonthActual) && voucher.C_datedeb.EndsWith(year)
                                                     select centreHealth.C_id_centre;



                            foreach (var item in QueryEmployeeHospitals.Distinct())
                            {
                                ctrDependecies = 0;
                                ctrEmployee = 0;
                                var QueryEmployee = from ds in dbContext.t_beneficiaires
                                                    join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                    join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                    where ds.C_id_succ.Equals(auth.Succursale) && voucher.C_datedeb.StartsWith(MonthActual)   && centreHealth.C_id_centre == item
                                                    select new { ds, voucher, centreHealth };


                                foreach (var itemE in QueryEmployee)
                                {
                                    ctrEmployee += 1;
                                }
                                var QueryDependents = from ds in dbContext.t_beneficiaires
                                                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                      join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                      where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_parent.Equals(null)
                                                      select new { ds, voucher, centreHealth };

                                foreach (var itemDep in QueryDependents)
                                {
                                    int idEmployee;
                                    if (!String.IsNullOrEmpty(itemDep.ds.C_id_parent))
                                    {
                                        idEmployee = int.Parse(itemDep.ds.C_id_parent);
                                        t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                              where ds.C_id_succ.Equals(auth.Succursale) && ds.C_id.Equals(idEmployee)
                                                              select ds).FirstOrDefault();
                                        if (em != null)
                                        {
                                            ctrDependecies += 1;
                                        }
                                    }


                                   
                                }
                                QueryDependents = from ds in dbContext.t_beneficiaires
                                                  join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                  join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                  where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_partenaire.Equals(null)
                                                         && ds.C_mat.Equals(null)
                                                  select new { ds, voucher, centreHealth };

                                foreach (var itemDep in QueryDependents)
                                {
                                    int idEmployee= 0;
                                    if (!String.IsNullOrEmpty(itemDep.ds.C_id_partenaire))
                                    {
                                        String succ = auth.Succursale;
                                        idEmployee = int.Parse(itemDep.ds.C_id_partenaire);
                                        t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                              where ds.C_id_succ.Equals(succ) && ds.C_id.Equals(idEmployee)
                                                              select ds).FirstOrDefault();
                                        if (em != null)
                                        {
                                            ctrDependecies += 1;
                                        }
                                    }
                                }

                                var row = dt.NewRow();
                                row["Company"] = auth.nameSuccursale;
                                row["Previous Month"] = "0";
                                row["Actual Month"] = ctrDependecies + ctrEmployee;
                                row["Hospital"] = dbContext.t_centre_soins.Where(e => e.C_id_centre == item).FirstOrDefault().C_name;
                                dt.Rows.Add(row);

                            }


                        }
                        else
                        {
                            var QueryEmployeeHospitals = from ds in dbContext.t_beneficiaires
                                                         join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                         join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                         where voucher.C_datedeb.StartsWith(MonthPrevious) || voucher.C_datedeb.StartsWith(MonthActual) && voucher.C_datedeb.EndsWith(year)
                                                         select centreHealth.C_id_centre;



                            foreach (var item in QueryEmployeeHospitals.Distinct())
                            {
                                ctrDependecies = 0;
                                ctrEmployee = 0;
                                int ctrEmployee2=0, ctrDependecies2 = 0;
                                var QueryEmployeePrevious = from ds in dbContext.t_beneficiaires
                                                    join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                    join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                    where ds.C_id_succ.Equals(auth.Succursale) && voucher.C_datedeb.StartsWith(MonthPrevious) && centreHealth.C_id_centre == item
                                                    select new { ds, voucher, centreHealth };


                                foreach (var itemE in QueryEmployeePrevious)
                                {
                                    ctrEmployee += 1;
                                }
                                var QueryDependentsPrevious = from ds in dbContext.t_beneficiaires
                                                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                      join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                      where voucher.C_datedeb.StartsWith(MonthPrevious) && centreHealth.C_id_centre == item && !ds.C_id_parent.Equals(null)
                                                      select new { ds, voucher, centreHealth };

                                foreach (var itemDep in QueryDependentsPrevious)
                                {
                                    int idEmployee;
                                    if (!String.IsNullOrEmpty(itemDep.ds.C_id_parent))
                                    {
                                        idEmployee = int.Parse(itemDep.ds.C_id_parent);
                                        t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                              where ds.C_id_succ.Equals(auth.Succursale) && ds.C_id.Equals(idEmployee)
                                                              select ds).FirstOrDefault();
                                        if (em != null)
                                        {
                                            ctrDependecies += 1;
                                        }
                                    }



                                }
                                QueryDependentsPrevious = from ds in dbContext.t_beneficiaires
                                                  join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                  join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                  where voucher.C_datedeb.StartsWith(MonthPrevious) && centreHealth.C_id_centre == item && !ds.C_id_partenaire.Equals(null)
                                                         && ds.C_mat.Equals(null)
                                                  select new { ds, voucher, centreHealth };

                                foreach (var itemDep in QueryDependentsPrevious)
                                {
                                    int idEmployee = 0;
                                    if (!String.IsNullOrEmpty(itemDep.ds.C_id_partenaire))
                                    {
                                        String succ = auth.Succursale;
                                        idEmployee = int.Parse(itemDep.ds.C_id_partenaire);
                                        t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                              where ds.C_id_succ.Equals(succ) && ds.C_id.Equals(idEmployee)
                                                              select ds).FirstOrDefault();
                                        if (em != null)
                                        {
                                            ctrDependecies += 1;
                                        }
                                    }
                                }


                                // ACTUAL

                                var QueryEmployeeActual = from ds in dbContext.t_beneficiaires
                                                            join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                            join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                            where ds.C_id_succ.Equals(auth.Succursale) && voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item
                                                            select new { ds, voucher, centreHealth };


                                foreach (var itemE in QueryEmployeeActual)
                                {
                                    ctrEmployee2 += 1;
                                }
                                var QueryDependentsActual = from ds in dbContext.t_beneficiaires
                                                              join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                              join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                              where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_parent.Equals(null)
                                                              select new { ds, voucher, centreHealth };

                                foreach (var itemDep in QueryDependentsActual)
                                {
                                    int idEmployee;
                                    if (!String.IsNullOrEmpty(itemDep.ds.C_id_parent))
                                    {
                                        idEmployee = int.Parse(itemDep.ds.C_id_parent);
                                        t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                              where ds.C_id_succ.Equals(auth.Succursale) && ds.C_id.Equals(idEmployee)
                                                              select ds).FirstOrDefault();
                                        if (em != null)
                                        {
                                            ctrDependecies2 += 1;
                                        }
                                    }



                                }
                                QueryDependentsActual = from ds in dbContext.t_beneficiaires
                                                          join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                          join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                          where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_partenaire.Equals(null)
                                                                 && ds.C_mat.Equals(null)
                                                          select new { ds, voucher, centreHealth };

                                foreach (var itemDep in QueryDependentsActual)
                                {
                                    int idEmployee = 0;
                                    if (!String.IsNullOrEmpty(itemDep.ds.C_id_partenaire))
                                    {
                                        String succ = auth.Succursale;
                                        idEmployee = int.Parse(itemDep.ds.C_id_partenaire);
                                        t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                              where ds.C_id_succ.Equals(succ) && ds.C_id.Equals(idEmployee)
                                                              select ds).FirstOrDefault();
                                        if (em != null)
                                        {
                                            ctrDependecies2 += 1;
                                        }
                                    }
                                }

                                var row = dt.NewRow();
                                row["Company"] = auth.nameSuccursale;
                                row["Previous Month"] = ctrEmployee+ctrDependecies;
                                row["Actual Month"] = ctrDependecies2 + ctrEmployee2;
                                row["Hospital"] = dbContext.t_centre_soins.Where(e => e.C_id_centre == item).FirstOrDefault().C_name;
                                dt.Rows.Add(row);

                            }

                        }


                        var ws = workbook.Worksheets.Add(dt, auth.nameSuccursale);
                        dt.Clear();

                    }
                    else
                    {
                        foreach (var itemSucc in dbContext.t_succursales)
                        {
                            int ctrDependecies = 0;
                            int ctrEmployee = 0;

                            var MonthActual = DateTime.Now.Month.ToString();
                            string MonthPrevious = from + "/";
                            MonthActual = to + "/";
                            String year = DateTime.Now.Year.ToString();




                            if (from.Equals(to))
                            {
                                var QueryEmployeeHospitals = from ds in dbContext.t_beneficiaires
                                                             join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                             join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                             where voucher.C_datedeb.StartsWith(MonthActual) && voucher.C_datedeb.EndsWith(year)
                                                             select centreHealth.C_id_centre;



                                foreach (var item in QueryEmployeeHospitals.Distinct())
                                {
                                    ctrDependecies = 0;
                                    ctrEmployee = 0;
                                    var QueryEmployee = from ds in dbContext.t_beneficiaires
                                                        join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                        join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                        where ds.C_id_succ.Equals(itemSucc.C_id) && voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item
                                                        select new { ds, voucher, centreHealth };


                                    foreach (var itemE in QueryEmployee)
                                    {
                                        ctrEmployee += 1;
                                    }
                                    var QueryDependents = from ds in dbContext.t_beneficiaires
                                                          join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                          join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                          where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_parent.Equals(null)
                                                          select new { ds, voucher, centreHealth };

                                    foreach (var itemDep in QueryDependents)
                                    {
                                        int idEmployee;
                                        if (!String.IsNullOrEmpty(itemDep.ds.C_id_parent))
                                        {
                                            idEmployee = int.Parse(itemDep.ds.C_id_parent);
                                            t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                                  where ds.C_id_succ.Equals(itemSucc.C_id) && ds.C_id.Equals(idEmployee)
                                                                  select ds).FirstOrDefault();
                                            if (em != null)
                                            {
                                                ctrDependecies += 1;
                                            }
                                        }



                                    }
                                    QueryDependents = from ds in dbContext.t_beneficiaires
                                                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                      join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                      where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_partenaire.Equals(null)
                                                             && ds.C_mat.Equals(null)
                                                      select new { ds, voucher, centreHealth };

                                    foreach (var itemDep in QueryDependents)
                                    {
                                        int idEmployee = 0;
                                        if (!String.IsNullOrEmpty(itemDep.ds.C_id_partenaire))
                                        {
                                            
                                            idEmployee = int.Parse(itemDep.ds.C_id_partenaire);
                                            t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                                  where ds.C_id_succ.Equals(itemSucc.C_id) && ds.C_id.Equals(idEmployee)
                                                                  select ds).FirstOrDefault();
                                            if (em != null)
                                            {
                                                ctrDependecies += 1;
                                            }
                                        }
                                    }

                                    var row = dt.NewRow();
                                    row["Company"] = auth.nameSuccursale;
                                    row["Previous Month"] = "0";
                                    row["Actual Month"] = ctrDependecies + ctrEmployee;
                                    row["Hospital"] = dbContext.t_centre_soins.Where(e => e.C_id_centre == item).FirstOrDefault().C_name;
                                    dt.Rows.Add(row);

                                }


                            }
                            else
                            {
                                var QueryEmployeeHospitals = from ds in dbContext.t_beneficiaires
                                                             join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                             join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                             where voucher.C_datedeb.StartsWith(MonthPrevious) || voucher.C_datedeb.StartsWith(MonthActual) && voucher.C_datedeb.EndsWith(year)
                                                             select centreHealth.C_id_centre;



                                foreach (var item in QueryEmployeeHospitals.Distinct())
                                {
                                    ctrDependecies = 0;
                                    ctrEmployee = 0;
                                    int ctrEmployee2 = 0, ctrDependecies2 = 0;
                                    var QueryEmployeePrevious = from ds in dbContext.t_beneficiaires
                                                                join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                                join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                                where ds.C_id_succ.Equals(itemSucc.C_id) && voucher.C_datedeb.StartsWith(MonthPrevious) && centreHealth.C_id_centre == item
                                                                select new { ds, voucher, centreHealth };


                                    foreach (var itemE in QueryEmployeePrevious)
                                    {
                                        ctrEmployee += 1;
                                    }
                                    var QueryDependentsPrevious = from ds in dbContext.t_beneficiaires
                                                                  join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                                  join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                                  where voucher.C_datedeb.StartsWith(MonthPrevious) && centreHealth.C_id_centre == item && !ds.C_id_parent.Equals(null)
                                                                  select new { ds, voucher, centreHealth };

                                    foreach (var itemDep in QueryDependentsPrevious)
                                    {
                                        int idEmployee;
                                        if (!String.IsNullOrEmpty(itemDep.ds.C_id_parent))
                                        {
                                            idEmployee = int.Parse(itemDep.ds.C_id_parent);
                                            t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                                  where ds.C_id_succ.Equals(itemSucc.C_id) && ds.C_id.Equals(idEmployee)
                                                                  select ds).FirstOrDefault();
                                            if (em != null)
                                            {
                                                ctrDependecies += 1;
                                            }
                                        }



                                    }
                                    QueryDependentsPrevious = from ds in dbContext.t_beneficiaires
                                                              join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                              join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                              where voucher.C_datedeb.StartsWith(MonthPrevious) && centreHealth.C_id_centre == item && !ds.C_id_partenaire.Equals(null)
                                                                     && ds.C_mat.Equals(null)
                                                              select new { ds, voucher, centreHealth };

                                    foreach (var itemDep in QueryDependentsPrevious)
                                    {
                                        int idEmployee = 0;
                                        if (!String.IsNullOrEmpty(itemDep.ds.C_id_partenaire))
                                        {
                                            
                                            idEmployee = int.Parse(itemDep.ds.C_id_partenaire);
                                            t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                                  where ds.C_id_succ.Equals(itemSucc.C_id) && ds.C_id.Equals(idEmployee)
                                                                  select ds).FirstOrDefault();
                                            if (em != null)
                                            {
                                                ctrDependecies += 1;
                                            }
                                        }
                                    }


                                    // ACTUAL

                                    var QueryEmployeeActual = from ds in dbContext.t_beneficiaires
                                                              join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                              join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                              where ds.C_id_succ.Equals(itemSucc.C_id) && voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item
                                                              select new { ds, voucher, centreHealth };


                                    foreach (var itemE in QueryEmployeeActual)
                                    {
                                        ctrEmployee2 += 1;
                                    }
                                    var QueryDependentsActual = from ds in dbContext.t_beneficiaires
                                                                join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                                join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                                where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_parent.Equals(null)
                                                                select new { ds, voucher, centreHealth };

                                    foreach (var itemDep in QueryDependentsActual)
                                    {
                                        int idEmployee;
                                        if (!String.IsNullOrEmpty(itemDep.ds.C_id_parent))
                                        {
                                            idEmployee = int.Parse(itemDep.ds.C_id_parent);
                                            t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                                  where ds.C_id_succ.Equals(itemSucc.C_id) && ds.C_id.Equals(idEmployee)
                                                                  select ds).FirstOrDefault();
                                            if (em != null)
                                            {
                                                ctrDependecies2 += 1;
                                            }
                                        }



                                    }
                                    QueryDependentsActual = from ds in dbContext.t_beneficiaires
                                                            join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                                                            join centreHealth in dbContext.t_centre_soins on voucher.C_id_centre equals centreHealth.C_id_centre
                                                            where voucher.C_datedeb.StartsWith(MonthActual) && centreHealth.C_id_centre == item && !ds.C_id_partenaire.Equals(null)
                                                                   && ds.C_mat.Equals(null)
                                                            select new { ds, voucher, centreHealth };

                                    foreach (var itemDep in QueryDependentsActual)
                                    {
                                        int idEmployee = 0;
                                        if (!String.IsNullOrEmpty(itemDep.ds.C_id_partenaire))
                                        {
                                           
                                            idEmployee = int.Parse(itemDep.ds.C_id_partenaire);
                                            t_beneficiaires em = (from ds in dbContext.t_beneficiaires
                                                                  where ds.C_id_succ.Equals(itemSucc.C_id) && ds.C_id.Equals(idEmployee)
                                                                  select ds).FirstOrDefault();
                                            if (em != null)
                                            {
                                                ctrDependecies2 += 1;
                                            }
                                        }
                                    }

                                    var row = dt.NewRow();
                                    row["Company"] = itemSucc.C_name;
                                    row["Previous Month"] = ctrEmployee + ctrDependecies;
                                    row["Actual Month"] = ctrDependecies2 + ctrEmployee2;
                                    row["Hospital"] = dbContext.t_centre_soins.Where(e => e.C_id_centre == item).FirstOrDefault().C_name;
                                    dt.Rows.Add(row);

                                }

                            }


                            var ws = workbook.Worksheets.Add(dt, itemSucc.C_name);
                            dt.Clear();
                        }
                    }
                }


                MemoryStream strm = new MemoryStream();
                workbook.SaveAs(strm);

                return File(strm.GetBuffer(), "content-disposition", "PREVIOUS_AND_ACTUAL_VOUCHERS.xlsx");
            }
        }
       
        public ActionResult excelSuccursaleReporting()
        {
            return View();
        }
        //4. Consultancy Cost
        public ActionResult ConsultancyCost()
        {
            this.LoadMonthYear();
            return View();
        }
       [HttpPost]
        public ActionResult ConsultancyCost(MonthList fmy)
        {
            var workbook = new XLWorkbook();
            DataTable dte = new DataTable();
            dte.Columns.Add("Month");
            dte.Columns.Add("Year");
            dte.Columns.Add("Hospital");
            dte.Columns.Add("Consultation");
            dte.Columns.Add("# Consultation");
            dte.Columns.Add("Coast");
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            String dateString = "";
            int x = 0;
            List<ConsultancyCoast> lstConsult = new List<ConsultancyCoast>();
            decimal totalGeneral = 0.0M;
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    if (ModelState.IsValid)
                    {
                        int _from = int.Parse(fmy.FromMonth);
                        int _to = int.Parse(fmy.ToMonth);


                        var Query = from bonCmd in dbContext.t_bon_commandes
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    join employee in dbContext.t_beneficiaires on bonCmd.C_id_bene equals employee.C_id
                                    where facture.C_datefacture.EndsWith(fmy.year) && employee.C_id_succ.Equals(auth.Succursale)
                                    select new { bonCmd, hospital, facture };


                        var QueryH = from bonCmd in dbContext.t_bon_commandes
                                     join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                     join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                     join employee in dbContext.t_beneficiaires on bonCmd.C_id_bene equals employee.C_id
                                     where facture.C_datefacture.EndsWith(fmy.year) && employee.C_id_succ.Equals(auth.Succursale)
                                     select bonCmd.C_id_centre;

                        foreach (var item in QueryH.Distinct())
                        {
                            var QueryM = from bonCmd in dbContext.t_bon_commandes
                                         join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                         join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                         join employee in dbContext.t_beneficiaires on bonCmd.C_id_bene equals employee.C_id
                                         where facture.C_datefacture.EndsWith(fmy.year) && bonCmd.C_id_centre == item && employee.C_id_succ.Equals(auth.Succursale)
                                         select bonCmd.C_motif;


                            foreach (var itemM in QueryM.Distinct())
                            {

                                for (int i = _from; i <= _to; i++)
                                {
                                    string month = i + "/";
                                    var QueryB = from bonCmd in dbContext.t_bon_commandes
                                                 join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                                 join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                 join employee in dbContext.t_beneficiaires on bonCmd.C_id_bene equals employee.C_id
                                                 where facture.C_datefacture.EndsWith(fmy.year) && facture.C_datefacture.StartsWith(month) && bonCmd.C_motif.Equals(itemM)
                                                 && employee.C_id_succ.Equals(auth.Succursale) && hospital.C_id_centre==item && facture.C_cout!=null && bonCmd.C_motif.Equals(itemM)
                                                 orderby bonCmd.C_id_bon ascending
                                                 select new { bonCmd, hospital, facture };

                                    if (QueryB.ToList().Count > 0)
                                    {
                                        int idH = (int)item;
                                        double coast = 0.0;
                                        foreach (var itemCoast in QueryB)
                                        {
                                            coast += (double)itemCoast.facture.C_cout;
                                        }
                                        totalGeneral +=(decimal) coast;
                                       
                                     
                                        var row = dte.NewRow();
                                        row["Month"] = (fmy.getMonths().Where(months => months.id.Equals(i.ToString()))).FirstOrDefault().name;
                                        row["Year"] = fmy.year;
                                        row["Hospital"] = dbContext.t_centre_soins.Where(e => e.C_id_centre == idH).FirstOrDefault().C_name;
                                        row["Consultation"] = itemM;
                                        row["# Consultation"] = QueryB.ToList().Count;
                                        row["Coast"] = String.Format("$ {0}", coast);
                                        dte.Rows.Add(row);
                                    }

                                }
                            }
                        }

                    }
                    var rowTotal2 = dte.NewRow();
                    rowTotal2["Month"] = "TOTAL GENERAL";
                    rowTotal2["Coast"] = "$"+totalGeneral;
                    dte.Rows.Add(rowTotal2);
                    MemoryStream strm2 = new MemoryStream();
                    workbook.AddWorksheet(dte, "Consultancy " + auth.nameSuccursale);
                    workbook.SaveAs(strm2);
                  //  this.LoadMonthYear();
                    return File(strm2.GetBuffer(), "content-disposition", "CONSULTANCY_COAST.xlsx");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        int _from = int.Parse(fmy.FromMonth);
                        int _to =int.Parse(fmy.ToMonth);


                        var Query = from bonCmd in dbContext.t_bon_commandes
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where facture.C_datefacture.EndsWith(fmy.year)
                                    select new { bonCmd, hospital, facture };


                        var QueryH = from bonCmd in dbContext.t_bon_commandes
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where facture.C_datefacture.EndsWith(fmy.year) 
                                    select bonCmd.C_id_centre;

                        foreach (var item in QueryH.Distinct())
                        {
                            var QueryM = from bonCmd in dbContext.t_bon_commandes
                                        join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                        join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                        where facture.C_datefacture.EndsWith(fmy.year) && bonCmd.C_id_centre==item
                                        select bonCmd.C_motif;

                        
                            foreach (var itemM in QueryM.Distinct())
                            {

                                for (int i = _from; i <= _to; i++)
                                {
                                    string month = i + "/";
                                    var QueryB = from bonCmd in dbContext.t_bon_commandes
                                                 join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                                 join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                 where facture.C_datefacture.EndsWith(fmy.year) && facture.C_datefacture.StartsWith(month) && bonCmd.C_motif.Equals(itemM)
                                                 && hospital.C_id_centre==item && facture.C_cout !=null && bonCmd.C_motif.Equals(itemM)
                                                 select new { bonCmd, hospital, facture };

                                    if (QueryB.ToList().Count>0)
                                    {
                                        int idH =(int) item;
                                        double coast = 0.0;
                                        foreach (var itemCoast in QueryB)
                                        {
                                            coast += (double)itemCoast.facture.C_cout;
                                        }
                                        totalGeneral += (decimal)coast;

                                        var row = dte.NewRow();
                                        row["Month"] = (fmy.getMonths().Where(months => months.id.Equals(i.ToString()))).FirstOrDefault().name;
                                        row["Year"] = fmy.year;
                                        row["Hospital"] = dbContext.t_centre_soins.Where(e => e.C_id_centre == idH).FirstOrDefault().C_name;
                                        row["Consultation"] = itemM;
                                        row["# Consultation"] = QueryB.ToList().Count;
                                        row["Coast"] = String.Format("$ {0}", coast);
                                        dte.Rows.Add(row);
                                    }
                                    
                                }
                            }
                        }

                    }
                }
                var rowTotal = dte.NewRow();
                rowTotal["Month"] = "TOTAL GENERAL";
                rowTotal["Coast"] = "$" + totalGeneral;
                dte.Rows.Add(rowTotal);
               
            }
            MemoryStream strm = new MemoryStream();
            workbook.AddWorksheet(dte, "Consultancy Coast");
            workbook.SaveAs(strm);
            //  this.LoadMonthYear();
            return File(strm.GetBuffer(), "content-disposition", "CONSULTANCY_COAST.xlsx");
            // byte[] bt = null;
            // var gv = new GridView();

            // gv.DataSource = lstConsult;
            // gv.DataBind();
            // Response.ClearContent();
            // Response.Buffer = true;
            // Response.AddHeader("content-disposition", "attachment; filename=REPORT_CONSULTANCY_COAST.xls");
            // Response.ContentType = "application/ms-excel";
            // Response.Charset = "";

            // StringWriter objStringWriter = new StringWriter();
            // HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            // gv.RenderControl(objHtmlTextWriter);
            // Response.Output.Write(objStringWriter.ToString());
            // Response.Flush();
            // Response.End();
            // bt = System.Text.Encoding.UTF8.GetBytes(objStringWriter.ToString());
            // return File(bt, "content-disposition");
            //// return (dateString=lstConsult.Count.ToString());
            // //return String.Format("From :{0} To:{1}", fmy.FromMonth, fmy.ToMonth);
        }

        public ActionResult excelConsultancyCost()
        {
            return View();
        }

        // 5. History of Medical vouchers and cost
        // 5.a. per employed
        public ActionResult HMVCEmployed()
        {
            return View();
        }

       [HttpPost]
        public ActionResult HMVCEmployed(DateFinding dFind)
        {
            var woorkbook = new XLWorkbook();
            DataTable dte = new DataTable();
            dte.Columns.Add("Date");
            dte.Columns.Add("Number ID");
            dte.Columns.Add("Employee Name");
            dte.Columns.Add("Hospital");
            dte.Columns.Add("Consultation");
            dte.Columns.Add("Coast");
            dte.Columns.Add("Voucher ID");
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            int x = 0;
            List<HMVC_Employee> HMVCE = new List<HMVC_Employee>();
            String[] tabF = dFind.from.Split('/');
            String[] tabT = dFind.to.Split('/');
            dFind.from = (int.Parse(tabF[0])).ToString() + "/" + tabF[1] + "/" + tabF[2];
            dFind.to= (int.Parse(tabT[0])).ToString() + "/" + tabT[1] + "/" + tabT[2];
            DateTime _from = DateTime.Parse(dFind.from);
            DateTime _to = DateTime.Parse(dFind.to);
            decimal totalGeneral = 0.0M;
            int totalVouchers = 0;
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"]; 
                if (auth.Priority.Equals("user"))
                {
                    if (ModelState.IsValid)
                    {
                        var Query = from employee in dbContext.t_beneficiaires
                                    join bonCmd in dbContext.t_bon_commandes on employee.C_id equals bonCmd.C_id_bene
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where employee.C_id_succ.Equals(auth.Succursale)
                                    select new { employee, facture, bonCmd, hospital };

                        foreach (var item in Query)
                        {
                            if (DateTime.Parse(item.bonCmd.C_datedeb) >= _from && DateTime.Parse(item.bonCmd.C_datedeb) <= _to)
                            {
                                var row = dte.NewRow();
                                row["Date"] = item.bonCmd.C_datedeb;
                                row["Number ID"] = item.employee.C_mat;
                                row["Employee Name"] = item.employee.C_name;
                                row["Hospital"] = item.hospital.C_name;
                                row["Consultation"] = item.bonCmd.C_motif;
                                row["Coast"] = "$"+item.facture.C_cout;
                                row["Voucher ID"] = item.bonCmd.C_id_bon.ToString(); 
                                dte.Rows.Add(row);
                                totalGeneral +=(decimal) item.facture.C_cout;
                                totalVouchers += 1;
                            }
                        }

                    }
                    var rowTotal = dte.NewRow();
                    rowTotal["Date"] = "TOTAL GENERAL";
                    rowTotal["Coast"] = "$"+totalGeneral;
                    rowTotal["Voucher ID"] = totalVouchers;
                    dte.Rows.Add(rowTotal);
                    woorkbook.AddWorksheet(dte, "VOUCHER_COST_Employees");

                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var Query = from employee in dbContext.t_beneficiaires
                                    join bonCmd in dbContext.t_bon_commandes on employee.C_id equals bonCmd.C_id_bene
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where !employee.C_id_succ.Equals(null)
                                    select new { employee, facture, bonCmd, hospital };

                        foreach (var item in Query)
                        {
                            if (DateTime.Parse(item.bonCmd.C_datedeb) >= _from && DateTime.Parse(item.bonCmd.C_datedeb) <= _to)
                            {
                                
                                var row = dte.NewRow();
                                row["Date"] = item.bonCmd.C_datedeb;
                                row["Number ID"] = item.employee.C_mat;
                                row["Employee Name"] = item.employee.C_name;
                                row["Hospital"] = item.hospital.C_name;
                                row["Consultation"] = item.bonCmd.C_motif;
                                row["Coast"] = "$" + item.facture.C_cout;
                                row["Voucher ID"] = item.bonCmd.C_id_bon.ToString();
                                dte.Rows.Add(row);
                                totalGeneral += (decimal)item.facture.C_cout;
                                totalVouchers += 1;
                            }
                        }

                        
                    }
                    
                    var rowTotal = dte.NewRow();
                    rowTotal["Date"] = "TOTAL GENERAL";
                    rowTotal["Coast"] = "$" + totalGeneral;
                    rowTotal["Voucher ID"] = totalVouchers;
                    dte.Rows.Add(rowTotal);
                    woorkbook.AddWorksheet(dte, "VOUCHER_COAST Employee");
                }
            }
            //return x;
           
            MemoryStream strm= new MemoryStream();
            woorkbook.SaveAs(strm);
            return File(strm.GetBuffer(), "content-disposition", "HOSPITAL MEDICAL VOUCHER EMPLOYEES.xlsx");
        }

        //5.b. Per Dependants
        public ActionResult HMVCDependecies()
        {
            return View();
        }

       [HttpPost]
        public ActionResult HMVCDependecies(DateFinding dFind)
        {
            var workbook = new XLWorkbook();
            DataTable dte = new DataTable();
            dte.Columns.Add("Id Number");
            dte.Columns.Add("Date");
            dte.Columns.Add("Dependent Name");
            dte.Columns.Add("Hospital");
            dte.Columns.Add("Consultation");
            dte.Columns.Add("Coast");
            dte.Columns.Add("Voucher ID");
            decimal totalGeneral = 0.0M;
            int totalVoucher = 0;
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            List<HMVC_Dependent> HMVCD = new List<HMVC_Dependent>();
            DateTime _from = DateTime.Parse(dFind.from);
            DateTime _to = DateTime.Parse(dFind.to);
            if (Session["userinfo"]!=null)
            {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    if (ModelState.IsValid)
                    {
                        var Query = from employee in dbContext.t_beneficiaires
                                    join bonCmd in dbContext.t_bon_commandes on employee.C_id equals bonCmd.C_id_bene
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where employee.C_id_succ.Equals(null) && employee.C_id_visitor.Equals(null) && (

                                    !employee.C_id_parent.Equals(null) || !employee.C_id_partenaire.Equals(null)
                                    )

                                    select new { employee, facture, bonCmd, hospital };
                        foreach (var itemP in Query)
                        {
                            if (!String.IsNullOrEmpty(itemP.employee.C_id_parent))
                            {
                                int idAgent = int.Parse(itemP.employee.C_id_parent);
                                var em = dbContext.t_beneficiaires.Where(p => p.C_id.Equals(idAgent)).FirstOrDefault();
                                if (em.C_id_succ.Equals(auth.Succursale))
                                {
                                    if (DateTime.Parse(itemP.bonCmd.C_datedeb) >= _from && DateTime.Parse(itemP.bonCmd.C_datedeb) <= _to)
                                    {
                                        
                                        var row = dte.NewRow();
                                        row[0] = em.C_mat;
                                        row[1] = itemP.bonCmd.C_datedeb;
                                        row[2] = itemP.employee.C_name;
                                        row[3] = itemP.hospital.C_name;
                                        row[4] = itemP.bonCmd.C_motif;
                                        row[5] = "$ " + itemP.facture.C_cout;
                                        row[6] = itemP.bonCmd.C_id_bon.ToString();
                                        dte.Rows.Add(row);
                                        totalGeneral += (decimal)itemP.facture.C_cout;
                                        totalVoucher += 1;
                                    }
                                }
                              

                            }
                            if (!String.IsNullOrEmpty(itemP.employee.C_id_partenaire))
                            {
                                int idAgent = int.Parse(itemP.employee.C_id_partenaire);
                                var em = dbContext.t_beneficiaires.Where(p => p.C_id.Equals(idAgent)).FirstOrDefault();
                                if (em.C_id_succ.Equals(auth.Succursale))
                                {
                                    if (DateTime.Parse(itemP.bonCmd.C_datedeb) >= _from && DateTime.Parse(itemP.bonCmd.C_datedeb) <= _to)
                                    {
                                        
                                        var row = dte.NewRow();
                                        row[0] = em.C_mat;
                                        row[1] = itemP.bonCmd.C_datedeb;
                                        row[2] = itemP.employee.C_name;
                                        row[3] = itemP.hospital.C_name;
                                        row[4] = itemP.bonCmd.C_motif;
                                        row[5] = "$ " + itemP.facture.C_cout;
                                        row[6] = itemP.bonCmd.C_id_bon.ToString();
                                        dte.Rows.Add(row);
                                        totalGeneral += (decimal)itemP.facture.C_cout;
                                        totalVoucher += 1;
                                    }
                                }
                              
                            }

                        }


                    }
                    var rowTotal = dte.NewRow();
                    rowTotal[0] = "TOTAL GENERAL";
                    rowTotal[5] = "$" + totalGeneral;
                    rowTotal[6] = totalVoucher;
                    dte.Rows.Add(rowTotal);
                    workbook.AddWorksheet(dte, "HMVC Dependents");
                } 
                else
                {
                    if (ModelState.IsValid)
                    {
                        var Query = from employee in dbContext.t_beneficiaires
                                    join bonCmd in dbContext.t_bon_commandes on employee.C_id equals bonCmd.C_id_bene
                                    join hospital in dbContext.t_centre_soins on bonCmd.C_id_centre equals hospital.C_id_centre
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where employee.C_id_succ.Equals(null) && employee.C_id_visitor.Equals(null) && (

                                    !employee.C_id_parent.Equals(null) || !employee.C_id_partenaire.Equals(null)
                                    )

                                    select new { employee, facture, bonCmd, hospital };
                        foreach (var itemP in Query)
                        {
                            if (!String.IsNullOrEmpty(itemP.employee.C_id_parent))
                            {
                                int idAgent = int.Parse(itemP.employee.C_id_parent);
                                var em = dbContext.t_beneficiaires.Where(p => p.C_id.Equals(idAgent)).FirstOrDefault();
                                if (DateTime.Parse(itemP.bonCmd.C_datedeb) >= _from && DateTime.Parse(itemP.bonCmd.C_datedeb) <= _to)
                                {
                                    var row = dte.NewRow();
                                    row[0] = em.C_mat;
                                    row[1] = itemP.bonCmd.C_datedeb;
                                    row[2] = itemP.employee.C_name;
                                    row[3] = itemP.hospital.C_name;
                                    row[4] = itemP.bonCmd.C_motif;
                                    row[5] = "$ " + itemP.facture.C_cout;
                                    row[6] = itemP.bonCmd.C_id_bon.ToString();
                                    dte.Rows.Add(row);
                                    totalGeneral += (decimal)itemP.facture.C_cout;
                                    totalVoucher += 1;
                                }

                            }
                            if (!String.IsNullOrEmpty(itemP.employee.C_id_partenaire))
                            {
                                int idAgent = int.Parse(itemP.employee.C_id_partenaire);
                                var em = dbContext.t_beneficiaires.Where(p => p.C_id.Equals(idAgent)).FirstOrDefault();
                                if (DateTime.Parse(itemP.bonCmd.C_datedeb) >= _from && DateTime.Parse(itemP.bonCmd.C_datedeb) <= _to)
                                {
                                    var row = dte.NewRow();
                                    row[0] = em.C_mat;
                                    row[1] = itemP.bonCmd.C_datedeb;
                                    row[2] = itemP.employee.C_name;
                                    row[3] = itemP.hospital.C_name;
                                    row[4] = itemP.bonCmd.C_motif;
                                    row[5] = "$ " + itemP.facture.C_cout;
                                    row[6] = itemP.bonCmd.C_id_bon.ToString();
                                    dte.Rows.Add(row);
                                    totalGeneral += (decimal)itemP.facture.C_cout;
                                    totalVoucher += 1;
                                }
                            }
                          
                        }

                             

                    }
                    var rowTotal = dte.NewRow();
                    rowTotal[0] = "TOTAL GENERAL";
                    rowTotal[5] = "$"+totalGeneral;
                    rowTotal[6] = totalVoucher;
                    dte.Rows.Add(rowTotal);
                    workbook.AddWorksheet(dte, "HMVC Dependents");
                }
            }
          
            MemoryStream strm = new MemoryStream();
            workbook.SaveAs(strm);
            return File(strm.GetBuffer(), "content-disposition","HMVC Dependents.xlsx");
        }

        //5.c. Cost per departement par Company

        public ActionResult HMVCcostdepartmentCompany()
        {
            this.LoadMonthYear();
            return View();
        }

    

       [HttpPost]
        public ActionResult HMVCcostdepartmentCompany(MonthList mList)
        {
            var woorkbook = new XLWorkbook();
            DataTable dte = new DataTable();
            dte.Columns.Add("Date");
            dte.Columns.Add("Year");
            dte.Columns.Add("Department");
            dte.Columns.Add("Coast");
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            String Month = "";
            decimal? cost = 0.0M,totalGeneral=0.0M;
            List<Cost_department_company> LstCDC = new List<Cost_department_company>();
            if (ModelState.IsValid)
            {
                if (Session["userinfo"]!=null)
                {
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    if (auth.Priority.Equals("user"))
                    {
                        String _from = mList.FromMonth + "/";
                        String _to = mList.ToMonth + "/";
                        String year = mList.year;
                        var QueryCompany = dbContext.t_succursales.ToList();
                        var Query = from employee in dbContext.t_beneficiaires
                                    join company in dbContext.t_succursales on employee.C_id_succ equals company.C_id
                                    join department in dbContext.t_departement on employee.C_id_depart equals department.C_id
                                    join bonCmd in dbContext.t_bon_commandes on employee.C_id equals bonCmd.C_id_bene
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where bonCmd.C_datedeb.StartsWith(_from) || bonCmd.C_datedeb.StartsWith(_to) && employee.C_id_succ.Equals(auth.Succursale) && 
                                           bonCmd.C_datedeb.EndsWith(year)
                                    select new { employee, company, department, bonCmd, facture };

                        String MonthFrom = (mList.getMonths().Where(month => month.id.Equals(mList.FromMonth))).FirstOrDefault().name;
                        String MonthTo = (mList.getMonths().Where(month => month.id.Equals(mList.ToMonth))).FirstOrDefault().name;
                        Month = String.Format("From :{0} === To :{1}", MonthFrom, MonthTo);
                        String current = "";
                        var Query2 = dbContext.t_departement;
                        foreach (var item in Query2)
                        {
                            decimal? coster = 0.0M;
                            var sql = from ds in Query
                                      where ds.department.C_id.Equals(item.C_id) && ds.bonCmd.C_datedeb.Contains(_from)
                                      select new {ds.company,ds.facture };
                            foreach (var item2 in sql)
                            {
                                coster += item2.facture.C_cout;
                            }
                            if (coster>1)
                            {
                                 

                                var row = dte.NewRow();
                                row[0] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                row[1] = mList.year;
                                row[2] = item.C_id_depart;
                                row[3] = "$" + coster.ToString();
                                dte.Rows.Add(row);
                                totalGeneral += coster;
                            }

                        }
                        foreach (var item in Query2)
                        {
                            decimal? coster = 0.0M;
                            var sql = from ds in Query
                                      where ds.department.C_id.Equals(item.C_id) && ds.bonCmd.C_datedeb.Contains(_to)
                                      select ds;
                            foreach (var item2 in sql)
                            {
                                coster += item2.facture.C_cout;
                            }
                            if (coster>0)
                            {
                                var row = dte.NewRow();
                                row[0] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                row[1] = mList.year;
                                row[2] = item.C_id_depart;
                                row[3] = "$" + coster.ToString();
                                dte.Rows.Add(row);
                                totalGeneral += coster;
                            }
                        }
                        var rowT = dte.NewRow();
                        rowT[0] = "TOTAL GENERAL";
                        rowT[3] = totalGeneral;
                        dte.Rows.Add(rowT);
                        woorkbook.AddWorksheet(dte, auth.nameSuccursale);
                    }
                    else
                    {
                        String _from = mList.FromMonth + "/";
                        String _to = mList.ToMonth + "/";
                        String year = mList.year;
                        var QueryCompany = dbContext.t_succursales.ToList();
                        var Query = from employee in dbContext.t_beneficiaires
                                    join company in dbContext.t_succursales on employee.C_id_succ equals company.C_id
                                    join department in dbContext.t_departement on employee.C_id_depart equals department.C_id
                                    join bonCmd in dbContext.t_bon_commandes on employee.C_id equals bonCmd.C_id_bene
                                    join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                    where bonCmd.C_datedeb.StartsWith(_from) || bonCmd.C_datedeb.StartsWith(_to) && bonCmd.C_datedeb.EndsWith(year)
                                    select new { employee, company, department, bonCmd, facture };

                        String MonthFrom = (mList.getMonths().Where(month => month.id.Equals(mList.FromMonth))).FirstOrDefault().name;
                        String MonthTo = (mList.getMonths().Where(month => month.id.Equals(mList.ToMonth))).FirstOrDefault().name;
                        Month = String.Format("From :{0} === To :{1}", MonthFrom, MonthTo);
                        foreach (var itemCompany in QueryCompany)
                        {
                            totalGeneral = 0.0M;
                            var Query2 = dbContext.t_departement;
                        foreach (var item in Query2)
                        {
                            decimal? coster = 0.0M;
                            var sql = from ds in Query
                                      where ds.department.C_id.Equals(item.C_id) && ds.bonCmd.C_datedeb.Contains(_from) && ds.company.C_id.Equals(itemCompany.C_id)
                                      select new {ds.company,ds.facture };
                            foreach (var item2 in sql)
                            {
                                coster += item2.facture.C_cout;
                            }
                            if (coster>1)
                            {
                                    var row = dte.NewRow();
                                    row[0] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                    row[1] = mList.year;
                                    row[2] = item.C_id_depart;
                                    row[3] = "$" + coster.ToString();
                                    dte.Rows.Add(row);
                                    totalGeneral += coster;
                                }
                        }
                        foreach (var item in Query2)
                        {
                            decimal? coster = 0.0M;
                            var sql = from ds in Query
                                      where ds.department.C_id.Equals(item.C_id) && ds.bonCmd.C_datedeb.Contains(_to) && ds.company.C_id.Equals(itemCompany.C_id)
                                      select ds;
                            foreach (var item2 in sql)
                            {
                                coster += item2.facture.C_cout;
                            }
                            if (coster>0)
                            {
                                    var row = dte.NewRow();
                                    row[0] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                    row[1] = mList.year;
                                    row[2] = item.C_id_depart;
                                    row[3] = "$" + coster.ToString();
                                    dte.Rows.Add(row);
                                    totalGeneral += coster;
                            }
                        }
                            var rowT = dte.NewRow();
                            rowT[0] = "TOTAL GENERAL";
                            rowT[3] = totalGeneral;
                            dte.Rows.Add(rowT);
                            woorkbook.AddWorksheet(dte, itemCompany.C_name);
                            dte.Clear();
                        }
                        
                    }
                }

            }
            MemoryStream strm = new MemoryStream();
            woorkbook.SaveAs(strm);
            return File(strm.GetBuffer(), "content-disposition", "History_of_Medical_Vouchers and_Cost_company_per_department.xlsx");
            
        }
        //5.d. Cost per hospital
        public ActionResult HMVCCostHospital()
        {
            this.LoadMonthYear();
            return View();
        }
       
       [HttpPost]
       public ActionResult HMVCCostHospital(MonthList mList)
       {
            var workbook = new XLWorkbook();
            DataTable dte = new DataTable();
            dte.Columns.Add("Date");
            dte.Columns.Add("Year");
            dte.Columns.Add("Hospital");
            dte.Columns.Add("# Vouchers");
            dte.Columns.Add("Cost");
           
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            List<Cost_Per_Hospital> lstCostForHospital = new List<Cost_Per_Hospital>();
           int ctr=0;
           String Line = "";
           decimal? cost = 0.0M;
             String _from =mList.FromMonth + "/";
             String _to = mList.ToMonth + "/";
            String year = mList.year;
            int x = 0;
            if (ModelState.IsValid)
           {
                if (Session["userinfo"] != null)
                {
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    if (auth.Priority.Equals("user"))
                    {
                        String[] tab = mList.ArrayCost.Split(',');
                        decimal totalPerCategory = 0.0M;
                        decimal total_general = 0.0M;
                        t_beneficiaires ben=new t_beneficiaires();
                        if (tab.Length>1)
                        {
                            foreach (var itemTab in tab)
                            {
                                if (itemTab.Equals("beneficiairies"))
                                {
                                    var QueryTotalBenef =
                                            from beneficiairies in dbContext.t_beneficiaires
                                            join voucher in dbContext.t_bon_commandes on beneficiairies.C_id equals voucher.C_id_bene
                                            join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                            where invoice.C_datefacture.StartsWith(_from) || invoice.C_datefacture.StartsWith(_to)&& invoice.C_datefacture.EndsWith(year)


                                            select new { invoice, beneficiairies };

                                    if (QueryTotalBenef.ToList().Count > 0)
                                    {
                                        var employee = from em in QueryTotalBenef
                                                       where em.beneficiairies.C_id_succ.Equals(auth.Succursale)
                                                       select em;

                                        total_general += employee.Sum(s => s.invoice.C_cout).Value;

                                        var visitor = from visit in QueryTotalBenef
                                                      where visit.beneficiairies.C_id_visitor.Equals(auth.Succursale)
                                                      select visit;

                                        if (visitor.ToList().Count > 0)
                                        {
                                            total_general += visitor.Sum(s => s.invoice.C_cout).Value;

                                        }

                                        var spouse = from spouses in QueryTotalBenef
                                                     where !spouses.beneficiairies.C_id_partenaire.Equals(null) || !spouses.beneficiairies.C_id_parent.Equals(null)
                                                     select spouses;

                                        foreach (var item in spouse)
                                        {
                                            if (!String.IsNullOrEmpty(item.beneficiairies.C_id_partenaire))
                                            {
                                                int id = int.Parse(item.beneficiairies.C_id_partenaire);
                                                ben = dbContext.t_beneficiaires.Where(e => e.C_id.Equals(id)).FirstOrDefault();
                                                bool isAgent = false;
                                                if (!String.IsNullOrEmpty(ben.C_id_succ))
                                                {
                                                    isAgent = ben.C_id_succ.Equals(auth.Succursale);
                                                    if (isAgent)
                                                    {
                                                        total_general += (decimal)item.invoice.C_cout;
                                                    }
                                                }
                                               

                                            }

                                            if (!String.IsNullOrEmpty(item.beneficiairies.C_id_parent))
                                            {
                                                int id = int.Parse(item.beneficiairies.C_id_parent);
                                                bool isAgent = dbContext.t_beneficiaires.Where(e => e.C_id.Equals(id)).FirstOrDefault().C_id_succ.Equals(auth.Succursale);
                                                if (isAgent)
                                                {
                                                    total_general += (decimal)item.invoice.C_cout;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (itemTab.Equals("casual"))
                                {
                                    var casualInvoices = from invoices in dbContext.t_vouchers_casuals
                                                         where invoices.C_id_company.Equals(auth.Succursale)
                                                         select invoices;

                                    if (casualInvoices.ToList().Count > 0)
                                    {
                                        total_general += casualInvoices.Sum(s => s.C_cout).Value;

                                    }
                                }
                                if (itemTab.Equals("contractor"))
                                {
                                    var contractorInvoices = from contractor in dbContext.t_contractor
                                                             join employee in dbContext.employee_contractor on contractor.C_id equals employee.C_idContractor
                                                             join invoices in dbContext.t_vouchers_contractor on employee.C_id equals invoices.C_id_Employed
                                                             where contractor.C_idSucc.Equals(auth.Succursale) && invoices.C_cout!=null
                                                             select invoices;

                                    if (contractorInvoices.ToList().Count > 0)
                                    {
                                        total_general += contractorInvoices.Sum(s => s.C_cout).Value;
                                    }
                                }
                             
                            }



                      
                            DataTable dt = new DataTable();
                            dt.Columns.Add("TOTAL GENERAL");
                            var rowTotal = dt.NewRow();
                            rowTotal["TOTAL GENERAL"] = "$" +total_general;
                            dt.Rows.Add(rowTotal);
                            workbook.AddWorksheet(dt, "TOTAL GENERAL");
                        }
                        foreach (var itemArrayCost in tab)
                        {
                            switch (itemArrayCost)
                            {
                                case "beneficiairies":
                                    var Query = from hospital in dbContext.t_centre_soins
                                                join bonCmd in dbContext.t_bon_commandes on hospital.C_id_centre equals bonCmd.C_id_centre
                                                join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                join benef in dbContext.t_beneficiaires on bonCmd.C_id_bene equals benef.C_id
                                                where facture.C_datefacture.StartsWith(_from) || facture.C_datefacture.StartsWith(_to) &&
                                                      facture.C_datefacture.EndsWith(year) && benef.C_id_visitor.Equals(null) && benef.C_id_succ.Equals(auth.Succursale)

                                                select new { facture, hospital, bonCmd };

                                    if (_from.Equals(_to))
                                    {
                                        Query = from hospital in dbContext.t_centre_soins
                                                join bonCmd in dbContext.t_bon_commandes on hospital.C_id_centre equals bonCmd.C_id_centre
                                                join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                join benef in dbContext.t_beneficiaires on bonCmd.C_id_bene equals benef.C_id
                                                where facture.C_datefacture.StartsWith(_from) &&
                                                      facture.C_datefacture.EndsWith(year) && benef.C_id_visitor.Equals(null)

                                                select new { facture, hospital, bonCmd };
                                    }
                                    var QueryHospital = dbContext.t_centre_soins.ToList();
                                    foreach (var itemH in QueryHospital)
                                    {

                                    }
                                    var QueryDistinct = from hospital in dbContext.t_centre_soins
                                                        join bonCmd in dbContext.t_bon_commandes on hospital.C_id_centre equals bonCmd.C_id_centre
                                                        join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                        where facture.C_datefacture.StartsWith(_from) || facture.C_datefacture.StartsWith(_to) &&
                                                              facture.C_datefacture.EndsWith(year)
                                                        select bonCmd.C_id_centre;


                                    foreach (var item in QueryDistinct.Distinct())
                                    {
                                        int id = (int)item;
                                        var QueryFrom = from ds in Query
                                                        where ds.facture.C_datefacture.StartsWith(_from) && ds.bonCmd.C_id_centre == id
                                                        select ds;

                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.facture.C_cout).Value;
                                            totalPerCategory += totalVouchers;
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);
                                        }


                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var item in QueryDistinct.Distinct())
                                        {
                                            int id = (int)item;
                                            var QueryTo = from ds in Query
                                                          where ds.facture.C_datefacture.StartsWith(_to) && ds.bonCmd.C_id_centre == id
                                                          select ds;

                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.facture.C_cout).Value;
                                                totalPerCategory += totalVouchers;

                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }


                                        }
                                    }
                                    var rowT = dte.NewRow();
                                    rowT["Date"] = "TOTAL ";
                                    rowT["Cost"] = "$" + totalPerCategory;
                                    dte.Rows.Add(rowT);
                                    var ws = workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();
                                    break;
                                case "casual":
                                    totalPerCategory = 0.0M;
                                    var Query2 = from hospital in dbContext.t_centre_soins
                                                 join bonCmd in dbContext.t_vouchers_casuals on hospital.C_id_centre equals bonCmd.C_id_centre
                                                 join company in dbContext.t_succursales on bonCmd.C_id_company equals company.C_id
                                                 where (bonCmd.C_date_casual.StartsWith(_from) || bonCmd.C_date_casual.StartsWith(_to)) &&
                                                    bonCmd.C_date_casual.EndsWith(year) && bonCmd.C_cout != null && company.C_id.Equals(auth.Succursale)

                                                 select new { hospital, bonCmd };

                                    var QueryHospital2 = dbContext.t_centre_soins.ToList();
                                    var QueryDistinct2 = from hospital in dbContext.t_centre_soins
                                                         join bonCmd in dbContext.t_vouchers_casuals on hospital.C_id_centre equals bonCmd.C_id_centre
                                                         join company in dbContext.t_succursales on bonCmd.C_id_company equals company.C_id
                                                         where (bonCmd.C_date_casual.StartsWith(_from) || bonCmd.C_date_casual.StartsWith(_to)) &&
                                                            bonCmd.C_date_casual.EndsWith(year) && bonCmd.C_cout != null && company.C_id.Equals(auth.Succursale)
                                                         select bonCmd.C_id_centre;



                                    foreach (var item in QueryDistinct2.Distinct())
                                    {
                                        int id = (int)item;
                                        var QueryFrom = from ds in Query2
                                                        where ds.bonCmd.C_date_casual.StartsWith(_from) && ds.bonCmd.C_id_centre == id
                                                        select ds;

                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.bonCmd.C_cout).Value;
                                            totalPerCategory += totalVouchers;
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);
                                        }


                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var item in QueryDistinct2.Distinct())
                                        {
                                            int id = (int)item;
                                            var QueryTo = from ds in Query2
                                                          where ds.bonCmd.C_date_casual.StartsWith(_to) && ds.bonCmd.C_id_centre == id
                                                          select ds;

                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.bonCmd.C_cout).Value;
                                                totalPerCategory += totalVouchers;
                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }


                                        }
                                    }
                                    var rowCasual = dte.NewRow();
                                    rowCasual["Date"] = "TOTAL GENERAL";
                                    rowCasual["Cost"] = "$" + totalPerCategory;
                                    dte.Rows.Add(rowCasual);
                                    workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();

                                    break;

                                case "contractor":
                                    totalPerCategory = 0.0M;
                                    var Query3 = from hospital in dbContext.t_centre_soins
                                                 join bonCmd in dbContext.t_vouchers_contractor on hospital.C_id_centre equals bonCmd.C_id_centre
                                                 join contractor_employee in dbContext.employee_contractor on bonCmd.C_id_Employed equals contractor_employee.C_id
                                                 join contractor in dbContext.t_contractor on contractor_employee.C_idContractor equals contractor.C_id
                                                 join company in dbContext.t_succursales on contractor.C_idSucc equals company.C_id
                                                 where (bonCmd.C_datedeb.StartsWith(_from) || bonCmd.C_datedeb.StartsWith(_to)) &&
                                                    bonCmd.C_datedeb.EndsWith(year) && bonCmd.C_cout != null && company.C_id.Equals(auth.Succursale)
                                                 select new { hospital, bonCmd };

                                    var QueryHospital3 = dbContext.t_centre_soins.ToList();
                                    var QueryDistinct3 = from hospital in dbContext.t_centre_soins
                                                         join bonCmd in dbContext.t_vouchers_contractor on hospital.C_id_centre equals bonCmd.C_id_centre
                                                         join contractor_employee in dbContext.employee_contractor on bonCmd.C_id_Employed equals contractor_employee.C_id
                                                         join contractor in dbContext.t_contractor on contractor_employee.C_idContractor equals contractor.C_id
                                                         join company in dbContext.t_succursales on contractor.C_idSucc equals company.C_id
                                                         where (bonCmd.C_datedeb.StartsWith(_from) || bonCmd.C_datedeb.StartsWith(_to)) &&
                                                            bonCmd.C_datedeb.EndsWith(year) && bonCmd.C_cout != null && company.C_id.Equals(auth.Succursale)
                                                         select bonCmd.C_id_centre;



                                    foreach (var item in QueryDistinct3.Distinct())
                                    {
                                        int id = (int)item;
                                        var QueryFrom = from ds in Query3
                                                        where ds.bonCmd.C_datedeb.StartsWith(_from) && ds.bonCmd.C_id_centre == id
                                                        select ds;

                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.bonCmd.C_cout).Value;
                                            totalPerCategory += totalVouchers;
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);
                                        }


                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var item in QueryDistinct3.Distinct())
                                        {
                                            int id = (int)item;
                                            var QueryTo = from ds in Query3
                                                          where ds.bonCmd.C_datedeb.StartsWith(_to) && ds.bonCmd.C_id_centre == id
                                                          select ds;

                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.bonCmd.C_cout).Value;
                                                totalPerCategory += totalVouchers;
                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }


                                        }
                                    }
                                    var rowContractor = dte.NewRow();
                                    rowContractor["Date"] = "TOTAL GENERAL";
                                    rowContractor["Cost"] = "$" + totalPerCategory;
                                    dte.Rows.Add(rowContractor);
                                    workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();
                                    break;
                                case "visitor":
                                    var Query4 = from visitor in dbContext.t_beneficiaires
                                                 join voucher in dbContext.t_bon_commandes on visitor.C_id equals voucher.C_id_bene
                                                 join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                 join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                 where (invoice.C_datefacture.StartsWith(_from) || invoice.C_datefacture.StartsWith(_to)) &&
                                                    invoice.C_datefacture.EndsWith(year) && visitor.C_id_visitor.Equals(auth.Succursale)

                                                 select new { invoice, hospital, voucher };

                                    var QueryHospital4 = dbContext.t_centre_soins.ToList();

                                    var QueryH4 = from visitor in dbContext.t_beneficiaires
                                                  join voucher in dbContext.t_bon_commandes on visitor.C_id equals voucher.C_id_bene
                                                  join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                  join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                  where (invoice.C_datefacture.StartsWith(_from) || invoice.C_datefacture.StartsWith(_to)) &&
                                                     invoice.C_datefacture.EndsWith(year) && visitor.C_id_visitor.Equals(auth.Succursale)
                                                  select hospital.C_id_centre;

                                    foreach (var itemHospital in QueryH4.Distinct())
                                    {
                                        var QueryFrom = from ds in Query4
                                                        where ds.invoice.C_datefacture.StartsWith(_from) && ds.voucher.C_id_centre == itemHospital
                                                        select ds;
                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.invoice.C_cout).Value;
                                            x += (int)totalVouchers;
                                            totalPerCategory += totalVouchers;
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);


                                        }

                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var itemHospital in QueryH4.Distinct())
                                        {
                                            var QueryTo = from ds in Query4
                                                          where ds.invoice.C_datefacture.StartsWith(_to) && ds.voucher.C_id_centre == itemHospital
                                                          select ds;
                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.invoice.C_cout).Value;
                                                totalPerCategory += totalVouchers;
                                                x += (int)totalVouchers;
                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }

                                        }
                                        
                                    }
                                    var rowT2 = dte.NewRow();
                                    rowT2["Date"] = "TOTAL ";
                                    rowT2["Cost"] = "$"+x.ToString() ;
                                    dte.Rows.Add(rowT2);
                                    workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();
                                    break;
                                default:
                                    break;
                            }
                        }
                        this.LoadMonthYear();
                        MemoryStream strm = new MemoryStream();
                        workbook.SaveAs(strm);
                        this.LoadMonthYear();
                        return File(strm.GetBuffer(), "content-disposition", "HOSPITAL_MEDICAL_VOUCHERS_COST.xlsx");
                    }
                    else
                    {
                        decimal total_general = 0.0M;
                        String[] tab = mList.ArrayCost.Split(','); 
                        if (tab.Length>1)
                        {
                            foreach (var itemTab in tab)
                            {
                                if (itemTab.Equals("beneficiairies"))
                                {
                                    var QueryTotal = from voucher in dbContext.t_bon_commandes
                                                     join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                     join benef in dbContext.t_beneficiaires on voucher.C_id_bene equals benef.C_id
                                                     where benef.C_id_visitor.Equals(null) && invoice.C_datefacture.StartsWith(_from) || invoice.C_datefacture.StartsWith(_to)&& invoice.C_datefacture.EndsWith(year)

                                                     select invoice;
                                    if (_from.Equals(_to))
                                    {
                                        QueryTotal = from voucher in dbContext.t_bon_commandes
                                                     join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                     join benef in dbContext.t_beneficiaires on voucher.C_id_bene equals benef.C_id
                                                     where benef.C_id_visitor.Equals(null) && invoice.C_datefacture.StartsWith(_from) && invoice.C_datefacture.EndsWith(year)

                                                     select invoice;

                                    }

                                    if (QueryTotal.ToList().Count > 0)
                                    {
                                        foreach (var itemC in QueryTotal)
                                        {
                                            if (itemC.C_cout!=null)
                                            {
                                                total_general += (decimal)itemC.C_cout;
                                            }

                                        }

                                    }
                                }


                                if (itemTab.Equals("visitor"))
                                {
                                    var QueryTotal = from voucher in dbContext.t_bon_commandes
                                                     join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                     join benef in dbContext.t_beneficiaires on voucher.C_id_bene equals benef.C_id
                                                     where (voucher.C_datedeb.StartsWith(_from) || voucher.C_datedeb.StartsWith(_to))
                                                          && !benef.C_id_visitor.Equals(null) && voucher.C_datedeb.EndsWith(year)

                                                     select invoice;

                                    if (QueryTotal.ToList().Count > 0)
                                    {
                                        foreach (var itemC in QueryTotal)
                                        {
                                            if (itemC.C_cout != null)
                                            {
                                                total_general += (decimal)itemC.C_cout;
                                            }

                                        }

                                    }
                                }

                                if (itemTab.Equals("casual"))
                                {
                                    var QueryTotalCasual = from vouchers in dbContext.t_vouchers_casuals
                                                           where vouchers.C_cout != null && vouchers.C_date_casual.StartsWith(_from) || vouchers.C_date_casual.StartsWith(_to)
                                                               && vouchers.C_date_casual.EndsWith(year)
                                                           select vouchers;

                                    if (QueryTotalCasual.ToList().Count > 0)
                                    {
                                        foreach (var itemC in QueryTotalCasual)
                                        {
                                            if (itemC.C_cout!=null)
                                            {
                                                total_general +=(decimal) itemC.C_cout;
                                            }
                                        }

                                    }
                                }
                                if (itemTab.Equals("contractor"))
                                {
                                    var QueryTotalContractor = from vouchers in dbContext.t_vouchers_contractor
                                                               where vouchers.C_datedeb.StartsWith(_from) || vouchers.C_datedeb.StartsWith(_to)
                                                               && vouchers.C_datedeb.EndsWith(year) && vouchers.C_cout != null
                                                               select vouchers;

                                    if (QueryTotalContractor.ToList().Count > 0)
                                    {
                                        total_general += QueryTotalContractor.Sum(s => s.C_cout).Value;
                                    }
                                }
                            }

                            DataTable dt = new DataTable();
                            dt.Columns.Add("TOTAL GENERAL");
                            var rowTotal = dt.NewRow();
                            rowTotal["TOTAL GENERAL"] = "$" + total_general;
                            dt.Rows.Add(rowTotal);
                            workbook.AddWorksheet(dt, "TOTAL GENERAL");
                        }
                        

                        

                        
                       
                        foreach (var itemArrayCost in tab)
                        {
                            decimal totalPerCategory=0.0M;
                            switch (itemArrayCost)
                            {
                                case "beneficiairies":
                                    var Query = from hospital in dbContext.t_centre_soins
                                                join bonCmd in dbContext.t_bon_commandes on hospital.C_id_centre equals bonCmd.C_id_centre
                                                join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                 join benef in dbContext.t_beneficiaires on bonCmd.C_id_bene equals benef.C_id
                                                where facture.C_datefacture.StartsWith(_from) || facture.C_datefacture.StartsWith(_to) &&
                                                      facture.C_datefacture.EndsWith(year) && benef.C_id_visitor.Equals(null)

                                                select new { facture, hospital, bonCmd };

                                    if (_from.Equals(_to))
                                    {
                                        Query = from hospital in dbContext.t_centre_soins
                                                join bonCmd in dbContext.t_bon_commandes on hospital.C_id_centre equals bonCmd.C_id_centre
                                                join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                join benef in dbContext.t_beneficiaires on bonCmd.C_id_bene equals benef.C_id
                                                where facture.C_datefacture.StartsWith(_from) &&
                                                      facture.C_datefacture.EndsWith(year) && benef.C_id_visitor.Equals(null)

                                                select new { facture, hospital, bonCmd };
                                    }
                                    var QueryHospital = dbContext.t_centre_soins.ToList();
                                    foreach (var itemH in QueryHospital)
                                    {

                                    }
                                    var QueryDistinct = from hospital in dbContext.t_centre_soins
                                                        join bonCmd in dbContext.t_bon_commandes on hospital.C_id_centre equals bonCmd.C_id_centre
                                                        join facture in dbContext.t_factures on bonCmd.C_id_bon equals facture.C_id_bon
                                                        where facture.C_datefacture.StartsWith(_from) || facture.C_datefacture.StartsWith(_to) &&
                                                              facture.C_datefacture.EndsWith(year)
                                                        select bonCmd.C_id_centre;


                                    foreach (var item in QueryDistinct.Distinct())
                                    {
                                        int id = (int)item;
                                        var QueryFrom = from ds in Query
                                                        where ds.facture.C_datefacture.StartsWith(_from) && ds.bonCmd.C_id_centre == id
                                                        select ds;

                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.facture.C_cout).Value;
                                            totalPerCategory += totalVouchers;                                         
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);
                                        }


                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var item in QueryDistinct.Distinct())
                                        {
                                            int id = (int)item;
                                            var QueryTo = from ds in Query
                                                          where ds.facture.C_datefacture.StartsWith(_to) && ds.bonCmd.C_id_centre == id
                                                          select ds;

                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.facture.C_cout).Value;
                                                totalPerCategory += totalVouchers;

                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }


                                        }
                                    }
                                    var rowT = dte.NewRow();
                                    rowT["Date"] = "TOTAL ";
                                    rowT["Cost"] = "$"+totalPerCategory;
                                    dte.Rows.Add(rowT);
                                    var ws = workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();
                                    break;
                                case "casual":
                                    totalPerCategory = 0.0M;
                                    var Query2 = from hospital in dbContext.t_centre_soins
                                                 join bonCmd in dbContext.t_vouchers_casuals on hospital.C_id_centre equals bonCmd.C_id_centre
                                                 where (bonCmd.C_date_casual.StartsWith(_from) || bonCmd.C_date_casual.StartsWith(_to)) &&
                                                    bonCmd.C_date_casual.EndsWith(year) && bonCmd.C_cout != null
                                                 select new { hospital, bonCmd };

                                    var QueryHospital2 = dbContext.t_centre_soins.ToList();
                                    var QueryDistinct2 = from hospital in dbContext.t_centre_soins
                                                          join bonCmd in dbContext.t_vouchers_casuals on hospital.C_id_centre equals bonCmd.C_id_centre
                                                          where (bonCmd.C_date_casual.StartsWith(_from) || bonCmd.C_date_casual.StartsWith(_to)) &&
                                                             bonCmd.C_date_casual.EndsWith(year) && bonCmd.C_cout!=null
                                                          select bonCmd.C_id_centre;
                                                          


                                    foreach (var item in QueryDistinct2.Distinct())
                                    {
                                        int id = (int)item;
                                        var QueryFrom = from ds in Query2
                                                        where ds.bonCmd.C_date_casual.StartsWith(_from) && ds.bonCmd.C_id_centre == id
                                                        select ds;

                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.bonCmd.C_cout).Value;
                                            totalPerCategory += totalVouchers;
                                            
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);
                                        }


                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var item in QueryDistinct2.Distinct())
                                        {
                                            int id = (int)item;
                                            var QueryTo = from ds in Query2
                                                          where ds.bonCmd.C_date_casual.StartsWith(_to) && ds.bonCmd.C_id_centre == id
                                                          select ds;

                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.bonCmd.C_cout).Value;
                                                totalPerCategory += totalVouchers;
                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }


                                        }
                                    }
                                    var rowTC = dte.NewRow();
                                    rowTC["Date"] = "TOTAL ";
                                    rowTC["Cost"] = "$" + totalPerCategory;
                                    dte.Rows.Add(rowTC);
                                    workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();

                                    break;

                                case "contractor":
                                    totalPerCategory = 0.0M;
                                    var Query3 = from hospital in dbContext.t_centre_soins
                                                 join bonCmd in dbContext.t_vouchers_contractor on hospital.C_id_centre equals bonCmd.C_id_centre
                                                 where (bonCmd.C_datedeb.StartsWith(_from) || bonCmd.C_datedeb.StartsWith(_to)) &&
                                                    bonCmd.C_datedeb.EndsWith(year) && bonCmd.C_cout != null
                                                 select new { hospital, bonCmd };

                                    var QueryHospital3 = dbContext.t_centre_soins.ToList();
                                    var QueryDistinct3 = from hospital in dbContext.t_centre_soins
                                                         join bonCmd in dbContext.t_vouchers_casuals on hospital.C_id_centre equals bonCmd.C_id_centre
                                                         where (bonCmd.C_date_casual.StartsWith(_from) || bonCmd.C_date_casual.StartsWith(_to)) &&
                                                            bonCmd.C_date_casual.EndsWith(year) && bonCmd.C_cout != null
                                                         select bonCmd.C_id_centre;



                                    foreach (var item in QueryDistinct3.Distinct())
                                    {
                                        int id = (int)item;
                                        var QueryFrom = from ds in Query3
                                                        where ds.bonCmd.C_datedeb.StartsWith(_from) && ds.bonCmd.C_id_centre == id
                                                        select ds;

                                        if (QueryFrom.ToList().Count > 0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.bonCmd.C_cout).Value;
                                            totalPerCategory += totalVouchers;
                                            
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);
                                        }


                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var item in QueryDistinct3.Distinct())
                                        {
                                            int id = (int)item;
                                            var QueryTo = from ds in Query3
                                                          where ds.bonCmd.C_datedeb.StartsWith(_to) && ds.bonCmd.C_id_centre == id
                                                          select ds;

                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.bonCmd.C_cout).Value;
                                                totalPerCategory += totalVouchers;
                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }


                                        }
                                    }
                                    var rowC = dte.NewRow();
                                    rowC["Date"] = "TOTAL ";
                                    rowC["Cost"] = "$" + totalPerCategory;
                                    dte.Rows.Add(rowC);
                                    workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();
                                    break;
                                case "visitor":
                                    totalPerCategory = 0.0M;
                                    var Query4 = from visitor in dbContext.t_beneficiaires
                                                 join voucher in dbContext.t_bon_commandes on visitor.C_id equals voucher.C_id_bene
                                                 join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                 join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                 where (invoice.C_datefacture.StartsWith(_from) || invoice.C_datefacture.StartsWith(_to)) &&
                                                    invoice.C_datefacture.EndsWith(year) && !visitor.C_id_visitor.Equals(null)

                                                 select new { invoice, hospital, voucher };

                                    var QueryHospital4 = dbContext.t_centre_soins.ToList();

                                    var QueryH4 = from visitor in dbContext.t_beneficiaires
                                                 join voucher in dbContext.t_bon_commandes on visitor.C_id equals voucher.C_id_bene
                                                 join invoice in dbContext.t_factures on voucher.C_id_bon equals invoice.C_id_bon
                                                 join hospital in dbContext.t_centre_soins on voucher.C_id_centre equals hospital.C_id_centre
                                                 where (voucher.C_datedeb.StartsWith(_from) || invoice.C_datefacture.StartsWith(_to)) &&
                                                    invoice.C_datefacture.EndsWith(year) && !visitor.C_id_visitor.Equals(null)
                                                 select hospital.C_id_centre;

                                    foreach (var itemHospital in QueryH4.Distinct())
                                    {
                                        var QueryFrom = from ds in Query4
                                                        where ds.invoice.C_datefacture.StartsWith(_from) && ds.voucher.C_id_centre == itemHospital
                                                        select ds;
                                        if (QueryFrom.ToList().Count>0)
                                        {
                                            int vouchers = QueryFrom.ToList().Count;
                                            decimal totalVouchers = QueryFrom.Sum(sum => sum.invoice.C_cout).Value;
                                            totalPerCategory += totalVouchers;
                                            var row = dte.NewRow();
                                            row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.FromMonth)).FirstOrDefault().name;
                                            row["Year"] = mList.year;
                                            row["Hospital"] = QueryFrom.FirstOrDefault().hospital.C_name;
                                            row["# Vouchers"] = vouchers.ToString();
                                            row["Cost"] = "$ " + totalVouchers.ToString();
                                            dte.Rows.Add(row);


                                        }
                                                     
                                    }

                                    if (!_from.Equals(_to))
                                    {
                                        foreach (var itemHospital in QueryH4.Distinct())
                                        {
                                            var QueryTo = from ds in Query4
                                                          where ds.invoice.C_datefacture.StartsWith(_to) && ds.voucher.C_id_centre == itemHospital
                                                          select ds;
                                            if (QueryTo.ToList().Count > 0)
                                            {
                                                int vouchers = QueryTo.ToList().Count;
                                                decimal totalVouchers = QueryTo.Sum(sum => sum.invoice.C_cout).Value;
                                                totalPerCategory += totalVouchers;
                                                var row = dte.NewRow();
                                                row["Date"] = mList.getMonths().Where(date => date.id.Equals(mList.ToMonth)).FirstOrDefault().name;
                                                row["Year"] = mList.year;
                                                row["Hospital"] = QueryTo.FirstOrDefault().hospital.C_name;
                                                row["# Vouchers"] = vouchers.ToString();
                                                row["Cost"] = "$ " + totalVouchers.ToString();
                                                dte.Rows.Add(row);
                                            }

                                        }
                                       
                                    }
                                    var rowV = dte.NewRow();
                                    rowV["Date"] = "TOTAL ";
                                    rowV["Cost"] = "$" + totalPerCategory;
                                    dte.Rows.Add(rowV);
                                    workbook.Worksheets.Add(dte, itemArrayCost);
                                    dte.Clear();
                                    break;
                                default:
                                    break;
                            }
                        }


                        MemoryStream strm = new MemoryStream();
                        workbook.SaveAs(strm);
                        this.LoadMonthYear();
                        return File(strm.GetBuffer(), "content-disposition", "HOSPITAL_MEDICAL_VOUCHERS_COST.xlsx");

                     
                    }
                }
                    
            
               //foreach (var item in QueryHospital)
               //{
               //    int id = item.C_id_centre;
               //    String currentMonth = "";
               //    var QueryList = Query.Where(hospital => hospital.bonCmd.C_id_centre.Equals(id));
               //   // int ctrVoucher = (QueryList == null ? 0 : QueryList.ToList().Count);
               //    foreach (var itemList in QueryList)
               //    {
               //        foreach (var itemCost in QueryList)
               //        {
               //            cost += itemCost.facture.C_cout;
               //        }
               //        Cost_Per_Hospital CPH = new Cost_Per_Hospital
               //        {
               //            Year = mList.year,
               //            Hospital = item.C_name,
               //            Vouchers = "2",
               //            Date = mList.getMonths().Where(date => date.id.Equals("")).FirstOrDefault().name
               //        };
               //        lstCostForHospital.Add(CPH);
               //    }
               //}

           }
            return View();

       }
        public ActionResult excelHMVC()
        {
            return View();
        }
        public ActionResult CostPerHospital()
        {
            this.LoadMonthYear();
            return View();
        }
       [HttpPost]
        public ActionResult CostPerHospital(MonthList mList)
        {
            return View();
        }

       public ActionResult Contractor()
       {
           return View();
       }
       [HttpPost]
       public ActionResult Contractor(t_contractor contractor)
       {
           if (Session["userinfo"]!=null)
           {
                Authenticate auth = (Authenticate)Session["userinfo"];
                if (ModelState.IsValid)
               {
                    if (contractor.C_id>0)
                   {
                        int id = contractor.C_id;
                        var Query = from ds in dbContext.t_contractor
                                    where ds.C_id.Equals(id)
                                    select ds;
                        foreach (var item in Query)
                        {
                            item.C_name = contractor.C_name;
                            item.C_phone = contractor.C_phone;
                            item.C_adresse = contractor.C_adresse;
                            item.C_status_system = contractor.C_status_system;
                            if (!auth.Priority.Equals("administrator"))
                            {
                                contractor.C_idSucc = auth.Succursale;

                            }
                            else
                            {
                                item.C_idSucc = contractor.C_idSucc;
                            }
                           

                        }
                        dbContext.SaveChanges();
                        ViewBag.result = "Contractor updated";

                        //T_logs logs = new T_logs()
                        //{
                        //    C_user = auth.username,
                        //    C_company = auth.nameSuccursale,
                        //    C_action = "Update Contractor",
                        //    C_date = DateTime.Now.ToShortDateString(),
                        //    C_time = DateTime.Now.ToShortTimeString(),
                        //    C_object = "Contractor",
                        //    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                        //};
                        //dbContext.T_logs.Add(logs);
                        //dbContext.SaveChanges();
                        if (contractor.C_status_system.Equals("0"))
                        {
                            var QueryEmployee = from contractor2 in dbContext.t_contractor
                                                join employee in dbContext.employee_contractor on contractor2.C_id equals employee.C_idContractor
                                                where employee.C_idContractor == contractor.C_id
                                                select employee;


                            foreach (var item in QueryEmployee)
                            {
                                item.C_status_system = contractor.C_status_system;

                            }
                            dbContext.SaveChanges();

                            //T_logs logs2 = new T_logs()
                            //{
                            //    C_user = auth.username,
                            //    C_company = auth.nameSuccursale,
                            //    C_action = "Update Contractor Employee",
                            //    C_date = DateTime.Now.ToShortDateString(),
                            //    C_time = DateTime.Now.ToShortTimeString(),
                            //    C_object = "Employee Contractor",
                            //    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                            //};
                            //dbContext.T_logs.Add(logs2);
                            //dbContext.SaveChanges();
                        }
                        


                    }
                    else
                    {
                        if (!auth.Priority.Equals("administrator"))
                        {
                            contractor.C_idSucc = auth.Succursale;

                        }
                        contractor.C_status_system = "1";
                        dbContext.t_contractor.Add(contractor);
                        dbContext.SaveChanges();
                        ViewBag.result = "Contractor Added";
                        //T_logs logs = new T_logs()
                        //{
                        //    C_user = auth.username,
                        //    C_company = auth.nameSuccursale,
                        //    C_action = "Add Contractor",
                        //    C_date = DateTime.Now.ToShortDateString(),
                        //    C_time = DateTime.Now.ToShortTimeString(),
                        //    C_object = "Contractor",
                        //    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.id)).FirstOrDefault().C_mat

                        //};
                        //dbContext.T_logs.Add(logs);
                        //dbContext.SaveChanges();
                    }
                   
                  
               }

           }
           return View();
       }
       public ActionResult VoucherContractor()
       {
           var QueryHospital = from ds in dbContext.t_centre_soins
                               where ds.C_status_system.Equals("1")
                               select ds;
           
           ViewData["listHospital"] = new SelectList(QueryHospital.ToList(), "C_id_centre", "C_name");
           List<String> lstMotif = new List<string>()
           {
               "GYNECHOLO",
               "PEDIATRIE",
               "OPHTAMOLOGY",
               "CARDIOLOGY"
           };
           ViewData["lstMotif"] = new SelectList(lstMotif);
           return View();
       }
       private byte[] PrintVoucherContractor(t_vouchers_contractor Voucher)
       {
           Authenticate auth = (Authenticate)Session["userinfo"];
           int id =(int) Voucher.C_id_Employed;
           var Query = (from company in dbContext.t_succursales
                        join contractor in dbContext.t_contractor
                        on company.C_id equals contractor.C_idSucc
                        join employee in dbContext.employee_contractor
                        on contractor.C_id equals employee.C_idContractor
                        join tVoucher in dbContext.t_vouchers_contractor
                        on employee.C_id equals tVoucher.C_id_Employed
                        join hospital in dbContext.t_centre_soins
                        on tVoucher.C_id_centre equals hospital.C_id_centre
                        where employee.C_id.Equals(id)
                        select new { company,employee,contractor,hospital }).FirstOrDefault();

           String fileName = String.Format("{0}.pdf", DateTime.Now.Millisecond);
           String realFilename = AppDomain.CurrentDomain.BaseDirectory + "/export/" + fileName;
           String pathLogo = AppDomain.CurrentDomain.BaseDirectory + "Images/logo2.jpg";
            String pathUser = AppDomain.CurrentDomain.BaseDirectory + "Images/user.png";
            Document _document = new Document();
           PdfWriter.GetInstance(_document, new System.IO.FileStream(realFilename, FileMode.Create));
           _document.AddCreationDate();
           _document.Open();
            PdfPTable tbhearder = new PdfPTable(2);

           Paragraph header_img = new Paragraph();
           Paragraph header_text = new Paragraph();
           header_text.Alignment = Element.ALIGN_CENTER;
           Paragraph header_text2 = new Paragraph();
           header_text2.Alignment = Element.ALIGN_CENTER;
            Chunk ch1 = new Chunk(Query.company.C_name,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA));
          //  ch1.SetUnderline(1.5f, 0.5f);
            float[] fw = new float[] { 20f, 40f };
            Phrase text_header = new Phrase(ch1);
           Phrase text_header2 = new Phrase( " \nFiliale Banro de corporation\n"+ Query.contractor.C_name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK));
           iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
           iTextSharp.text.Image imgUser = iTextSharp.text.Image.GetInstance(pathUser);
            img.Alignment = Element.ALIGN_LEFT;
            imgUser.Alignment = Element.ALIGN_LEFT;
            img.ScaleToFit(70f, 70f);
            imgUser.ScaleToFit(70f, 70f);
            header_text.Add(text_header);
            header_text.Add(text_header2);
            PdfPTable pTable = new PdfPTable(2);

            pTable.WidthPercentage = 100;
            pTable.DefaultCell.Border= iTextSharp.text.Rectangle.NO_BORDER;
            fw = new float[] { 20f,200f };
            pTable.SetWidths(fw);
            PdfPCell cellLog = new PdfPCell(img,false);
            cellLog.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pTable.AddCell(cellLog);
            PdfPCell headerCell = new PdfPCell();
            headerCell.AddElement(header_text);
            headerCell.Border= iTextSharp.text.Rectangle.NO_BORDER;
            //   headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //     headerCell.Border= iTextSharp.text.Rectangle.NO_BORDER;

            pTable.AddCell(headerCell);

            Phrase pictureText = new Phrase("PHOTO DU MALADE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,10,0,BaseColor.BLACK));
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            fw = new float[] { 20f, 25f };
            table.SetWidths(fw);
            PdfPCell cellUser = new PdfPCell(imgUser, false);
            Paragraph pVoucher = new Paragraph();
            Phrase p = new Phrase(new Chunk("BON DE SOINS\n",FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
            Phrase p2 = new Phrase("N° :"+Voucher.C_id_voucher+"\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            Phrase p3 = new Phrase(Query.hospital.C_name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            pVoucher.Add(p);
            pVoucher.Add(p2);
            pVoucher.Add(p3);
            cellUser.Border= iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cellUser);
            PdfPCell cellTextVoucher = new PdfPCell();
            cellTextVoucher.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellTextVoucher.AddElement(pVoucher);
            table.AddCell(cellTextVoucher);
            _document.Add(pTable);
            _document.Add(pictureText);
            _document.Add(table);
            PdfPTable Motif = new PdfPTable(4);
            Motif.PaddingTop = 10f;
           
            Motif.WidthPercentage = 100;
            Motif.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            
            fw = new float[] { 25f,100f,26f,70f };
            Motif.SetWidths(fw);
            //  Motif.LockedWidth = true;
            Motif.DefaultCell.PaddingTop = 15f;
            Motif.AddCell(
                            new Phrase("MOTIF :", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK))
                            );
            Chunk ch2 = new Chunk(Voucher.C_motif+"\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            ch2.SetUnderline(0.5f, -3.5f);
            Paragraph pChunk2 = new Paragraph(ch2);
            pChunk2.Alignment = Element.ALIGN_LEFT;
            Motif.AddCell(pChunk2);
            Motif.AddCell("SERVICE");
            Chunk ch3 = new Chunk(": "+Voucher.C_service);
            ch3.SetUnderline(0.5f, -3.5f);
            Motif.AddCell(new Phrase(ch3));
            _document.Add(Motif);
            Paragraph pGraph = new Paragraph();
            Phrase phrse = new Phrase("Veuillez examiner puis donner le traitement nécessaire à la personnnes suivante:\n\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10));
            pGraph.Alignment = Element.ALIGN_CENTER;
         
            pGraph.Add(phrse);
            _document.Add(pGraph);
            int old = 0;
            PdfPTable tableContent = new PdfPTable(4);
            tableContent.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tableContent.WidthPercentage = 100;
            tableContent.DefaultCell.Padding = 5f;
            tableContent.AddCell(new Phrase(new Chunk("Nom du Malade",FontFactory.GetFont(FontFactory.HELVETICA_BOLD,10))));
            tableContent.AddCell(new Phrase(new Chunk(":"+Voucher.employee_contractor.C_name, FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("Nom de l'agent ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":"+Voucher.employee_contractor.C_name, FontFactory.GetFont(FontFactory.HELVETICA, 8))));

            tableContent.AddCell(new Phrase(new Chunk("Identifiant", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": Contractant" , FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("Departement ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": "+Query.contractor.C_name, FontFactory.GetFont(FontFactory.HELVETICA, 8))));

            tableContent.AddCell(new Phrase(new Chunk("Age du malade", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": "+old.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("N° ID ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + Voucher.employee_contractor.C_id, FontFactory.GetFont(FontFactory.HELVETICA, 8))));

            tableContent.AddCell(new Phrase(new Chunk("Rep de BANRO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + auth.username, FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("Date", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));

            tableContent.AddCell(new Phrase(new Chunk("Signature", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":___________", FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk(" ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(" " , FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            _document.Add(tableContent);
            Paragraph pText = new Paragraph();
            Phrase phText = new Phrase("\nNGALIEMA, KINSHASA – RDC SIEGE ADMINISTRATIF: VILLA BRUPPACHER, Avenue MWANGA N0. 15, COMMUNE D’IBANDA, BUKAVU/ SUD-KIVU – RDC TEL. : +243 (0) 994059133 - +243 (0) 998665952 - +243 (0) 816942373 – FAX: 00243 (0) 812 616 096 SITEWEB: http://www.BANRO.COM"
                
                ,iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA,8)
                );
            pText.Alignment = Element.ALIGN_CENTER;
            pText.Add(phText);
            _document.Add(pText);
            phText = new Phrase("\nSiège Social:14, avenue Sergent Moke, Concession Safricas, commune de Ngaliema, Kinshasa, République Démocratique du Congo\n\n"

                    , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8)
                    );
            pText.Alignment = Element.ALIGN_CENTER;
            pText = new Paragraph();
            pText.Add(phText);
            _document.Add(pText);
            PdfPTable tbTicket = new PdfPTable(1);
            tbTicket.DefaultCell.Padding = 10f;
            PdfPCell cellTcket = new PdfPCell();
            String ticket = "Cette partie du feuillet est réservée au service Administratif de l’hopital – Date Réception du bon: ............................\n" +
                            "Services visités par le Malade :………................................              Signature de réception: .............................";
            Paragraph pTicket = new Paragraph(ticket, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            cellTcket.AddElement(pTicket);
            tbTicket.AddCell(cellTcket);
            tbTicket.WidthPercentage = 100;
            _document.Add(tbTicket);
       
            // pText.Add("___________________________________________________________________________________________________________________");
            //_document.Add(pText);

            pText = new Paragraph("Après examenn veuillez détacher puis retourner cette partie du feuillet auprès du département et des Ressources Humaines de " + Query.company.C_name + ".", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Alignment = Element.ALIGN_CENTER;
            pText.Add("AVERTISSEMENT- ANGALISHO");
            _document.Add(pText);
            pText.Alignment = Element.ALIGN_LEFT;
            pText = new Paragraph("",iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA,8));
            pText.Add("- Retirer un bon de soins en faveur d’une personne qui n’est pas reconnue par la compagnie est une tentative de fraude et par conséquent une faute lourde. Art. 74. Code du Travail\n");
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add("- Unapo cukuwa iyi bon de soins hakikisha kama matunzo ni ya mtu anaye stahili kutunzwa kwa na anjulikana na Kampuni. Kutunza mtu asiye julikana na kampuni ni wizi ama kosa kubwa mu kazi. art.74. code du travail.\n");
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add("Nom de l’agent : ……………………… Signature de l’Agent ou son délégué :………...............");
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8));
            pText.Add("NB: la Signature de l’agent ou son délégué est obligatoire.");
            Chunk chk = new Chunk(); ;
            for (int i = 0; i <=38; i++)
            {
                chk.Append("......");
            }
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8));
            pText.Add(chk);
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add(
                
                "Cette partie du feuillet est réservée au service Administratif de l’hopital                – Date Réception du bon: ............................\n"+


                "\nServices visités par le Malade :………................................                                  Signature de réception: .............................");
            _document.Add(pText);


            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8));
            pText.Add(chk);
            _document.Add(pText);
            pText= new Paragraph("Apres examen, veuillez détacher puis retourner cette partie du feuillet aupres de responsable H.R de Banro\n ", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            _document.Add(pText);
            pText = new Paragraph("Résultat & Detail du Traitement : .…………………………………………………………………………......\n .…………………………………………………………………………......\n", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 10));
            _document.Add(pText);
            PdfPTable tableF = new PdfPTable(3);
            pText = new Paragraph(
                "Rep. de Banro: ………………………………………  Nom de Médecin: …………………………  Date: ……………………………\n"
                , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));

            _document.Add(pText);

            pText = new Paragraph(
                "Nombre de jours de répos: ……………………………………………………………………………… \n\n"
                , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            _document.Add(pText);
            Phrase footer = new Phrase(
                 "\nID. NAT. 01-128-N40946U / RCCM. CD/KIN/RCCM/14 – B – 4004 / BP 13896 KINSHASA – RDC Villa Bruppacher, Avenue Mwanga N0. 15, Commune Dd’Ibanda, BUKAVU/ SUD-KIVU – RDC \n+243 (0) 816942373 – FAX: 00243 (0) 812 616 096 / http:/www.banro.com",
                  new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK)
                  );
            Paragraph prghFooter = new Paragraph(footer);
           
           prghFooter.Alignment = Element.ALIGN_CENTER;
           _document.Add(prghFooter);
           _document.CloseDocument();
           byte[] _buffer = System.IO.File.ReadAllBytes(realFilename);
          
           return _buffer;
       }

       private byte[] PrintVoucherCasual(t_vouchers_casuals casual)
       {
           Authenticate auth = (Authenticate)Session["userinfo"];
           int id = (int)casual.C_id_voucher;
           var Query = (from company in dbContext.t_succursales
                       join voucherCasual in dbContext.t_vouchers_casuals
                       on company.C_id equals voucherCasual.C_id_company
                       join hospital in dbContext.t_centre_soins
                       on voucherCasual.C_id_centre equals hospital.C_id_centre
                       where voucherCasual.C_id_voucher.Equals(id)
                       select new {company,voucherCasual,hospital }).FirstOrDefault();

           

            String fileName = String.Format("{0}.pdf", DateTime.Now.Millisecond);
            String realFilename = AppDomain.CurrentDomain.BaseDirectory + "/export/" + fileName;
            String pathLogo = AppDomain.CurrentDomain.BaseDirectory + "Images/logo2.jpg";
            String pathUser = AppDomain.CurrentDomain.BaseDirectory + "Images/user.png";
            Document _document = new Document();
            PdfWriter.GetInstance(_document, new System.IO.FileStream(realFilename, FileMode.Create));
            _document.AddCreationDate();
            _document.Open();
            PdfPTable tbhearder = new PdfPTable(2);

            Paragraph header_img = new Paragraph();
            Paragraph header_text = new Paragraph();
            header_text.Alignment = Element.ALIGN_CENTER;
            Paragraph header_text2 = new Paragraph();
            header_text2.Alignment = Element.ALIGN_CENTER;
            Chunk ch1 = new Chunk(Query.company.C_name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA));
            //  ch1.SetUnderline(1.5f, 0.5f);
            float[] fw = new float[] { 20f, 40f };
            Phrase text_header = new Phrase(ch1);
            Phrase text_header2 = new Phrase(" \nFiliale Banro de corporation\n"+casual.C_company_casual, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK));
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pathLogo);
            iTextSharp.text.Image imgUser = iTextSharp.text.Image.GetInstance(pathUser);
            img.Alignment = Element.ALIGN_LEFT;
            imgUser.Alignment = Element.ALIGN_LEFT;
            img.ScaleToFit(70f, 70f);
            imgUser.ScaleToFit(70f, 70f);
            header_text.Add(text_header);
            header_text.Add(text_header2);
            PdfPTable pTable = new PdfPTable(2);

            pTable.WidthPercentage = 100;
            pTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            fw = new float[] { 20f, 200f };
            pTable.SetWidths(fw);
            PdfPCell cellLog = new PdfPCell(img, false);
            cellLog.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pTable.AddCell(cellLog);
            PdfPCell headerCell = new PdfPCell();
            headerCell.AddElement(header_text);
            headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //   headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //     headerCell.Border= iTextSharp.text.Rectangle.NO_BORDER;

            pTable.AddCell(headerCell);

            Phrase pictureText = new Phrase("PHOTO DU MALADE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            fw = new float[] { 20f, 25f };
            table.SetWidths(fw);
            PdfPCell cellUser = new PdfPCell(imgUser, false);
            Paragraph pVoucher = new Paragraph();
            Phrase p = new Phrase(new Chunk("BON DE SOINS\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
            Phrase p2 = new Phrase("N° :" + casual.C_id_voucher + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            Phrase p3 = new Phrase(Query.hospital.C_name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            pVoucher.Add(p);
            pVoucher.Add(p2);
            pVoucher.Add(p3);
            cellUser.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cellUser);
            PdfPCell cellTextVoucher = new PdfPCell();
            cellTextVoucher.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellTextVoucher.AddElement(pVoucher);
            table.AddCell(cellTextVoucher);
            _document.Add(pTable);
            _document.Add(pictureText);
            _document.Add(table);
            PdfPTable Motif = new PdfPTable(4);
            Motif.PaddingTop = 10f;

            Motif.WidthPercentage = 100;
            Motif.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            fw = new float[] { 25f, 100f, 26f, 70f };
            Motif.SetWidths(fw);
            //  Motif.LockedWidth = true;
            Motif.DefaultCell.PaddingTop = 15f;
            Motif.AddCell(
                            new Phrase("MOTIF :", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK))
                            );
            Chunk ch2 = new Chunk(casual.C_cause + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, 0, BaseColor.BLACK));
            ch2.SetUnderline(0.5f, -3.5f);
            Paragraph pChunk2 = new Paragraph(ch2);
            pChunk2.Alignment = Element.ALIGN_LEFT;
            Motif.AddCell(pChunk2);
            Motif.AddCell("SERVICE");
            Chunk ch3 = new Chunk(": " + casual.C_service);
            ch3.SetUnderline(0.5f, -3.5f);
            Motif.AddCell(new Phrase(ch3));
            _document.Add(Motif);
            Paragraph pGraph = new Paragraph();
            Phrase phrse = new Phrase("Veuillez examiner puis donner le traitement nécessaire à la personnnes suivante:\n\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10));
            pGraph.Alignment = Element.ALIGN_CENTER;

            pGraph.Add(phrse);
            _document.Add(pGraph);
            int old = 0;
            if (!String.IsNullOrEmpty(casual.C_datenaiss))
            {
              old=DateTime.Now.Year - int.Parse(casual.C_datenaiss.Split('/')[2]);
            }
            PdfPTable tableContent = new PdfPTable(4);
            tableContent.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tableContent.WidthPercentage = 100;
            tableContent.DefaultCell.Padding = 5f;
            tableContent.AddCell(new Phrase(new Chunk("Nom du Malade", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + casual.C_name_casual, FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("Nom de l'agent ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + casual.C_name_casual, FontFactory.GetFont(FontFactory.HELVETICA, 8))));

            tableContent.AddCell(new Phrase(new Chunk("Identifiant", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": Occasionnel", FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("Departement ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": " + casual.C_department, FontFactory.GetFont(FontFactory.HELVETICA, 8))));

            tableContent.AddCell(new Phrase(new Chunk("Age du malade", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": " + old.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("N° ID ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(": _________", FontFactory.GetFont(FontFactory.HELVETICA, 8))));

            tableContent.AddCell(new Phrase(new Chunk("Rep de BANRO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + auth.username, FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk("Date", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":" + DateTime.Now.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));

            tableContent.AddCell(new Phrase(new Chunk("Signature", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(":___________", FontFactory.GetFont(FontFactory.HELVETICA, 8))));
            tableContent.AddCell(new Phrase(new Chunk(" ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            tableContent.AddCell(new Phrase(new Chunk(" ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
            _document.Add(tableContent);
            Paragraph pText = new Paragraph();
            Phrase phText = new Phrase("\nNGALIEMA, KINSHASA – RDC SIEGE ADMINISTRATIF: VILLA BRUPPACHER, Avenue MWANGA N0. 15, COMMUNE D’IBANDA, BUKAVU/ SUD-KIVU – RDC TEL. : +243 (0) 994059133 - +243 (0) 998665952 - +243 (0) 816942373 – FAX: 00243 (0) 812 616 096 SITEWEB: http://www.BANRO.COM"

                , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8)
                );
            pText.Alignment = Element.ALIGN_CENTER;
            pText.Add(phText);
            _document.Add(pText);
            phText = new Phrase("\nSiège Social:14, avenue Sergent Moke, Concession Safricas, commune de Ngaliema, Kinshasa, République Démocratique du Congo\n\n"

                    , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8)
                    );
            pText.Alignment = Element.ALIGN_CENTER;
            pText = new Paragraph();
            pText.Add(phText);
            _document.Add(pText);
            PdfPTable tbTicket = new PdfPTable(1);
            tbTicket.DefaultCell.Padding = 10f;
            PdfPCell cellTcket = new PdfPCell();
            String ticket = "Cette partie du feuillet est réservée au service Administratif de l’hopital – Date Réception du bon: ............................\n" +
                            "Services visités par le Malade :………................................              Signature de réception: .............................";
            Paragraph pTicket = new Paragraph(ticket, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            cellTcket.AddElement(pTicket);
            tbTicket.AddCell(cellTcket);
            tbTicket.WidthPercentage = 100;
            _document.Add(tbTicket);

            // pText.Add("___________________________________________________________________________________________________________________");
            //_document.Add(pText);

            pText = new Paragraph("Après examenn veuillez détacher puis retourner cette partie du feuillet auprès du département et des Ressources Humaines de " + Query.company.C_name + ".", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Alignment = Element.ALIGN_CENTER;
            pText.Add("AVERTISSEMENT- ANGALISHO");
            _document.Add(pText);
            pText.Alignment = Element.ALIGN_LEFT;
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add("- Retirer un bon de soins en faveur d’une personne qui n’est pas reconnue par la compagnie est une tentative de fraude et par conséquent une faute lourde. Art. 74. Code du Travail\n");
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add("- Unapo cukuwa iyi bon de soins hakikisha kama matunzo ni ya mtu anaye stahili kutunzwa kwa na anjulikana na Kampuni. Kutunza mtu asiye julikana na kampuni ni wizi ama kosa kubwa mu kazi. art.74. code du travail.\n");
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add("Nom de l’agent : ……………………… Signature de l’Agent ou son délégué :………...............");
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8));
            pText.Add("NB: la Signature de l’agent ou son délégué est obligatoire.");
            Chunk chk = new Chunk(); ;
            for (int i = 0; i <= 38; i++)
            {
                chk.Append("......");
            }
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8));
            pText.Add(chk);
            _document.Add(pText);
            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            pText.Add(

                "Cette partie du feuillet est réservée au service Administratif de l’hopital                – Date Réception du bon: ............................\n" +


                "\nServices visités par le Malade :………................................                                  Signature de réception: .............................");
            _document.Add(pText);


            pText = new Paragraph("", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8));
            pText.Add(chk);
            _document.Add(pText);
            pText = new Paragraph("Apres examen, veuillez détacher puis retourner cette partie du feuillet aupres de responsable H.R de Banro\n ", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            _document.Add(pText);
            pText = new Paragraph("Résultat & Detail du Traitement : .…………………………………………………………………………......\n .…………………………………………………………………………......\n", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 10));
            _document.Add(pText);
            PdfPTable tableF = new PdfPTable(3);
            pText = new Paragraph(
                "Rep. de Banro: ………………………………………  Nom de Médecin: …………………………  Date: ……………………………\n"
                , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));

            _document.Add(pText);

            pText = new Paragraph(
                "Nombre de jours de répos: ……………………………………………………………………………… \n\n"
                , iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8));
            _document.Add(pText);
            Phrase footer = new Phrase(
                 "\nID. NAT. 01-128-N40946U / RCCM. CD/KIN/RCCM/14 – B – 4004 / BP 13896 KINSHASA – RDC Villa Bruppacher, Avenue Mwanga N0. 15, Commune Dd’Ibanda, BUKAVU/ SUD-KIVU – RDC \n+243 (0) 816942373 – FAX: 00243 (0) 812 616 096 / http:/www.banro.com",
                  new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK)
                  );
            Paragraph prghFooter = new Paragraph(footer);

            prghFooter.Alignment = Element.ALIGN_CENTER;
            _document.Add(prghFooter);
            _document.CloseDocument();
            byte[] _buffer = System.IO.File.ReadAllBytes(realFilename);

            return _buffer;
        }
       public ActionResult VoucherCasual()
       {
           var QueryHospital = from ds in dbContext.t_centre_soins
                               where ds.C_status_system.Equals("1")
                               select ds;

           ViewData["listHospital"] = new SelectList(QueryHospital.ToList(), "C_id_centre", "C_name");
           List<String> lstMotif = new List<string>()
           {
               "GYNECHOLOGY",
               "PEDIATRIE",
               "OPHTAMOLOGY",
               "CARDIOLOGY"
           };
           ViewData["lstMotif"] = new SelectList(lstMotif);

           return View();
       }

       [HttpPost]
       public ActionResult VoucherCasual(t_vouchers_casuals voucher)
       {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
           if (ModelState.IsValid)
           {

               voucher.C_id_company = ((Authenticate)Session["userinfo"]).Succursale;
                if (String.IsNullOrEmpty(voucher.C_date_casual))
                {
                    voucher.C_date_casual = DateTime.Now.ToShortDateString();
                }
                else
                {
                    voucher.C_date_casual = DateTime.Parse(voucher.C_date_casual).ToShortDateString();

                }
                dbContext.t_vouchers_casuals.Add(voucher);
               dbContext.SaveChanges();
                Authenticate auth = (Authenticate)Session["userinfo"];
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_company = auth.nameSuccursale,
                    C_action = "Add New",
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_object = "Voucher Casual",
                    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();
            }
           byte[] _buffer = this.PrintVoucherCasual(voucher);
           String fileName=String.Format("{0}{1}",("MEDICAL_VOUCHER_CASUAL"+DateTime.Now),".pdf");
           return File(_buffer, "application/pdf", fileName);
       }

       [HttpPost]
       public ActionResult VoucherContractor(t_vouchers_contractor Voucher)
       {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            if (ModelState.IsValid)
           {
                Voucher.C_datedeb = DateTime.Parse(Voucher.C_datedeb).ToShortDateString();
               dbContext.t_vouchers_contractor.Add(Voucher);
               dbContext.SaveChanges();
               ViewBag.message = "Voucher Added";
                Authenticate auth = (Authenticate)Session["userinfo"];
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_company = auth.nameSuccursale,
                    C_action = "Add New",
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_object = "Voucher Contractor",
                    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();
            }
           var QueryHospital = from ds in dbContext.t_centre_soins
                               select ds;

           ViewData["listHospital"] = new SelectList(QueryHospital.ToList(), "C_id_centre", "C_name");
           List<String> lstMotif = new List<string>()
           {
               "GYNECHOLOGY",
               "PEDIATRIE",
               "OPHTAMOLOGY",
               "CARDIOLOGY"
           };
           ViewData["lstMotif"] = new SelectList(lstMotif);

           byte[] arrayBinary=this.PrintVoucherContractor(Voucher);
           String fileName = "VOUCHER_CONTRACTOR_" + DateTime.Now+".pdf";
           return File(arrayBinary,"Application/pdf",fileName);
       }
       public ActionResult AddEmployeeContractor()
       {
           if (Session["userinfo"]!=null)
           {
               Authenticate auth = (Authenticate)Session["userinfo"];
                if (auth.Priority.Equals("user"))
                {
                    var Query = from ds in dbContext.t_contractor
                                where ds.C_idSucc.Equals(auth.Succursale)
                                select ds;
                    ViewData["ListContractor"] = new SelectList(Query.ToList(), "C_id", "C_name");
                    this.LoadGenre();
                }
                else
                {
                    var Query = from ds in dbContext.t_contractor
                                select ds;
                    ViewData["ListContractor"] = new SelectList(Query.ToList(), "C_id", "C_name");
                    this.LoadGenre();
                }
              
           }
          
           return View();
       }

       [HttpPost]
       public ActionResult AddEmployeeContractor(employee_contractor EContractor)
       {
          
           if (ModelState.IsValid)
           {
                if (EContractor.C_id>0)
                {
                    var QueryEntreprise =( from employee in dbContext.employee_contractor
                                join entreprise in dbContext.t_contractor on employee.C_idContractor equals entreprise.C_id
                                where employee.C_id==EContractor.C_id
                                select entreprise).FirstOrDefault();

                    if (QueryEntreprise.C_status_system.Equals("1"))
                    {
                        var contractor = (from ds in dbContext.employee_contractor
                                          where ds.C_id == EContractor.C_id
                                          select ds).FirstOrDefault();

                        contractor.C_idContractor = EContractor.C_idContractor;
                        contractor.C_name = EContractor.C_name;
                        contractor.C_phone = EContractor.C_phone;
                        contractor.C_adresse = EContractor.C_adresse;
                        contractor.C_sex = EContractor.C_sex;
                        contractor.C_status_system = EContractor.C_status_system;
                        dbContext.SaveChanges();
                        ViewBag.message = "Contractor Employee modify";
                        Authenticate auth = (Authenticate)Session["userinfo"];
                        T_logs logs = new T_logs()
                        {
                            C_user = auth.username,
                            C_company = auth.nameSuccursale,
                            C_action = "Update Contractor Employee",
                            C_date = DateTime.Now.ToShortDateString(),
                            C_time = DateTime.Now.ToShortTimeString(),
                            C_object = "Employee_Contractor",
                            C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                        };
                        dbContext.T_logs.Add(logs);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        ViewBag.message = "500";
                        Authenticate auth = (Authenticate)Session["userinfo"];
                        T_logs logs = new T_logs()
                        {
                            C_user = auth.username,
                            C_company = auth.nameSuccursale,
                            C_action = "Update Contractor Employee| Failed",
                            C_date = DateTime.Now.ToShortDateString(),
                            C_time = DateTime.Now.ToShortTimeString(),
                            C_object = "Employee_Contractor",
                            C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.idEmpolyee)).FirstOrDefault().C_mat

                        };
                        dbContext.T_logs.Add(logs);
                        dbContext.SaveChanges();
                    }
                    
                }
                else
                {
                    EContractor.C_status_system = "1";
                    if (String.IsNullOrEmpty(EContractor.C_status_system))
                    {
                        EContractor.C_status_system = "1";
                    }
                    dbContext.employee_contractor.Add(EContractor);
                   
                    dbContext.SaveChanges();
                    Authenticate auth = (Authenticate)Session["userinfo"];
                    T_logs logs = new T_logs()
                    {
                        C_user = auth.username,
                        C_date = DateTime.Now.ToShortDateString(),
                        C_time = DateTime.Now.ToShortTimeString(),
                        C_action = "Add new",
                        C_object = "Employee Contractor",
                        C_company = auth.nameSuccursale,
                        C_mat = dbContext.t_beneficiaires.Where(id => id.C_id == auth.idEmpolyee).FirstOrDefault().C_mat

                    };
                    dbContext.T_logs.Add(logs);
                    dbContext.SaveChanges();
                }
              
               
           }
           var Query = from ds in dbContext.t_contractor
                       select ds;
           ViewData["ListContractor"] = new SelectList(Query.ToList(), "C_id", "C_name");
           this.LoadGenre();
           return View();
       }
       [HttpPost]
       public String ModifyHospital()
       {
           using (StreamReader sReader=new StreamReader(Request.InputStream))
           {
               var data = sReader.ReadToEnd();
               JavaScriptSerializer jSerial = new JavaScriptSerializer();
               t_centre_soins hospital = jSerial.Deserialize<t_centre_soins>(data);
               var Query = from ds in dbContext.t_centre_soins
                           where ds.C_id_centre.Equals(hospital.C_id_centre)
                           select ds;

               foreach (var item in Query)
               {
                   item.C_name = hospital.C_name;
                   item.adresse = hospital.adresse;
                   item.C_phone = hospital.C_phone;
                    item.C_status_system = hospital.C_status_system;
               }
               dbContext.SaveChanges();
           }
         
           return "200";
       }
       [Route("searchcasual")]
       public String SearchCasuals()
       {
           String data = String.Empty;
           if (Session["userinfo"]!=null)
           {
               Authenticate auth = (Authenticate)Session["userinfo"];
               if (auth.Priority.Equals("user"))
               {
                   var Query = from casual in dbContext.t_vouchers_casuals
                               join company in dbContext.t_succursales
                               on casual.C_id_company equals company.C_id
                               join hospital in dbContext.t_centre_soins
                               on casual.C_id_centre equals hospital.C_id_centre
                               where casual.C_id_company.Equals(auth.Succursale)
                               select new CasualObject
                               {
                                   idVoucher = casual.C_id_voucher
                                   ,
                                   NameCasual = casual.C_name_casual
                                   ,
                                   CompanyCasual = casual.C_company_casual
                                   ,
                                   DateCasual = casual.C_date_casual
                                   ,
                                   Motif = casual.C_motif
                                   ,
                                   idHospital = hospital.C_name
                                   ,
                                   idCompanyVisited = company.C_name,
                                   Cause = casual.C_cause,
                                   Cost=casual.C_cout
                               };

                   data= new JavaScriptSerializer().Serialize(Query.ToList());
               }
               else
               {
                   var Query = from casual in dbContext.t_vouchers_casuals
                               join company in dbContext.t_succursales
                                on casual.C_id_company equals company.C_id
                               join hospital in dbContext.t_centre_soins
                               on casual.C_id_centre equals hospital.C_id_centre
                               select new CasualObject
                               {
                                   idVoucher = casual.C_id_voucher
                                   ,
                                   NameCasual = casual.C_name_casual
                                   ,
                                   CompanyCasual = casual.C_company_casual
                                   ,
                                   DateCasual = casual.C_date_casual
                                   ,
                                   Motif = casual.C_motif
                                   ,idHospital = hospital.C_name
                  
                                   ,idCompanyVisited = company.C_name
                                   ,Cause = casual.C_cause
                               };

                   data = new JavaScriptSerializer().Serialize(Query.ToList());
               }
           }
           return data;
       }
        public ActionResult addlogger()
        {
            return View();
        }

       [HttpPost]
        public ActionResult addlogger(Models.t_logger logger)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    t_logger login = (from loggerfind in dbContext.t_logger
                                     where loggerfind.C_username.Equals(logger.C_username)
                                     select loggerfind).FirstOrDefault();
                    if (login!=null)
                    {
                        ViewBag.response = "202";
                    }
                    else
                    {
                        logger.password = HomeController.CryptoMD5(logger.password);
                        logger.C_status_system = "1";
                        dbContext.t_logger.Add(logger);
                        dbContext.SaveChanges();
                        ViewBag.response = "200";
                        Authenticate auth = (Authenticate)Session["userinfo"];
                        T_logs logs = new T_logs()
                        {
                            C_user = auth.username,
                            C_date = DateTime.Now.ToShortDateString(),
                            C_time = DateTime.Now.ToShortTimeString(),
                            C_action = "Add new",
                            C_object = "User Login",
                            C_company = auth.nameSuccursale,
                            C_mat = dbContext.t_beneficiaires.Where(id => id.C_id == auth.id).FirstOrDefault().C_mat

                        };
                        dbContext.T_logs.Add(logs);
                        dbContext.SaveChanges();
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    ViewBag.response = ex.Message;
                    
                }
            }
            else
            {
                ViewBag.response = "400";
            }
            return View();
        }
        public ActionResult updatelogger()
        {

            return View();
        }

        [HttpPost]
        public ActionResult updatelogger(t_logger logger)
        {
            t_logger Query = (from login in dbContext.t_logger
                        where login.C_employeeId==logger.C_employeeId
                        select login).FirstOrDefault();

            if (Query!=null)
            {
                Query.C_username = logger.C_username;
                if (!String.IsNullOrEmpty(logger.password))
                {
                    Query.password = HomeController.CryptoMD5(logger.password);
                }
                
                if (logger.C_priority!=null)
                {
                    Query.C_priority = logger.C_priority;
                }
                Query.C_status_system = logger.C_status_system;
                dbContext.SaveChanges();
                ViewBag.response = "200";
                Authenticate auth = (Authenticate)Session["userinfo"];
                T_logs logs = new T_logs()
                {
                    C_user = auth.username,
                    C_company = auth.nameSuccursale,
                    C_action = "Update Account Login",
                    C_date = DateTime.Now.ToShortDateString(),
                    C_time = DateTime.Now.ToShortTimeString(),
                    C_object = "Login",
                    C_mat = dbContext.t_beneficiaires.Where(idBenef => idBenef.C_id.Equals(auth.id)).FirstOrDefault().C_mat

                };
                dbContext.T_logs.Add(logs);
                dbContext.SaveChanges();
            }
            else
            {
                ViewBag.response = "202";

            }

            return View();
        }
        private static String CryptoMD5(String input)
       {
           StringBuilder builder = new StringBuilder();
           System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
           byte[] bt = md5.ComputeHash(new UTF8Encoding().GetBytes(input));
           for (int i = 0; i < bt.Length; i++)
           {
               builder.Append(bt[i].ToString("x2"));
           }
           return builder.ToString();
       }
        [Route("Home/listusersaccount/{idSucc}")]
        public String getListUsersAccount(String idSucc)
        { 
            var Query = from ds in dbContext.t_beneficiaires
                        join ds2 in dbContext.t_succursales
                        on ds.C_id_succ equals ds2.C_id
                        join login in dbContext.t_logger
                        on ds.C_id equals login.C_employeeId
                        into login from subLogger in login.DefaultIfEmpty()
                        where ds2.C_name.Equals(idSucc) && subLogger.C_username.Equals(null)
                        select new { idEmployee = ds.C_id, nameEmployee = ds.C_name,idCompany=ds.C_id_succ,username=subLogger.C_username };

            List<Dictionary<string, string>> ArrayJSON = new List<Dictionary<string, string>>();
           
            foreach (var item in Query)
            {
                Dictionary<String, String> listgetter = new Dictionary<string, string>();
                listgetter.Add("id", item.idEmployee.ToString());
                listgetter.Add("name", item.nameEmployee);
                listgetter.Add("idCompany", item.idCompany);
                listgetter.Add("user", item.username);
                ArrayJSON.Add(listgetter);
            }
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            var data = jserializer.Serialize(ArrayJSON);
            return data;
        }

        [Route("Home/listusersaccountexist/{idSucc}")]
        public String getListUsersAccountExist(String idSucc)
        {
            var Query = from ds in dbContext.t_beneficiaires
                        join ds2 in dbContext.t_succursales
                        on ds.C_id_succ equals ds2.C_id
                        join login in dbContext.t_logger
                        on ds.C_id equals login.C_employeeId
                        into login
                        from subLogger in login.DefaultIfEmpty()
                        where ds2.C_name.Equals(idSucc) && !subLogger.C_username.Equals(null)
                        select new { idEmployee = ds.C_id, nameEmployee = ds.C_name, idCompany = ds.C_id_succ, username = subLogger.C_username, priority = subLogger.C_priority,status=subLogger.C_status_system };

            List<Dictionary<string, string>> ArrayJSON = new List<Dictionary<string, string>>();

            foreach (var item in Query)
            {
                Dictionary<String, String> listgetter = new Dictionary<string, string>();
                listgetter.Add("id", item.idEmployee.ToString());
                listgetter.Add("name", item.nameEmployee);
                listgetter.Add("idCompany", item.idCompany);
                listgetter.Add("user", item.username);
                listgetter.Add("priority", item.priority);
                listgetter.Add("status", item.status);
                ArrayJSON.Add(listgetter);
            }
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            var data = jserializer.Serialize(ArrayJSON);
            return data;
        }
        public String DeleteAction(String id,String type)
        {
            String returnValue = "";
            switch (type)
            {
                case "company":
                    this.DelCompany(id);
                    break;

                case "department":

                    break;
                default:
                    break;
            }
            return returnValue;
        }

        private bool DelCompany(String id)
        {
           
            var Query = from ds in dbContext.t_succursales
                        where ds.C_id == id
                        select ds;

            foreach (var item in Query)
            {
                dbContext.t_succursales.Remove(item);

            }
            dbContext.SaveChanges();

            return true;

        }
        private bool DelDepartment(String id)
        {
            int idD = int.Parse(id);
            var Query = from ds in dbContext.t_departement
                        where ds.C_id == idD
                        select ds;


            foreach (var item in Query)
            {
                dbContext.t_departement.Add(item);
            }
            dbContext.SaveChanges();
            return true;
        }
        private bool DelCasual(String id)
        {
            int idCasual = int.Parse(id);
            var Query = from ds in dbContext.t_vouchers_casuals
                        where ds.C_id_voucher == idCasual
                        select ds;

            foreach (var item in Query)
            {
                dbContext.t_vouchers_casuals.Add(item);
            }

            dbContext.SaveChanges();
            return true;
        }

        private bool DelContractorEmployee(String id)
        {
            int idEmployee = int.Parse(id);
            var Query = from contractor in dbContext.t_contractor
                        join employee in dbContext.employee_contractor on contractor.C_id equals employee.C_idContractor
                        join vouchers in dbContext.t_vouchers_contractor on employee.C_id equals vouchers.C_id_Employed
                        where employee.C_id == idEmployee
                        select new { contractor, employee, vouchers };

            foreach (var item in Query)
            {

                var DelVouchers = from ds in dbContext.t_vouchers_contractor
                                  where ds.C_id_Employed == idEmployee
                                  select ds;

                foreach (var item2 in DelVouchers)
                {
                    dbContext.t_vouchers_contractor.Remove(item2);
                }
                dbContext.SaveChanges();
                dbContext.employee_contractor.Remove(item.employee);
            }
            return true;
        }

        private bool DelContractor(String id)
        {
            int idContractor = int.Parse(id);
            var Query = from ds in dbContext.t_contractor
                        join employee in dbContext.employee_contractor on ds.C_id equals employee.C_idContractor
                        where ds.C_id == idContractor
                        select new { ds, employee };

            foreach (var item in Query)
            {
                this.DelContractorEmployee(item.employee.C_id.ToString());
                dbContext.t_contractor.Remove(item.ds);
            }
            dbContext.SaveChanges();
            return true;
        }
        private bool DelMedicalFacility(String id)
        {
            int idF = int.Parse(id);
            var Query = from facility in dbContext.t_centre_soins
                        where facility.C_id_centre == idF
                        select facility;

            foreach (var item in Query)
            {
                dbContext.t_centre_soins.Remove(item);

            }
            dbContext.SaveChanges();
            return true;
        }
        private bool DelBeneficiairies(String id)
        {
            int idV = int.Parse(id);
            var Query = from ds in dbContext.t_beneficiaires
                        where ds.C_id == idV
                        select ds;

            foreach (var item in Query)
            {
                dbContext.t_beneficiaires.Remove(item);
            }
            dbContext.SaveChanges();
            return true;
        }
        private bool DelVoucher(String id)
        {
            int idVoucher = int.Parse(id);
            var Query = from ds in dbContext.t_bon_commandes
                        where ds.C_id_bon == idVoucher
                        select ds;

            foreach (var item in Query)
            {
                dbContext.t_bon_commandes.Remove(item);

            }
            dbContext.SaveChanges();
            return true;
        }
        private bool DelUserLogin(String id)
        {
            int idUser = int.Parse(id);
            var Query = from ds in dbContext.t_logger
                        where ds.C_id == idUser
                        select ds;

            foreach (var item in Query)
            {
                dbContext.t_logger.Remove(item);

            }
            dbContext.SaveChanges();
            return true;
        }
        

       
        public ActionResult Logs()
        {

            return View();
        }


        public ActionResult viewvisitors()
        {
            return View();
        }
        [HttpPost]
        public ActionResult viewvisitors(Models.AccountStatus accountStatus)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(accountStatus.value))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<String, String>> array = js.Deserialize<List<Dictionary<String, String>>>(accountStatus.value);


                    foreach (var item in array)
                    {
                        int id = int.Parse(item["id"]);
                        string status = item["status"];
                        var Query = from ds in dbContext.t_beneficiaires
                                    where ds.C_id == id
                                    select ds;

                        foreach (var item2 in Query)
                        {
                            if (status.Equals("1"))
                            {
                                item2.C_status_system = "0";
                            }
                            else
                            {
                                item2.C_status_system = "1";
                            }


                        }
                        dbContext.SaveChanges();
                    }
                    if (array.Count > 0)
                    {
                        ViewBag.message = "1";
                    }
                    else
                    {
                        ViewBag.message = "0";
                    }
                }
            }
            return View();
        }
        public ActionResult viewstatusEcontractor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult viewstatusEcontractor(Models.AccountStatus accountStatus)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(accountStatus.value))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<String,String>> array = js.Deserialize<List<Dictionary<String,String>>>(accountStatus.value);
                    

                    foreach (var item in array)
                    {
                        int id = int.Parse(item["id"]);
                        string status = item["status"];
                        var Query = from ds in dbContext.employee_contractor
                                    where ds.C_id == id
                                    select ds;

                        foreach (var item2 in Query)
                        {
                            if (status.Equals("1"))
                            {
                                item2.C_status_system ="0";
                            }
                            else
                            {
                                item2.C_status_system = "1";
                            }
                            

                        }
                        dbContext.SaveChanges();
                    }
                    if (array.Count>0)
                    {
                        ViewBag.message = "1";
                    }
                    else
                    {
                        ViewBag.message = "0";
                    }
                }
            }
            
            return View();
        }
        //public ActionResult viewbeneficiairies()
        //{
        //    return View();
        //}

        public ActionResult viewbeneficiairies(String categ)
        {
            return View();
        }
        [HttpPost]
        public ActionResult viewbeneficiairies(Models.AccountStatus account)
        {
            String sss="";
            if (!String.IsNullOrEmpty(account.value))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
               
                    List<Dictionary<String,String>> array = js.Deserialize<List<Dictionary<String, String>>>(account.value);
                sss = array.Count.ToString();
                    foreach (var item in array)
                    {
                        int id = int.Parse(item["id"]);
                    string QueryId = item["id"];
                        string status = item["status"];
                        var QueryEmployee = from ds in dbContext.t_beneficiaires
                                            where ds.C_id == id
                                            select ds;

                        foreach (var item2 in QueryEmployee)
                        {
                            if (!String.IsNullOrEmpty(item2.C_mat))
                            {
                                if (status.Equals("1"))
                                {
                                    item2.C_status_system = "0";
                                    var Dependent = from dependent in dbContext.t_beneficiaires
                                                    where dependent.C_mat.Equals(null) && (dependent.C_id_parent.Equals(QueryId) || dependent.C_id_partenaire.Equals(QueryId))
                                                    select dependent;

                                    foreach (var item3 in Dependent)
                                    {
                                        item3.C_status_system = "0"; 
                                        ViewBag.message = "1";
                                    }
                                }
                                else
                                {
                                    item2.C_status_system = "1";
                                     ViewBag.message = "1";
                                //var Dependent = from dependent in dbContext.t_beneficiaires
                                //                where dependent.C_mat.Equals(null) && (dependent.C_id_parent.Equals(item.id) || dependent.C_id_partenaire.Equals(item.id))
                                //                select dependent;

                                //foreach (var item3 in Dependent)
                                //{
                                //    item3.C_status_system = "1";
                                //}
                            }
                        }
                        else if (!String.IsNullOrEmpty(item2.C_id_partenaire) && String.IsNullOrEmpty(item2.C_mat))
                        {
                            if (status.Equals("0"))
                            {
                                int idP = int.Parse(item2.C_id_partenaire);
                                var Employee = (from employee in dbContext.t_beneficiaires
                                               where employee.C_id == idP
                                               select employee).FirstOrDefault();
                                if (Employee.C_status_system.Equals(status))
                                {
                                    ViewBag.message = "3";
                                }
                                else
                                {
                                    item2.C_status_system = "1";
                                    ViewBag.message = "1";
                                }

                            }
                            else
                            {
                                item2.C_status_system = "0";
                                ViewBag.message = "1";

                            }
                        }
                        else
                        {
                            if (status.Equals("0"))
                            {
                                int idP = int.Parse(item2.C_id_parent);
                                var Employee = (from employee in dbContext.t_beneficiaires
                                                where employee.C_id == idP
                                                select employee).FirstOrDefault();
                                if (Employee.C_status_system.Equals(status))
                                {
                                    ViewBag.message = "3";
                                }
                                else
                                {
                                    item2.C_status_system = "1";
                                    ViewBag.message = "1";
                                }

                            }
                            else
                            {
                                item2.C_status_system = "0";
                                ViewBag.message = "1";

                            }

                        }
                           
                        }
                        dbContext.SaveChanges();

                    }
                    if (String.IsNullOrEmpty(sss))
                    {
                        ViewBag.message = "0";
                    
                    }
                  
                
              

            }
            return View();
        }
        public String UpdateVisitor()
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var data = "";
            using (System.IO.StreamReader red = new System.IO.StreamReader(Request.InputStream))
            {
                data = red.ReadToEnd();
            }

            Models.SerialVisitor sVisitor = ser.Deserialize<Models.SerialVisitor>(data);
            var Query = from ds in dbContext.t_beneficiaires
                        where ds.C_id == sVisitor.id
                        select ds;


            foreach (var item in Query)
            {
                item.C_name = sVisitor.name;
                item.C_phone = sVisitor.phone;
                item.C_sex = sVisitor.gender;
                item.C_company_visitor = sVisitor.companyvisitor;
                item.C_motif_visit = sVisitor.motif;
            }
            dbContext.SaveChanges();

            return "200";
        }
    }
  
    
    enum typeExportion
    {
        CSV,
        PDF,
        WORD
    }  
}