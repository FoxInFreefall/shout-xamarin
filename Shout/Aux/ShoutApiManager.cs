using System;
using System.Threading.Tasks;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class ShoutApiManager : ApiManager
	{
		/*---------- SINGLETON ----------*/
		public static readonly ShoutApiManager Instance = new ShoutApiManager ();


		/*---------- CONSTRUCTOR ----------*/
		private ShoutApiManager () : base (Globals.BASE_URL + "/" + Globals.API_URI + "/")
		{
			
		}


		/*---------- SHOUT CALLS ----------*/
		public async Task<DictModel> AutoLogin ()
		{
			return await MakeRequestAsync (Globals.SESSIONS_URI, null, RequestMethod.POST);
		}

		public async Task<DictModel> Login (string email, string password)
		{
			DictModel dict = new DictModel ("user");
			dict.Add ("email", email);
			dict.Add ("password", password);

			DictModel response = await MakeRequestAsync (Globals.SESSIONS_URI, dict, RequestMethod.POST);
			string authentication_token = response.d ("user").s ("authentication_token", true);
			RegisterAuthenticationToken (authentication_token);
			return response;
		}

		public void Logout ()
		{
			ClearAuthenticationToken ();
		}

		public async Task<DictModel> Register (string email, string password)
		{
			DictModel dict = new DictModel ("user");
			dict.Add ("email", email);
			dict.Add ("password", password);

			return await MakeRequestAsync (Globals.USERS_URI, dict, RequestMethod.POST);
		}

		public async Task<DictModel> CreateProject (string projectName)
		{
			DictModel dict = new DictModel ("project");
			dict.Add ("name", projectName);

			return await MakeRequestAsync (Globals.PROJECTS_URI, dict, RequestMethod.POST);
		}

		public async Task<DictModel> DestroyProject (int projectId)
		{
			var requestUri = Globals.PROJECTS_URI + "/" + projectId;

			return await MakeRequestAsync (requestUri, null, RequestMethod.DELETE);
		}

		public async Task<DictModel> InviteUserToProject (string email, int projectId)
		{
			DictModel dict = new DictModel ("invitation");
			dict.Add ("email", email);
			dict.Add ("project_id", projectId.ToString ());

			return await MakeRequestAsync (Globals.INVITATIONS_URI, dict, RequestMethod.POST);
		}
	}
}