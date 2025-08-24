using System.Web.Optimization;

namespace WebArribosPlanta
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/responsive.dataTables.css"));

            bundles.Add(new ScriptBundle("~/bundles/complementos").Include(
                     "~/Scripts/DataTables/jquery.dataTables.js",
                     "~/Scripts/DataTables/dataTables.responsive.js"));
        }
    }
}
