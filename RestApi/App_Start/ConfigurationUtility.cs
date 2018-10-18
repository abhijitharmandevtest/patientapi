using System.Web.Http;
using RestApi.Interfaces;
using RestApi.Models;
using Unity;

namespace RestApi
{
	/// <summary>
	/// Utility to help build the server configuration on startup. To be used before starting the HTTP/OWIN server.
	/// </summary>
	public class ConfigurationUtility
	{
		protected HttpConfiguration Config { get; private set; }

		/// <summary>
		/// Creates a new instance of this type with to configure an HttpConfiguration instance.
		/// </summary>
		/// <param name="config"></param>
		public ConfigurationUtility(HttpConfiguration config)
		{
			this.Config = config;
		}

		/// <summary>
		/// Configures routes for the application
		/// </summary>
		public void ConfigureRoutes()
		{
			Config.Routes.MapHttpRoute(
					name: "Patients and episodes",
					routeTemplate: "patients/{patientId}/episodes",
					defaults: new { controller = "Patients", action = "Get", patientId = RouteParameter.Optional });
		}

		/// <summary>
		/// Configure the dependencies needed for the application
		/// </summary>
		public virtual void ConfigureDependencies()
		{
			var container = new UnityContainer();
			container.RegisterType<IDatabaseContext, PatientContext>();
			Config.DependencyResolver = new UnityResolver(container);
		}
	}
}