using System.Web.Optimization;

public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        // ✅ jQuery - avoid ScriptBundle to skip WebGrease minification crash
        bundles.Add(new Bundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-3.7.0.min.js" // use the minified version directly
        ));

        // ✅ Bootstrap JS - same, use minified version directly
        bundles.Add(new Bundle("~/bundles/bootstrap").Include(
            "~/Scripts/bootstrap.bundle.min.js"
        ));

        // ✅ Bootstrap CSS bundle
        bundles.Add(new StyleBundle("~/Content/css").Include(
            "~/Content/bootstrap.css",
            "~/Content/site.css"
        ));

        // ✅ Modernizr (no change needed if not crashing)
        bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            "~/Scripts/modernizr-*"
        ));

        // ✅ Optional: turn off optimizations while debugging
        BundleTable.EnableOptimizations = false;
    }
}
