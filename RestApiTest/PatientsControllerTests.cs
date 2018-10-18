using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using RestApi.Models;

namespace RestApiTest
{
	[TestFixture]
	public class PatientsControllerTests
	{
		/// <summary>
		/// A test to check if the API can retrieve existing patients using the correct route
		/// </summary>
		[Test]
		public async Task TestGetForExistingPatient()
		{
			using (var testServer = TestServer.Create<RestApiTest.Startup>())
			{
				var result = await testServer.HttpClient.GetAsync(@"/patients/1/episodes");
				Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
				var responseContent = result.Content.ReadAsStringAsync();
				var patient = JsonConvert.DeserializeObject<Patient>(responseContent.Result);

				//Verify the patient object
				Assert.IsInstanceOf<Patient>(patient, "The result was not a Patient type");
				Assert.AreEqual(1, patient.PatientId, "patient ID does't match");
				Assert.AreEqual("1111111111", patient.NhsNumber, "NHS number");
				Assert.AreEqual("Millicent", patient.FirstName);
				Assert.AreEqual("Hammond", patient.LastName);
				Assert.AreEqual(new DateTime(1972, 10, 27), patient.DateOfBirth);

				//Verify the episode collection
				Assert.IsNotNull(patient.Episodes, "No related episodes found");
				Assert.IsTrue(patient.Episodes.Any(), "Episodes collection is empty");
				Assert.AreEqual(3, patient.Episodes.Count(), "Episode count is incorrect");

				//Verify the episode for the patient
				var episode = patient.Episodes.ElementAt(0);
				Assert.IsInstanceOf<Episode>(episode, "The first episode is null");

				Assert.AreEqual("Irritation of inner ear", episode.Diagnosis, "Diagnosis is incorrect");
				Assert.AreEqual(1, episode.PatientId, "Patient ID doesn't match");
				Assert.AreEqual(1, episode.EpisodeId, "Episode ID is incorrect");
				Assert.AreEqual(new DateTime(2014, 11, 27), episode.DischargeDate, "DischargeDate is incorrect");
				Assert.AreEqual(new DateTime(2014, 11, 12), episode.AdmissionDate, "AdmissionDate is incorrect");

				episode = patient.Episodes.ElementAt(1);
				Assert.IsInstanceOf<Episode>(episode, "The second episode is null");

				Assert.AreEqual("Sprained wrist", episode.Diagnosis, "Diagnosis is incorrect");
				Assert.AreEqual(1, episode.PatientId, "Patient ID doesn't match");
				Assert.AreEqual(2, episode.EpisodeId, "Episode ID is incorrect");
				Assert.AreEqual(new DateTime(2015, 4, 2), episode.DischargeDate, "DischargeDate is incorrect");
				Assert.AreEqual(new DateTime(2015, 3, 20), episode.AdmissionDate, "AdmissionDate is incorrect");

				episode = patient.Episodes.ElementAt(2);
				Assert.IsInstanceOf<Episode>(episode, "The third episode is null");

				Assert.AreEqual("Stomach cramps", episode.Diagnosis, "Diagnosis is incorrect");
				Assert.AreEqual(1, episode.PatientId, "Patient ID doesn't match");
				Assert.AreEqual(3, episode.EpisodeId, "Episode ID is incorrect");
				Assert.AreEqual(new DateTime(2015, 11, 14), episode.DischargeDate, "DischargeDate is incorrect");
				Assert.AreEqual(new DateTime(2015, 11, 12), episode.AdmissionDate, "AdmissionDate is incorrect");
			}
		}

		/// <summary>
		/// Tests to see if the API can be reached with an incorrect route pattern. This is not an exhaustive test case.
		/// </summary>
		[Test]
		public async Task TestFailGetForIncorrectRoute()
		{
			using (var testServer = TestServer.Create<RestApiTest.Startup>())
			{
				var result = await testServer.HttpClient.GetAsync(@"/patient/1/episodes");
				Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);

				result = await testServer.HttpClient.GetAsync(@"/patients/1");
				Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);

				result = await testServer.HttpClient.GetAsync(@"/patient/1/episode");
				Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);
			}
		}

		/// <summary>
		/// Test to ensure that the API returns a 404 not found if the object does not exist
		/// </summary>
		[Test]
		public async Task TestFindForIncorrectPatientId()
		{
			using (var testServer = TestServer.Create<RestApiTest.Startup>())
			{
				var result = await testServer.HttpClient.GetAsync(@"/patients/10/episodes");
				Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);
			}
		}
	}
}