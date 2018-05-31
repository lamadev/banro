using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanroWebApp.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace BanroWebApp.Controllers
{
    public class StudentDetailController : Controller
    {
        private BANROEntities db = new BANROEntities();

        public ActionResult ExportData()
        {
            GridView gv = new GridView();
           

            gv.DataSource = db.t_succursales.ToList();
            gv.DataBind();
           // Response.ClearContent();
           // Response.Buffer = true;
            //Response.AddHeader("Content-disposition", "attachement;filename=MVI.xls");
            //Response.ContentType = "application/ms-excel";
            //Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            //byte[] bt=new byte[1025666666];
            //Response.OutputStream.Read(bt,0,bt.Length);
            Response.Flush();
            byte[]bt=System.Text.Encoding.UTF8.GetBytes(sw.ToString());
            return File(bt, "application/ms-excel", "MVI.xls");
        }
        // GET: /StudentDetail/
        public ActionResult Index()
        {
            return View(db.t_succursales.ToList());
        }

        // GET: /StudentDetail/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_succursales t_succursales = db.t_succursales.Find(id);
            if (t_succursales == null)
            {
                return HttpNotFound();
            }
            return View(t_succursales);
        }

        // GET: /StudentDetail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /StudentDetail/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="C_id,C_name,C_adresse,C_phone")] t_succursales t_succursales)
        {
            if (ModelState.IsValid)
            {
                db.t_succursales.Add(t_succursales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_succursales);
        }

        // GET: /StudentDetail/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_succursales t_succursales = db.t_succursales.Find(id);
            if (t_succursales == null)
            {
                return HttpNotFound();
            }
            return View(t_succursales);
        }

        // POST: /StudentDetail/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="C_id,C_name,C_adresse,C_phone")] t_succursales t_succursales)
        {
            if (ModelState.IsValid)
            {
              //  db.Entry(t_succursales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_succursales);
        }

        // GET: /StudentDetail/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_succursales t_succursales = db.t_succursales.Find(id);
            if (t_succursales == null)
            {
                return HttpNotFound();
            }
            return View(t_succursales);
        }

        // POST: /StudentDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            t_succursales t_succursales = db.t_succursales.Find(id);
            db.t_succursales.Remove(t_succursales);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
