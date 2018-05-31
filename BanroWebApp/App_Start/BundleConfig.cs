using System.Web;
using System.Web.Optimization;

namespace BanroWebApp
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le regroupement, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap.min.js",
                        "~/Scripts/dataTables.responsive.js"
                        
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l’outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/metisMenu.min.js",
                      "~/Scripts/sb-admin-2.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/Mypicker.js"
                    
                    
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/Site.css",
                      "~/Content/metisMenu.min.css",
                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/sb-admin-2.css",
                      "~/Content/morris.css",
                      "~/Content/timeline.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/dataTables.responsive.css",
                      "~/Content/bootstrap-datepicker.standalone.min.css"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.min.js",
                      "~/Scripts/angular-route.min.js",
                      "~/Scripts/ServicesModels.js",
                      "~/Scripts/Controllers.js",
                      "~/Scripts/Routers.js",
                      "~/Scripts/angular-datatables.js"

                ));
        }
    }
}
