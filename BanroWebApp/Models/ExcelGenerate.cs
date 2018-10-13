using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
//using Microsoft.Office.Interop;
namespace BanroWebApp.Models
{
    public class MonthList
    {
        public string id { set; get; }
        public string name { set; get; }
        public string year { set; get; }
        public string FromMonth { set; get; }
        public string ToMonth { set; get; }
        public string ArrayCost { set; get; }
        public List<MonthList> getMonths()
        {
            List<MonthList> lst = new List<MonthList>()
            {
                new MonthList{
                                id="1",
                                name="January"
                            },
                            new MonthList{
                                id="2",
                                name="February"
                            },
                            new MonthList{
                                id="3",
                                name="March"
                            },
                            new MonthList{
                                id="4",
                                name="April"
                            },new MonthList{
                                id="5",
                                name="May"
                            },new MonthList{
                                id="6",
                                name="June"
                            },new MonthList{
                                id="7",
                                name="July"
                            },new MonthList{
                                id="8",
                                name="August"
                            },new MonthList{
                                id="9",
                                name="September"
                            },new MonthList{
                                id="10",
                                name="October"
                            },new MonthList{
                                id="11",
                                name="November"
                            },new MonthList{
                                id="12",
                                name="December"
                            }
            };
            return lst;
        }
    }
    public class exportDataMCI
    {
        public String category { set; get; }
        public String date1 { set; get; }
        public String date2 { set; get; }
    }
    public class HED
    {
        public string Date { set; get; }
        public string Hospital { set; get; }
        public string Company { set; get; }
        public int Employee { set; get; }
        public int Spouse { set; get; }
        public int Children { set; get; }
        public int Visitor { set; get; }
        public int Total { set; get; }
    }
    public class RMV_Department
    {
        public string from { set; get; }
        public string to{set;get;}
        public string Date { set; get; }
        public string Company { set; get; }
        public string Department { set; get; }
        public string Vouchers { set; get; }
        public string dataList { set; get; }
    }
    public class RMV_General_Report
    {
        public string Date { set; get; }
        public string Company { set; get; }
        public int Vouchers { set; get; }
    }
    public class DateFinding
    {
        public string from { set; get; }
        public string to { set; get; }
    }
    public class MVIObject
    {
        public String idVoucher { set; get; }
        public string Date { set; get; }
        public string DependentID { set; get; }
        public string DependentName { set; get; }
        public string Gender { set; get; }
        public string EmployeeID { set; get; }
        public string EmployeeName { set; get; }
        public string GenderEmployee { set; get; }
        public string Hospital { set; get; }
        public string Consultation { set; get; }
        public string Company { set; get; }
        public string Department { set; get; }
        public decimal Coast { set; get; }
        public String HospitalCode { set; get; }
       
    }
    public class Medical_Voucher_Issued 
    {
        public String Date { set; get; }
        public String Dependent_ID { set; get; }
        public String Dependent_Name { set; get; }
        public String Dependent_Gender { set; get; }
        public String ID_Employee { set; get; }
        public String Employee_Name { set; get; }
        public String Gender { set; get; }
        public String Hospital_Code { set; get; }
        public String Hospital { set; get; }
        public String Consultation { set; get; }
        public String Company { set; get; }
        public String Department { set; get; }
     



    }
    public class Actual_Previous_Month
    {
        public string Company { set; get; }
        public string PreviousMonth { set; get; }
        public string ActualMonth { set; get; }
        public string Hospital { set; get; }
    }
    public class FilterMonthyear
    {
        public string MonthFrom { set; get; }
        public string MonthTo { set; get; }
        public string Year { set; get; }
    }
    public class ConsultancyCoast
    {
        public string Month { set; get; }
        public string Year { set; get; }
        public string Hospital { set; get; }
        public string Consultation { set; get; }
        public int CountConsultation{ set; get; }
        public string Coast { set; get; }
    }
    public class HMVC_Dependent
    {
        public String IdNumber{ set; get; }
        public string Date { set; get; }

        public string DependentName { set; get; }
        public string Hospital { set; get; }
        public string Consultation { set; get; }
        public string Coast { set; get; }
        public string Voucher { set; get; }
    }

    public class HMVC_Employee
    {
        public string Date { set; get; }
        public String NumberID { set; get; }
        public string EmployeeName { set; get; }
        
        public string Hospital { set; get; }
        public string Consultation { set; get; }
        public string Coast { set; get; }
        public string Voucher { set; get; }
    }
    public class Contractor
    {
        public int id { set;get;}
        public string name { set; get; }
        public string adress { get; set; }
        public string phone { set; get; }
        public string idCompany { set; get; }
        public virtual string status { set; get; } 

    }
    public class Employee_Contractor:Contractor
    {
        public string Sexe { set; get; }
        public override string status { set; get; }
        public string datenais { set; get; }
        public string account_status { set; get; }
    }
    public class Cost_department_company
    {
        public string Date { set; get; }
        public string Year { set; get; }
        public string Company { set; get; }
        public string Department { set; get; }
        public string Cost { set; get; }
    }
    public class Cost_Per_Hospital
    {
        public string Date { set; get; }
        public string Year { set; get; }
        public string Hospital { set; get; }
        public string Vouchers { set; get; }
        public string Cost { set; get; }
    }
    class DayOfWeeks
    {
        private int id { set; get; }
        private string name { set; get; }
        public  int Day
        {
            get
            {
                return this.id;
            }
        }
        public DayOfWeeks() { }
        public DayOfWeeks(DayOfWeek dayOfWeek)
        {
            this.id=this.getDays().Where(e => e.name.Equals(dayOfWeek.ToString())).FirstOrDefault().id;
        }
        private List<DayOfWeeks> getDays()
        {
            List<DayOfWeeks> lst = new List<DayOfWeeks>
            {
                new DayOfWeeks{
                        id=1,
                        name="Monday"
                    },
                    new DayOfWeeks{
                        id=2,
                        name="Tuesday"
                    },
                    new DayOfWeeks{
                        id=3,
                        name="Wednesday"
                    },
                    new DayOfWeeks{
                        id=4,
                        name="Thursday"
                    },
                    new DayOfWeeks{
                        id=5,
                        name="Friday"
                    },
                    new DayOfWeeks{
                        id=6,
                        name="Saturday"
                    },
                    new DayOfWeeks{
                        id=7,
                        name="Sunday"
                    }
            };
            return lst;
        }
    }
    public class ExcelGenerate
    {

        public static string company { set; get; }
        public static string department { set; get; }
        public List<RMV_General_Report> __lstRMVGR = new List<RMV_General_Report>();
        Models.RequestEmployed ReqEmployed = new RequestEmployed();
        List<Employed> __getListEmployed = new List<Employed>();
        public List<Employed> __getListEmployedSelected = new List<Employed>();
        List<BonCommand> __getListDepencies = new List<BonCommand>();
        public List<RMV_Department> lstRMV = new List<RMV_Department>();
        DateTime _from;
        DateTime _to;
        public List<MVIObject> __listMVC = new List<MVIObject>();
       
        StringBuilder __strBuilder = new StringBuilder();
        public ExcelGenerate(Categorie categorie,String scope="",String from="",String to="")
        {
            switch (categorie)
            {
                case Categorie.MEDICAL_VOUCHERS_ISSUED:
                    if (scope.Equals("Monthly"))
                    {
                        __medicalVouchersIssued();                        
                    }
                    if (scope.Equals("Daily"))
                    {
                        CultureInfo cInfo = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = cInfo;
                        String year = DateTime.Now.Year.ToString();
                        String months = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
                        String day = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
                        String dateNow1 = months + "/" + "/" + day + "/" + year;
                        __medicalVouchersIssued(DateTime.Now.ToShortDateString());
                    }
                    if (scope.Equals("Choose"))
                    {
                        CultureInfo cInfo = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = cInfo;
                        this._from = DateTime.Parse(from);
                         this._to = DateTime.Parse(to);
                        __medicalVouchersIssued(_from, _to);
                    }
                    if (scope.Equals("Weekly"))
                    {
                        CultureInfo cInfo = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = cInfo;
              
                        this.__medicalVouchersIssued(DateTime.Now.DayOfWeek);
                    }
                    
                    break;
                case Categorie.REPORT_OF_MEDICAL_VOUCHERS:
                    if (scope.Equals("GR"))
                    {
                        CultureInfo cInfo = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = cInfo;
                        this._from = Convert.ToDateTime(from);
                            this._to = Convert.ToDateTime(to);
                            this.__RMV_General_Report(_from, _to);
                    }
                    if (scope.Equals("RMVDep"))
                    {
                        CultureInfo cInfo = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = cInfo;
                        this._from = Convert.ToDateTime(from);
                            this._to = Convert.ToDateTime(to);
                            this.__RMV_for_Department(this._from, this._to, ExcelGenerate.company, ExcelGenerate.department);
                    }
                    break;
                case Categorie.FOR_EACH_HOSPITAL_EMPLOYEES_VS_DEPENDENTS:
                    break;
                case Categorie.FOR_DEPARTMENTS:
                    break;
                case Categorie.ACTUALS_VS_PREVIOUS_MONTH:
                    break;
                case Categorie.CONSULTATION_AND_COST:
                    break;
                case Categorie.HMVC_EMPLOYEE:
                    break;
                case Categorie.HMVC_DEPENDENTS:
                    break;
                case Categorie.HMVC_COST_DEPARTMENT_COMPANY:
                    break;
                case Categorie.HMVC_COST_HOSPITAL:
                    break;
                default:
                    break;
            }
        }
        Models.BANROEntities dbContext = new BANROEntities();
        private void __medicalVouchersIssued()
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            String CurrentMonth = DateTime.Now.Month.ToString();
            //if (CurrentMonth.Length==1)
            //{
            //    CurrentMonth = "0" + CurrentMonth;
            //}

            var req = from ds in dbContext.t_beneficiaires
                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                      join health in dbContext.t_centre_soins on voucher.C_id_centre equals health.C_id_centre
                      orderby voucher.C_id_bon descending
                      select new { benef = ds, vouchers = voucher,facilityMedical=health };


            foreach (var item in req)
            {
                if (!String.IsNullOrEmpty(item.benef.C_id_parent) )
                {
                    int idChild = int.Parse(item.benef.C_id_parent);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idChild
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart =(int) em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName= dbContext.t_departement.FirstOrDefault(s => s.C_id==idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                    if (month.Equals(CurrentMonth))
                    {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = testValue,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = item.benef.C_name,
                            sexeAuthor = item.benef.C_sex,
                            motif = item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = em.C_mat,
                                name = em.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };

                        this.__getListDepencies.Add(bcmd);
                    }
                    

                }
                else if (!String.IsNullOrEmpty(item.benef.C_id_partenaire) && String.IsNullOrEmpty(item.benef.C_mat))
                {
                    int idPart = int.Parse(item.benef.C_id_partenaire);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idPart
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                    if (month.Equals(CurrentMonth))
                    {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = testValue,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = item.benef.C_name,
                            sexeAuthor = item.benef.C_sex,
                            motif = item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = em.C_mat,
                                name = em.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };

                        this.__getListDepencies.Add(bcmd);

                    }
                }
               
                else if(!String.IsNullOrEmpty(item.benef.C_mat))
                {
           

                    String idSucc = item.benef.C_id_succ;
                    int idDepart =(int)item.benef.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                    if (month.Equals(CurrentMonth))
                    {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = item.vouchers.C_motif,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = "-",
                            sexeAuthor = "-",
                            motif=item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = item.benef.C_mat,
                                name = item.benef.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };
                        this.__getListDepencies.Add(bcmd);

                    }
                }
            }

        

            this.__DrawingExcelMVI("Monthly");
        }
        String testValue = "";
        private void __medicalVouchersIssued(DayOfWeek dayofWeek)
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            double c = 0;
            Stack<String> StaList = new Stack<string>();
            while (c<=7)
            {
                double d = Double.Parse(String.Format("{0}{1}", "-", c));
                StaList.Push(dt.AddDays(d).ToShortDateString());
                c++;
            }
               
                this.__getListDepencies.Clear();
            int ctr = 0;
            


            var req = from ds in dbContext.t_beneficiaires
                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                      join health in dbContext.t_centre_soins on voucher.C_id_centre equals health.C_id_centre
                      where ds.C_id_visitor.Equals(null)
                      select new { benef = ds, vouchers = voucher, facilityMedical = health };


            foreach (var item in req)
            {
                if (!String.IsNullOrEmpty(item.benef.C_id_parent))
                {
                    testValue = "LAMA";
                    int idChild = int.Parse(item.benef.C_id_parent);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idChild
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                    foreach (var itemStack in StaList)
                    {
                        if (item.vouchers.C_datedeb.Equals(itemStack))
                        {
                            BonCommand bcmd = new BonCommand()
                            {
                                id = item.vouchers.C_id_bon,
                                datecmd = item.vouchers.C_datedeb,
                                Consultation = testValue,
                                idHealth = item.facilityMedical.C_name,
                                CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                                idBene = (int)item.vouchers.C_id_bene,
                                nameAuthor = item.benef.C_name,
                                sexeAuthor = item.benef.C_sex,
                                motif = item.vouchers.C_motif,
                                Employed = new Employed
                                {
                                    id = item.benef.C_id.ToString(),
                                    Matricule = em.C_mat,
                                    name = em.C_name,
                                    ID_Departement = DepartName,
                                    ID_Succursale = CompanyName,
                                    sex = item.benef.C_sex
                                }

                            };
                            this.__getListDepencies.Add(bcmd);
                            ctr++;
                        }
                    }


                }
                if (!String.IsNullOrEmpty(item.benef.C_id_partenaire) && String.IsNullOrEmpty(item.benef.C_mat))
                {
                    int idPart = int.Parse(item.benef.C_id_partenaire);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idPart
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                    foreach (var itemStack in StaList)
                    {
                        if (item.vouchers.C_datedeb.Equals(itemStack))
                        {
                            BonCommand bcmd = new BonCommand()
                            {
                                id = item.vouchers.C_id_bon,
                                datecmd = item.vouchers.C_datedeb,
                                Consultation = testValue,
                                idHealth = item.facilityMedical.C_name,
                                CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                                idBene = (int)item.vouchers.C_id_bene,
                                nameAuthor = item.benef.C_name,
                                sexeAuthor = item.benef.C_sex,
                                motif = item.vouchers.C_motif,
                                Employed = new Employed
                                {
                                    id = item.benef.C_id.ToString(),
                                    Matricule = em.C_mat,
                                    name = em.C_name,
                                    ID_Departement = DepartName,
                                    ID_Succursale = CompanyName,
                                    sex = item.benef.C_sex
                                }

                            };
                            this.__getListDepencies.Add(bcmd);
                            ctr++;
                        }
                    }
                }

                else if (!String.IsNullOrEmpty(item.benef.C_mat))
                {


                    String idSucc = item.benef.C_id_succ;
                    int idDepart = (int)item.benef.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                    foreach (var itemStack in StaList)
                    {
                        if (item.vouchers.C_datedeb.Equals(itemStack))
                        {
                            BonCommand bcmd = new BonCommand()
                            {
                                id = item.vouchers.C_id_bon,
                                datecmd = item.vouchers.C_datedeb,
                                Consultation = item.vouchers.C_motif,
                                idHealth = item.facilityMedical.C_name,
                                CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                                idBene = (int)item.vouchers.C_id_bene,
                                nameAuthor = "-",
                                sexeAuthor = "-",
                                motif = item.vouchers.C_motif,
                                Employed = new Employed
                                {
                                    id = item.benef.C_id.ToString(),
                                    Matricule = item.benef.C_mat,
                                    name = item.benef.C_name,
                                    ID_Departement = DepartName,
                                    ID_Succursale = CompanyName,
                                    sex = item.benef.C_sex
                                }

                            };
                            this.__getListDepencies.Add(bcmd);
                            ctr++;
                        }
                    }
                   
                }
            }

            

                this.__DrawingExcelMVI();
           
        }
        private void __medicalVouchersIssued(String dateNow)
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            this.__getListDepencies.Clear();
            int ctr = 0;
            


            var req = from ds in dbContext.t_beneficiaires
                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                      join health in dbContext.t_centre_soins on voucher.C_id_centre equals health.C_id_centre
                      select new { benef = ds, vouchers = voucher, facilityMedical = health };


            foreach (var item in req)
            {
                if (!String.IsNullOrEmpty(item.benef.C_id_parent))
                {
                    int idChild = int.Parse(item.benef.C_id_parent);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idChild
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
        
                   
                        if (item.vouchers.C_datedeb.Equals(dateNow))
                        {
                            BonCommand bcmd = new BonCommand()
                            {
                                id = item.vouchers.C_id_bon,
                                datecmd = item.vouchers.C_datedeb,
                                Consultation = item.vouchers.C_motif,
                                idHealth = item.facilityMedical.C_name,
                                CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                                idBene = (int)item.vouchers.C_id_bene,
                                nameAuthor = item.benef.C_name,
                                sexeAuthor = item.benef.C_sex,
                                motif = item.vouchers.C_motif,
                                Employed = new Employed
                                {
                                    id = em.C_id.ToString(),
                                    Matricule = em.C_mat,
                                    name = em.C_name,
                                    ID_Departement = DepartName,
                                    ID_Succursale = CompanyName,
                                    sex = item.benef.C_sex
                                }

                            };
                            this.__getListDepencies.Add(bcmd);
                            ctr++;
                        }
                  


                }
                else if (!String.IsNullOrEmpty(item.benef.C_id_partenaire) && String.IsNullOrEmpty(item.benef.C_mat))
                {
                    int idPart = int.Parse(item.benef.C_id_partenaire);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idPart
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                  
                   
                        if (item.vouchers.C_datedeb.Equals(dateNow))
                        {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = testValue,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = item.benef.C_name,
                            sexeAuthor = item.benef.C_sex,
                            motif = item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = em.C_mat,
                                name = em.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };
                        this.__getListDepencies.Add(bcmd);
                            ctr++;
                        }
                    
                }

                else if (!String.IsNullOrEmpty(item.benef.C_mat))
                {


                    String idSucc = item.benef.C_id_succ;
                    int idDepart = (int)item.benef.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[0]).ToString();
                   
                        if (item.vouchers.C_datedeb.Equals(dateNow))
                        {
                            BonCommand bcmd = new BonCommand()
                            {
                                id = item.vouchers.C_id_bon,
                                datecmd = item.vouchers.C_datedeb,
                                Consultation = item.vouchers.C_motif,
                                idHealth = item.facilityMedical.C_name,
                                CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                                idBene = (int)item.vouchers.C_id_bene,
                                nameAuthor = "-",
                                sexeAuthor = "-",
                                motif = item.vouchers.C_motif,
                                Employed = new Employed
                                {
                                    id = item.benef.C_id.ToString(),
                                    Matricule = item.benef.C_mat,
                                    name = item.benef.C_name,
                                    ID_Departement = DepartName,
                                    ID_Succursale = CompanyName,
                                    sex = item.benef.C_sex
                                }

                            };
                            this.__getListDepencies.Add(bcmd);
                            ctr++;
                        }
                    

                }
            }

            this.__DrawingExcelMVI();
        }
        private void __medicalVouchersIssued(DateTime from,DateTime to)
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;
            this.__getListDepencies.Clear();
            int ctr = 0;
          


            var req = from ds in dbContext.t_beneficiaires
                      join voucher in dbContext.t_bon_commandes on ds.C_id equals voucher.C_id_bene
                      join health in dbContext.t_centre_soins on voucher.C_id_centre equals health.C_id_centre
                      select new { benef = ds, vouchers = voucher, facilityMedical = health };


            foreach (var item in req)
            {
               
                DateTime dt = DateTime.Parse(item.vouchers.C_datedeb.Trim());
                if (!String.IsNullOrEmpty(item.benef.C_id_parent))
                {
                    int idChild = int.Parse(item.benef.C_id_parent);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idChild
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[1]).ToString();
                    
                    if (dt >= from && dt <= to)
                    {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = item.vouchers.C_motif,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = item.benef.C_name,
                            sexeAuthor = item.benef.C_sex,
                            motif = item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = em.C_mat,
                                name = em.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };
                        this.__getListDepencies.Add(bcmd);
                        ctr++;
                    }



                }
                else if (!String.IsNullOrEmpty(item.benef.C_id_partenaire) && String.IsNullOrEmpty(item.benef.C_mat))
                {
                    int idPart = int.Parse(item.benef.C_id_partenaire);
                    var em = (from ds in dbContext.t_beneficiaires
                              where ds.C_id == idPart
                              select ds).FirstOrDefault();

                    String idSucc = em.C_id_succ;
                    int idDepart = (int)em.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    String month = int.Parse(findMonth[1]).ToString();

                    if (dt >= from && dt <= to)
                    {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = testValue,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = item.benef.C_name,
                            sexeAuthor = item.benef.C_sex,
                            motif = item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = em.C_mat,
                                name = em.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };
                        this.__getListDepencies.Add(bcmd);
                        ctr++;
                    }

                }

                else if(String.IsNullOrEmpty(item.benef.C_id_visitor))
                {


                    String idSucc = item.benef.C_id_succ;
                    int idDepart = (int)item.benef.C_id_depart;
                    String CompanyName = dbContext.t_succursales.FirstOrDefault(s => s.C_id.Equals(idSucc)).C_name;
                    String DepartName = dbContext.t_departement.FirstOrDefault(s => s.C_id == idDepart).C_id_depart;
                    String[] findMonth = item.vouchers.C_datedeb.Split('/');
                    //    String month = int.Parse(findMonth[0]).ToString();

                    if (dt >= from && dt <= to)
                    {
                        BonCommand bcmd = new BonCommand()
                        {
                            id = item.vouchers.C_id_bon,
                            datecmd = item.vouchers.C_datedeb,
                            Consultation = item.vouchers.C_motif,
                            idHealth = item.facilityMedical.C_name,
                            CodeHosto = item.facilityMedical.C_id_centre.ToString(),
                            idBene = (int)item.vouchers.C_id_bene,
                            nameAuthor = "-",
                            sexeAuthor = "-",
                            motif = item.vouchers.C_motif,
                            Employed = new Employed
                            {
                                id = item.benef.C_id.ToString(),
                                Matricule = item.benef.C_mat,
                                name = item.benef.C_name,
                                ID_Departement = DepartName,
                                ID_Succursale = CompanyName,
                                sex = item.benef.C_sex
                            }

                        };
                        this.__getListDepencies.Add(bcmd);
                        ctr++;
                    }


                }
            }
           

          
            this.__DrawingExcelMVI("Choose");
            
        }
        private void __DrawingExcelMVI(String categ="")
        {
           
            this.__listMVC.Clear();
            if (categ.Equals("Monthly"))
            {
                if (this.__getListDepencies.Count > 0)
                {

                    foreach (var item in this.__getListDepencies)
                    {
                        MVIObject mvi = new MVIObject
                        {
                            EmployeeID = item.Employed.Matricule,
                            EmployeeName = item.Employed.name,
                            GenderEmployee = item.Employed.sex,
                            Company = item.Employed.ID_Succursale,
                            Department = item.Employed.ID_Departement

                        };
                        mvi.idVoucher = item.id.ToString();
                        mvi.Date = item.datecmd;
                        mvi.DependentID =(item.nameAuthor.Equals("-")?"-": mvi.EmployeeID + "-" + item.idBene.ToString());
                        mvi.DependentName = item.nameAuthor;
                        mvi.Gender = item.sexeAuthor;
                        mvi.Consultation = item.motif;
                        mvi.Hospital = item.idHealth;
                        mvi.Coast = item.cout;
                        mvi.HospitalCode = item.CodeHosto;
                        __listMVC.Add(mvi);
                        mvi = null;

                    }

                }
            }
            else if (categ.Equals("Choose"))
            {
                if (this.__getListDepencies.Count > 0)
                {

                    foreach (var item in this.__getListDepencies)
                    {
                        MVIObject mvi = new MVIObject
                        {
                            EmployeeID = item.Employed.Matricule,
                            EmployeeName = item.Employed.name,
                            GenderEmployee = item.Employed.sex,
                            Company = item.Employed.ID_Succursale,
                            Department = item.Employed.ID_Departement

                        };
                        mvi.idVoucher = item.id.ToString();
                        mvi.Date = item.datecmd;
                        mvi.DependentID = (item.nameAuthor.Equals("-") ? "-" : mvi.EmployeeID + "-" + item.idBene.ToString());
                        mvi.DependentName = item.nameAuthor;
                        mvi.Gender = item.sexeAuthor;
                        mvi.Consultation = item.motif;
                        mvi.Hospital = item.idHealth;
                        mvi.Coast = item.cout;
                        mvi.HospitalCode = item.CodeHosto;
                        __listMVC.Add(mvi);
                        mvi = null;

                    }

                }
            }
            else
            {
                if (this.__getListDepencies.Count > 0)
                {

                    foreach (var item in this.__getListDepencies)
                    {
                        MVIObject mvi = new MVIObject
                        {
                            EmployeeID = item.Employed.Matricule,
                            EmployeeName = item.Employed.name,
                            GenderEmployee = item.Employed.sex,
                            Company = item.Employed.ID_Succursale,
                            Department = item.Employed.ID_Departement

                        };
                        mvi.idVoucher = item.id.ToString();
                        mvi.Date = item.datecmd;
                        mvi.DependentID = (item.nameAuthor.Equals("-") ? "-" : mvi.EmployeeID + "-" + item.idBene.ToString());
                        mvi.DependentName = item.nameAuthor;
                        mvi.Gender = item.sexeAuthor;
                        mvi.Consultation = item.motif;
                        mvi.Hospital = item.idHealth;
                        mvi.Coast = item.cout;
                        mvi.HospitalCode = item.CodeHosto;
                        __listMVC.Add(mvi);
                        //mvi = null;

                    }

                }
            }
            

           // sBuilder.AppendLine("Date Dependent-ID Dependent-Name Gender ID-Employee Employee-Name Gender Department Hospital-Code Hospital Consultation Company");
            
          //  this.__strBuilder = sBuilder;
        }

        private void __RMV_General_Report(DateTime from, DateTime to)
        {

            String dte = from.Month.ToString() + "/" + (from.Day.ToString().Length == 1 ? "0" + from.Day.ToString() : from.Day.ToString()) + "/" + from.Year.ToString();

            
            var listCompanies = from ds in dbContext.t_succursales
                                select ds;
            foreach (var item in listCompanies)
            {
                string idCurrent = item.C_id;
                var req = from benef in dbContext.t_beneficiaires
                          join voucher in dbContext.t_bon_commandes on benef.C_id equals voucher.C_id_bene
                          where benef.C_id_succ.Equals(idCurrent) && voucher.C_datedeb.Equals(dte)
                          select new { beneficiairies = benef,vouchers=voucher } ;

                RMV_General_Report RMVGR = new RMV_General_Report();
                RMVGR.Date = dte;
                RMVGR.Company = item.C_name;
                RMVGR.Vouchers = req.ToList().Count;
                __lstRMVGR.Add(RMVGR);

            }

                var reqDep = from benef in dbContext.t_beneficiaires
                          join voucher in dbContext.t_bon_commandes on benef.C_id equals voucher.C_id_bene
                          where benef.C_id_succ.Equals(null) && voucher.C_datedeb.Equals(dte)
                          select new { beneficiairies = benef, vouchers = voucher };

         
                foreach (var itemGR in reqDep)
                {
                    if (!String.IsNullOrEmpty(itemGR.beneficiairies.C_id_parent))
                    {
                        int idParent = int.Parse(itemGR.beneficiairies.C_id_parent);
                    var req2 = (from ds in dbContext.t_beneficiaires
                                join company in dbContext.t_succursales
                                on ds.C_id_succ equals company.C_id
                                where ds.C_id == idParent
                                select new { ds,company }).FirstOrDefault();

                        foreach (var itemCompany in listCompanies)
                        {
                            if (itemCompany.C_name.Equals(req2.company.C_name))
                            {
                               var Company = __lstRMVGR.Where(t=> t.Company.Equals(itemCompany.C_name)).FirstOrDefault();
                                 if (Company!=null)
                                 {
                                    for (int i = 0; i < __lstRMVGR.Count; i++)
                                    {
                                        if (__lstRMVGR[i].Company.Equals(itemCompany.C_name))
                                        {
                                            __lstRMVGR[i].Vouchers += 1;

                                        }
                                    }
                                }
                                else
                                {
                                RMV_General_Report RMVGR = new RMV_General_Report();
                                RMVGR.Date = dte;
                                RMVGR.Company = itemCompany.C_name;
                                RMVGR.Vouchers +=1;
                                __lstRMVGR.Add(RMVGR);
                                }
                               
                            
                                
                            }


                        }

                }
                else if(!String.IsNullOrEmpty(itemGR.beneficiairies.C_id_partenaire))
                {
                    int idPartenaire = int.Parse(itemGR.beneficiairies.C_id_partenaire);
                    var req2 = (from ds in dbContext.t_beneficiaires
                                join company in dbContext.t_succursales
                                on ds.C_id_succ equals company.C_id
                                where ds.C_id == idPartenaire
                                select new { ds, company }).FirstOrDefault();

                    foreach (var itemCompany in listCompanies)
                    {
                        if (itemCompany.C_name.Equals(req2.company.C_name))
                        {
                            var Company = __lstRMVGR.Where(t => t.Company.Equals(itemCompany.C_name)).FirstOrDefault();
                            if (Company != null)
                            {
                                for (int i = 0; i < __lstRMVGR.Count; i++)
                                {
                                    if (__lstRMVGR[i].Company.Equals(itemCompany.C_name))
                                    {
                                        __lstRMVGR[i].Vouchers += 1;

                                    }
                                }
                            }
                            else
                            {
                                RMV_General_Report RMVGR = new RMV_General_Report();
                                RMVGR.Date = dte;
                                RMVGR.Company = itemCompany.C_name;
                                RMVGR.Vouchers += 1;
                                __lstRMVGR.Add(RMVGR);
                            }



                        }


                    }
                }

                }

               

            


        }

        List<Employed> lstHED = new List<Employed>();
        private void __RMV_for_Department(DateTime from, DateTime to,String company,String department)
        {
            CultureInfo cInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cInfo;

            List<Employed>LstQueryFull=ReqEmployed.getListEmployed();
            var QueryDate = from datas in LstQueryFull
                            where datas.ID_Succursale.Equals(company) && datas.ID_Departement.Equals(department)
                            select datas;

            foreach (var item in QueryDate)
            {
                RMV_Department depRVM = new RMV_Department()
                {
                     Company=item.ID_Succursale,
                     Department=item.ID_Departement,
                     Vouchers=(item.Facturations==null?0:item.Facturations.Count).ToString()
                };
                lstRMV.Add(depRVM);
            }
            //foreach (var itemDate in QueryDate)
            //{
            //    if (itemDate.Facturations!=null)
            //    {
            //        RMV_Department rmvdep = new RMV_Department();
            //        foreach (var item in itemDate.Facturations)
            //        {
            //            if (DateTime.Parse(item.datecmd)>=from && DateTime.Parse(item.datecmd)<=to)
            //            {
            //                RMV_Department rmv = new RMV_Department
            //                {
            //                    Date=item.datecmd,
            //                    Company=company,
            //                    Department=department
            //                };
            //                lstRMV.Add(rmv);
            //            }
            //        }

            //    }   
            //}
        }
        
    }
   public enum Categorie
    {
        MEDICAL_VOUCHERS_ISSUED,
        REPORT_OF_MEDICAL_VOUCHERS,
        FOR_EACH_HOSPITAL_EMPLOYEES_VS_DEPENDENTS,
        FOR_DEPARTMENTS,
        ACTUALS_VS_PREVIOUS_MONTH,
        CONSULTATION_AND_COST,
        HMVC_EMPLOYEE,
        HMVC_DEPENDENTS,
        HMVC_COST_DEPARTMENT_COMPANY,
        HMVC_COST_HOSPITAL
    }
}