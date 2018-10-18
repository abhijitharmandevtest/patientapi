using System.Linq;
using System.Net;
using System.Web.Http;
using RestApi.Interfaces;
using RestApi.Models;

namespace RestApi.Controllers
{
	public class PatientsController : ApiController
	{
		private readonly IDatabaseContext databaseContext;

		public PatientsController(IDatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		[HttpGet]
		public Patient Get(int patientId)
		{
			//pending for a full fledged production ready application..
			//todo: validtion checks, update model layer with data annotations
			//todo: add a "service" layer
			//todo: make the service layer use a unit of work pattern
			//todo: add other methods for CRUD
			//todo: add methods for search/filters, pagination, ordering. OData can help here
			//todo: add caching for the DBContext layer

			var patientsAndEpisodes =
				from p in databaseContext.Patients
				join e in databaseContext.Episodes on p.PatientId equals e.PatientId
				where p.PatientId == patientId
				select new {p, e};

			if (patientsAndEpisodes.Any())
			{
				var first = patientsAndEpisodes.First().p;
				first.Episodes = patientsAndEpisodes.Select(x => x.e).ToArray();
				return first;
			}

			throw new HttpResponseException(HttpStatusCode.NotFound);
		}
	}
}