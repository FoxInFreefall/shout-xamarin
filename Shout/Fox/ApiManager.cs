using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Fox
{
	public class ApiManager
	{
		/*---------- PUBLIC ----------*/
		public enum RequestMethod { GET, POST, PUT, DELETE }


		/*---------- PROPERTIES ----------*/
		public string AuthenticationToken {
			get { return ISave.Instance.GetString ("authentication_token"); }
			set { ISave.Instance.PutString ("authentication_token", value); }
		}


		/*---------- PRIVATE ----------*/
		private int nextTicket = 0;
		private int currentlyServing = 0;
		private HttpClient client = null;


		/*---------- CONSTRUCTOR ----------*/
		protected ApiManager (string baseAddress) {
			client = new HttpClient ();
			client.BaseAddress = new Uri (baseAddress);
			client.DefaultRequestHeaders.Add ("Accept", "application/json");
			RegisterAuthenticationToken (AuthenticationToken);
		}

		protected void RegisterAuthenticationToken (string token)
		{
			AuthenticationToken = token;
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Token", "token=" + token);
		}

		protected void ClearAuthenticationToken ()
		{
			ISave.Instance.Remove ("authentication_token");
			client.DefaultRequestHeaders.Authorization = null;
		}


		/*---------- BOTTLENECK ----------*/
		protected async Task<DictModel> MakeRequestAsync (string requestUri, DictModel parameters, RequestMethod method)
		{
			int ticket = nextTicket++;
			while (ticket != currentlyServing) {
				await Task.Yield ();
				Debug.WriteLine ("Ticket: " + ticket + ", nextTicket: " + nextTicket + ", currentlyServing: " + currentlyServing + ", request: " + requestUri);
			}
			Debug.WriteLine ("Running ticket: " + ticket + ", request: " + method + " " + requestUri);

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
			DictModel dict = null;
			try {
				dict = await response.Content.ReadAsStringAsync ();
			} catch {
				dict = new DictModel ();
			}
			dict.Add ("status_code", response.IsSuccessStatusCode);
			return dict;
		}


		/*---------- REST ----------*/
		private async Task<HttpResponseMessage> GetAsync (string requestUri, DictModel parameters)
		{
			parameters = parameters ?? new DictModel ();

			requestUri += "?";

			foreach (var entry in parameters) {
				requestUri += String.Format ("&{0}={1}", entry.Key, entry.Value.ToString ());
			}
			Debug.WriteLine (requestUri);
			return await client.GetAsync (requestUri);
		}

		private async Task<HttpResponseMessage> PostAsync (string requestUri, DictModel parameters)
		{
			parameters = parameters ?? new DictModel ();
			var content = parameters.ToContent ();
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return await client.PostAsync (requestUri, content);
		}

		private async Task<HttpResponseMessage> PutAsync (string requestUri, DictModel parameters)
		{
			parameters = parameters ?? new DictModel ();
			var content = parameters.ToContent ();
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