using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;

using System.Diagnostics;

namespace Shout
{
	public class ApiManager
	{
		/*---------- SINGLETON ----------*/
		private static ApiManager instance;
		public static ApiManager Instance {
			get { return instance ?? (instance = new ApiManager ()); }
		}


		/*---------- PUBLIC ----------*/
		public enum RequestMethod { GET, POST, PUT, DELETE }


		/*---------- PRIVATE ----------*/
		private int nextTicket = 0;
		private int currentlyServing = 0;
		private HttpClient client = null;


		/*---------- CONSTRUCTOR ----------*/
		private ApiManager () {
			client = new HttpClient ();
			client.BaseAddress = new Uri (Globals.BASE_URL + "/" + Globals.API_URI + "/");
		}


		/*---------- SHOUT CALLS ----------*/
		public async Task<string> Login (string email, string password)
		{
			var loginInfoDictionary = new Dictionary <string, object> {
				{ "email", email },
				{ "password", password }
			};
//			var userDictionary = new Dictionary<string, object> {
//				{ "user", loginInfoDictionary }
//			};

			var response = await MakeRequestAsync (Globals.USERS_URI, loginInfoDictionary, RequestMethod.GET);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> Register (string email, string password)
		{
			var loginInfoDictionary = new Dictionary <string, string> {
				{ "email", email },
				{ "password", password }
			};
			var userDictionary = new Dictionary<string, object> {
				{ "user", loginInfoDictionary }
			};

			var response = await MakeRequestAsync (Globals.USERS_URI, userDictionary, RequestMethod.POST);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> CreateProject (string projectName)
		{
			var projectInfoDictionary = new Dictionary <string, string> {
				{ "name", projectName }
			};
			var projectDictionary = new Dictionary<string, object> {
				{ "project", projectInfoDictionary }
			};

			var response = await MakeRequestAsync (Globals.PROJECTS_URI, projectDictionary, RequestMethod.POST);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> DestroyProject (int projectId)
		{
			var requestUri = Globals.PROJECTS_URI + "/" + projectId;

			var response = await MakeRequestAsync (requestUri, null, RequestMethod.DELETE);
			return await response.Content.ReadAsStringAsync();
		}


		/*---------- BOTTLENECK ----------*/
		private async Task<HttpResponseMessage> MakeRequestAsync (string requestUri, Dictionary<string, object> parameters, RequestMethod method)
		{
			int ticket = nextTicket++;
			while (ticket != currentlyServing) {
				await Task.Yield ();
				Debug.WriteLine ("Ticket: " + ticket + ", nextTicket: " + nextTicket + ", currentlyServing: " + currentlyServing + ", request: " + requestUri);
			}

			HttpResponseMessage response = null;
			try {
				switch (method) {
				case RequestMethod.GET:		response = await GetAsync (requestUri, parameters);		break;
				case RequestMethod.POST:	response = await PostAsync (requestUri, parameters);	break;
				case RequestMethod.PUT:		response = await PutAsync (requestUri, parameters);		break;
				case RequestMethod.DELETE:	response = await DeleteAsync (requestUri);				break;
				}
			} catch (Exception ex) {
				currentlyServing++;
				throw ex;
			}

			currentlyServing++;
			return response;
		}


		/*---------- REST ----------*/
		private async Task<HttpResponseMessage> GetAsync (string requestUri, Dictionary<string, object> parameters)
		{
			if (parameters == null)
				parameters = new Dictionary <string, object> { };

			requestUri += "?";

			foreach (KeyValuePair<string, object> entry in parameters) {
				requestUri += String.Format ("{0}={1}&", entry.Key, entry.Value.ToString ());
			}
			Debug.WriteLine (requestUri);
			return await client.GetAsync (requestUri);
		}

		private async Task<HttpResponseMessage> PostAsync (string requestUri, Dictionary<string, object> parameters)
		{
			var content = new StringContent (JsonConvert.SerializeObject (parameters));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return await client.PostAsync (requestUri, content);
		}

		private async Task<HttpResponseMessage> PutAsync (string requestUri, Dictionary<string, object> parameters)
		{
			var content = new StringContent (JsonConvert.SerializeObject (parameters));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return await client.PutAsync (requestUri, content);
		}

		private async Task<HttpResponseMessage> DeleteAsync (string requestUri)
		{
			Debug.WriteLine (requestUri);
			return await client.DeleteAsync (requestUri);
		}
	}
}