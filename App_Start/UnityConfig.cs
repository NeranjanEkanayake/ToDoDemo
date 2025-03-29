using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using ToDoApp.Models;
using ToDoApp.Services;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace ToDoApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            // Register the custom user store
            container.RegisterType<IUserStore<UserModel, int>, CustomUserStore>();

            // Register UserManager with constructor injection
            container.RegisterType<UserManager<UserModel, int>>(
                new InjectionConstructor(typeof(IUserStore<UserModel, int>)));

            // Register your services
            container.RegisterType<UserService>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}