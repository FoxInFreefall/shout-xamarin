using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;

using System.Diagnostics;

namespace Fox
{
	public class ApiManager
	{
		/*---------- PUBLIC ----------*/
		public enum RequestMethod { GET, POST, PUT, DELETE }


		/*---------- PRIVATE ----------*/
		private int nextTicket = 0;
		private int currentlyServing = 0;
		private HttpClient client = null;


		/*---------- CONSTRUCTOR ----------*/
		protected ApiManager (string baseAddress) {
			client = new HttpClient ();
			client.BaseAddress = new Uri (baseAddress);
			client.DefaultRequestHeaders.Add ("Accept", "application/json");
		}

		protected void RegisterAuthenticationToken (string token)
		{
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Token", "token=" + token);
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
			return await response.Content.ReadAsStringAsync();
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
//			content.Headers.Add ("Authorization", "Basic Token token=p5gymjQqyQ-3UpiRNAFT");

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