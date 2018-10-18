using System.Web.Http;
using RestApi;
using RestApi.Interfaces;
using Unity;

namespace RestApiTest
{
	public class TestConfigurationUtility : ConfigurationUtility
	{
		public TestConfigurationUtility(HttpConfiguration config) : base(config)
		{ }

		public override void ConfigureDependencies()
		{
			var container = new UnityContainer();
			container.RegisterInstance<IDatabaseContext>(TestDbContextBuilder.Construct());
			Config.DependencyResolver = new UnityResolver(container);
		}
	}
}
