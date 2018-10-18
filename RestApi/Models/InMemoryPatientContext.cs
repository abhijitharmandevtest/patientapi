using System.Collections.Generic;
using System.Data.Entity;
using RestApi.Interfaces;

namespace RestApi.Models
{
	public class InMemoryPatientContext : IDatabaseContext
	{
		private readonly InMemoryDbSet<Patient> _patients;
		private readonly InMemoryDbSet<Episode> _episodes;

		public InMemoryPatientContext(IEnumerable<Patient> patients, IEnumerable<Episode> episodes)
		{
			_patients = new InMemoryDbSet<Patient>(patients);
			_episodes = new InMemoryDbSet<Episode>(episodes);
		}

		public IDbSet<Patient> Patients
		{
			get { return _patients; }
		}

		public IDbSet<Episode> Episodes
		{
			get { return _episodes; }
		}
	}
}