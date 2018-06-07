using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanroWebApp.Models
{
    public class DatePreviousActual
    {
        public string From { set; get; }
    }
    public class ConsultancyCost
    {
        public String Month { set; get; }
        public String Year { set; get; }
        public String Hospital { set; get; }
        public String Consultation { set; get; }
        public String Count { set; get; }
        public String Coast { set; get; }
    }
    public class AccountStatus
    {
        public string value { set; get; }
    }
    public class SerialVisitor
    {
        public int id { set; get; }
        public string name { set; get; }
        public string phone { set; get; }
        public string gender { set; get; }
        public string company { set; get; }
        public string companyvisitor { set; get; }
        public string status { set; get; }
        public string motif { set; get; }
    }
    public class Employed
    {
        public String id { set; get; }
        public String Matricule { set; get; }
        public String name { set; get; }
        public String sex { set; get; }
        public String phone { set; get; }
        public String CivilStatus { set; get; }
        public String picture { set; get; }
        public String datenaiss { set; get; }
        public String ID_Succursale { set; get; }
        public String ID_Departement { set; get; }
        public Partner partner { set; get; }
        public Boolean status { set; get; }
        public String account_system { set; get; }
        public List<Children> Childs { set;get; }
        public int CtrBon = 0;
        public List<BonCommand> dependecies { set; get; }
        public int nbreDepencies = 0;
        public List<BonCommand> Facturations { set; get; }
        public String getImageURL(String source,String pathServer)
        {
            
                var base64 = source;
                String Picture=DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Year+""+DateTime.Now.Hour+""+DateTime.Now.Minute+""+DateTime.Now.Second+DateTime.Now.Millisecond + ".jpg";
                pathServer = pathServer + "/" +DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg";
                base64 = base64.ToString().Substring((base64.ToString().IndexOf(",") + 1));
                byte[] img = Convert.FromBase64String(base64);
                //System.IO.MemoryStream sMemory = new System.IO.MemoryStream(img, 0, img.Length);
                //System.Drawing.Image image = System.Drawing.Image.FromStream(sMemory);
                //image.Save(pathServer);
                System.IO.FileStream fs = new System.IO.FileStream(pathServer, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                fs.Write(img, 0, img.Length);
                fs.Flush();
                fs.Close();
                this.picture = pathServer;
                 return pathServer.Split('/')[1];
           // return (DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg").ToString();
        }
    }

    public class Conjoint
    {
        public String id { set; get; }
        public String name { set; get; }
        public String sex { set; get; }
        public String phone { set; get; }
        public String picture { set; get; }
        public String datenais { set; get; }
        public String conjoint { set; get; }
        public String idSuccursale { set; get; }
        public String idDepartement { set; get; }
        public string account_system { set; get; }
        public Boolean status
        {
            set
            {
                status = false;
            }
            get
            {
                return status;
            }
        }
        public String getImageURL(String source, String pathServer)
        {
            try
            {
                var base64 = source;
                String ImageCreated=DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Year+""+DateTime.Now.Hour+""+DateTime.Now.Minute+""+DateTime.Now.Second+DateTime.Now.Millisecond + ".jpg";
                pathServer = pathServer + "/" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg";
                base64 = base64.ToString().Substring((base64.ToString().IndexOf(",") + 1));
                byte[] img = Convert.FromBase64String(base64);
                System.IO.FileStream fs = new System.IO.FileStream(pathServer, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                fs.Write(img, 0, img.Length);
                fs.Flush();
                fs.Close();
                this.picture = pathServer;
                 return pathServer.Split('/')[1];
                //return  (DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg").ToString();
            }
            catch (Exception)
            {


            }
            return "default.jpg";
            //return (DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg").ToString();
        }
    }

    public class Children
    {
        public String id { set; get; }
        public String name { set; get; }
        public String sex { set; get; }
        public String datenais { set; get; }
        public String picture { set; get; }
        public String parent { set; get; }
        public String status { set; get; }
        public String account_system { set; get; }
        public String getImageURL(String source, String pathServer)
        {
            try
            {
                var base64 = source;
                String ImageCreated=DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Year+""+DateTime.Now.Hour+""+DateTime.Now.Minute+""+DateTime.Now.Second+DateTime.Now.Millisecond + ".jpg";
                pathServer = pathServer + "/" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg";
                base64 = base64.ToString().Substring((base64.ToString().IndexOf(",") + 1));
                byte[] img = Convert.FromBase64String(base64);
                System.IO.FileStream fs = new System.IO.FileStream(pathServer, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                fs.Write(img, 0, img.Length);
                fs.Flush();
                fs.Close();
                this.picture = pathServer;
                 return pathServer.Split('/')[1];
                //return (DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg").ToString();
            }
            catch (Exception)
            {


            }
            return "";
        }
       
    }

    public class Partner : Employed { }
    public class Visitor : Children 
    {
        public String Phone { set; get; }
        public String idVisitor { set; get; }
        public int Uid { set; get; }
        public String ComapnyName { set; get; }
        public String Cause { set; get; }
        public String CompanyVisitor { set; get; }
    }
    public class BonCommand
    {
        public int id { set; get; }
        public string idHealth { set; get; }
        public int idBene { set; get; }
        public String datecmd { set; get; }
        public String dateValidation { set; get; }
        public String nameDoctor { set; get; }
        public String approuve { set; get; }
        public String motif { set; get; }
        public Employed Employed { set; get; }
        public Children Child { set; get; }
        public Partner Partner { set; get; }
        public decimal cout { set; get; }
        public String nameAuthor { set; get; }
        public String sexeAuthor { set; get; }
        public String dateFacture { set; get; }
        public String Consultation { set; get; }
        public String CodeHosto { set; get; }
        public String CompanyName { set; get; }
    }
    public class Facture
    {
        public int id { set; get; }
        public List<BonCommand> BCommands { set; get; }
        public String dateFacture { set; get; }
        public String timeFacture { set; get; }
        public double Cout { set; get; }
        public String FileConverter(String source, String pathServer)
        {
            try
            {
                var base64 = source;
                String ImageCreated=DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Year+""+DateTime.Now.Hour+""+DateTime.Now.Minute+""+DateTime.Now.Second+DateTime.Now.Millisecond + ".jpg";
                pathServer = pathServer + "/" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg";
                base64 = base64.ToString().Substring((base64.ToString().IndexOf(",") + 1));
                byte[] img = Convert.FromBase64String(base64);
                System.IO.FileStream fs = new System.IO.FileStream(pathServer, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                fs.Write(img, 0, img.Length);
                fs.Flush();
                fs.Close();
                //this.DocumentFacture= pathServer;
                 return pathServer.Split('/')[1];
               // return (DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg").ToString();
            }
            catch (Exception)
            {


            }
            return "";
        }
        public String DocumentFacture { set; get; }

    }
    public class RequestEmployed:IDisposable
    {
        private Models.BANROEntities dbContext = null;
        public RequestEmployed()
        {
            dbContext = new BANROEntities();
        }
        public List<Employed> getListEmployed()
        {
            //int idEmployed = 0;

            List<Employed> listEmployed = new List<Employed>();
           // List<Dependecy> listDependecies = new List<Dependecy>();
            var QueryDistinct = from employed in dbContext.t_beneficiaires
                                join Succursal in dbContext.t_succursales on employed.C_id_succ equals Succursal.C_id
                                join Departement in dbContext.t_departement on employed.C_id_depart equals Departement.C_id
                                

                                select new{employed,Succursal,Departement};




            var Query = from employed in dbContext.t_beneficiaires
                        join bonCommand in dbContext.t_bon_commandes on employed.C_id equals bonCommand.C_id_bene
                        join Facturation in dbContext.t_factures on bonCommand.C_id_bon equals Facturation.C_id_bon
                        into Facturation from subfacturation in Facturation.DefaultIfEmpty()
                        join CenterHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals CenterHealth.C_id_centre
                        join Succursal in dbContext.t_succursales on employed.C_id_succ equals Succursal.C_id
                        join Departement in dbContext.t_departement on employed.C_id_depart equals Departement.C_id
                        

                        select new { employed, bonCommand, subfacturation, CenterHealth,Succursal,Departement};

            
            foreach (var query in QueryDistinct)
            {
                List<BonCommand> listDependecies = new List<BonCommand>();
                List<BonCommand> MyFacturations = new List<BonCommand>();
                
              
                int id = query.employed.C_id;
                string _id = id.ToString();
              var result=  Query.Where(x => x.employed.C_id.Equals(id)).FirstOrDefault();
              int nbreBCmd = Query.Where(x => x.employed.C_id.Equals(id)).ToList().Count;

              foreach (var itemMyFact in Query.Where(x => x.employed.C_id.Equals(id)))
              {
                  BonCommand Bcommand = new BonCommand
                  {
                      id=itemMyFact.bonCommand.C_id_bon,
                      datecmd=itemMyFact.bonCommand.C_datedeb,
                      dateValidation=itemMyFact.bonCommand.C_datefin,
                      motif=itemMyFact.bonCommand.C_motif,
                      idBene=itemMyFact.bonCommand.C_id_bene.Value,
                      idHealth=itemMyFact.CenterHealth.C_name,
                      cout = (decimal)(itemMyFact.subfacturation == null ? (0.0M) : itemMyFact.subfacturation.C_cout),
                      nameDoctor=itemMyFact.bonCommand.C_namedoctor,
                    dateFacture=(itemMyFact.subfacturation == null ? String.Empty:itemMyFact.subfacturation.C_datefacture),
                    nameAuthor=itemMyFact.employed.C_name

                  };
                    if (itemMyFact.subfacturation!=null)
                    {
                        MyFacturations.Add(Bcommand);

                    }
                }
              var dependecies = from beneficiaire in dbContext.t_beneficiaires
                                join bonCommand in dbContext.t_bon_commandes on beneficiaire.C_id equals bonCommand.C_id_bene
                                join Facturation in dbContext.t_factures on bonCommand.C_id_bon equals Facturation.C_id_bon
                                into Facturation
                                from subFacturation in Facturation.DefaultIfEmpty()
                                join CenterHealth in dbContext.t_centre_soins on bonCommand.C_id_centre equals CenterHealth.C_id_centre
                                where beneficiaire.C_id_parent.Equals(_id) || beneficiaire.C_id_partenaire.Equals(_id) && beneficiaire.C_id_succ.Equals(null)
                                select new { beneficiaire,subFacturation,CenterHealth,bonCommand };

              int nbreChilds = dependecies.ToList().Count;
             
              foreach (var itemCmd in dependecies)
              {
                  int idBon = itemCmd.bonCommand.C_id_bon;
                  BonCommand bcmd = new BonCommand
                  {
                      id = itemCmd.bonCommand.C_id_bon,
                      datecmd = itemCmd.bonCommand.C_datedeb,
                      dateValidation = itemCmd.bonCommand.C_datefin,
                      nameDoctor = itemCmd.bonCommand.C_namedoctor,
                      motif = itemCmd.bonCommand.C_motif,
                      idBene=itemCmd.bonCommand.C_id_bene.Value,
                      idHealth=itemCmd.CenterHealth.C_name,
                      cout = (decimal)(itemCmd.subFacturation==null?(0.0M):itemCmd.subFacturation.C_cout),
                      nameAuthor=itemCmd.beneficiaire.C_name,
                      sexeAuthor=itemCmd.beneficiaire.C_sex,
                      dateFacture=(itemCmd.subFacturation==null?string.Empty:itemCmd.subFacturation.C_datefacture)
                  };
                  if (itemCmd.subFacturation!=null)
                  {
                      
                  }
                 
                  listDependecies.Add(bcmd);
                 
              }

               // this.AddMyFacturation(query.employed.C_id);
              if (result!=null)
              {
                  Employed employed = new Employed
                {
                    Matricule=query.employed.C_mat,
                    id = query.employed.C_id.ToString(),
                    name = result.employed.C_name,
                    sex = result.employed.C_sex,
                    phone = result.employed.C_phone,
                    datenaiss = result.employed.C_datenais,
                    CivilStatus = result.employed.C_statusmaritalk,
                    picture = result.employed.C_picture,
                    ID_Succursale = result.Succursal.C_name,
                    ID_Departement = result.Departement.C_id_depart,
                    CtrBon=nbreBCmd,
                    nbreDepencies=nbreChilds,
                    dependecies=listDependecies,
                    Facturations=MyFacturations
                };
                
                listEmployed.Add(employed);
              }
              else
              {
                  if (nbreChilds>0)
                  {
                      Employed employed = new Employed
                      {
                          Matricule=query.employed.C_mat,
                          id = query.employed.C_id.ToString(),
                          name = query.employed.C_name,
                          sex = query.employed.C_sex,
                          phone = query.employed.C_phone,
                          datenaiss = query.employed.C_datenais,
                          CivilStatus = query.employed.C_statusmaritalk,
                          picture = query.employed.C_picture,
                          ID_Succursale = query.Succursal.C_name,
                          ID_Departement = query.Departement.C_id_depart,
                          CtrBon = 0,
                          nbreDepencies = nbreChilds,
                           dependecies=listDependecies
                      };

                      listEmployed.Add(employed);    
                  }
              }
               
                

                
            }
           // HttpContext.Current.Session.Add("reporting", listEmployed);
            return listEmployed;
        }




        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    class CenterHelath
    {
         public int C_id_centre { get; set; }
        public string C_name { get; set; }
        public string adresse { get; set; }
        public string C_phone { get; set; }
        public string C_status { get; set; }
    }
    class PrintTableCmd
    {
        public String idBon { set; get; }
        public String datecmd { set; get; }
        public String nameEmployed { set; get; }
        public String sex { set; get; }
        public String phone { set; get; }
        public String company { set; get; }
        public String department { set; get; }
        public String Health { set; get; }
        public String nameAuthor { set; get; }
        public String Category { set; get; }
        public decimal Cost { set; get; }
        

    }
    public class CasualObject
    {
        public int idVoucher { set; get; }
        public String NameCasual { set; get; }
        public String CompanyCasual { set; get; }
        public String DateCasual { set; get; }
        public String Motif { set; get; }
        public String idHospital { set; get; }
        public String idCompanyVisited { set; get; }
        public String Cause { set; get; }
        public Decimal? Cost { set; get; }
    }
    class FactureCmd
    {
        public int id { set; get; }
        public float cout { set; get; }
        public string Categorie { set; get; }
    }
       //var object = {
       //                 idBon: value.id,
       //                 datecmd: value.datecmd,
       //                 nameEmployed: value.Employed.name,
       //                 sex: value.Employed.sex,
       //                 phone: value.Employed.phone,
       //                 company: value.Employed.ID_Succursale,
       //                 department: value.Employed.ID_Departement,
       //                 Health: value.idHealth
       //             };
}