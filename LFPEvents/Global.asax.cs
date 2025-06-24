using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;           // <-- Added for Database.SetInitializer
using LFPEvents.Models;             // Access to LFPDataBContext

namespace LFPEvents
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Disable EF database initialization (prevents recreate error)
            Database.SetInitializer<LFPDataBContext>(null);

            // Call seed method safely
            SeedEventTypes();
        }

        private void SeedEventTypes()
        {
            try
            {
                using (var context = new LFPDataBContext())
                {
                    if (!context.EventTypes.Any())
                    {
                        context.EventTypes.AddRange(new List<EventType>
                        {
                            new EventType { Name = "Wedding" },
                            new EventType { Name = "Concert" },
                            new EventType { Name = "Conference" },
                            new EventType { Name = "Birthday" }
                        });

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or ignore exception as needed
                System.Diagnostics.Debug.WriteLine("[SeedEventTypes] Error: " + ex.Message);
            }
        }
    }
}
