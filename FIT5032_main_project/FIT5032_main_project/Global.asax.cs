using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.SignalR;
using Autofac;
using FIT5032_main_project.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;

namespace FIT5032_main_project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            // Register your SignalR hubs
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // Register your DbContext
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();

            // Build the container
            var container = builder.Build();

            // Set the dependency resolver for SignalR
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);

            // Initialize roles
            InitializeRoles().Wait();
        }

        private async Task InitializeRoles()
        {
            var context = new ApplicationDbContext();
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                context.Roles.Add(new IdentityRole("Admin"));
            }
            if (!context.Roles.Any(r => r.Name == "Doctor"))
            {
                context.Roles.Add(new IdentityRole("Doctor"));
            }
            if (!context.Roles.Any(r => r.Name == "Patient"))
            {
                context.Roles.Add(new IdentityRole("Patient"));
            }
            await context.SaveChangesAsync();
        }
    }
}
