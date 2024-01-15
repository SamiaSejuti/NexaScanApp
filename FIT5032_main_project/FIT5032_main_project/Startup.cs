using Microsoft.Owin;
using Owin;
using Autofac.Integration.SignalR;
using Autofac;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartupAttribute(typeof(FIT5032_main_project.Startup))]
namespace FIT5032_main_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Auth and SignalR
            ConfigureAuth(app);
            var builder = new ContainerBuilder();

            // Example: builder.RegisterType<YourType>().As<IYourInterface>();

            var container = builder.Build();

            // Set Autofac as the dependency resolver for SignalR
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);

            // Configure Auth
            ConfigureAuth(app);

            // SignalR
            app.MapSignalR();
        }
    }
}
