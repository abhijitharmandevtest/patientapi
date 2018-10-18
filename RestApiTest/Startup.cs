using System.Web.Http;
using Owin;
using RestApi;
using RestApi.Interfaces;
using Unity;

namespace RestApiTest
{
	/// <summary>
	/// Used to configure the testing OWIN server
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Configures the host
		/// </summary>
		public void Configuration(IAppBuilder appBuilder)
		{
			var config = new HttpConfiguration();

			var configUtil = new TestConfigurationUtility(config);
			configUtil.ConfigureRoutes();
			configUtil.ConfigureDependencies();


			appBuilder.UseWebApi(config);
		}
	}
}
