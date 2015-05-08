using System;
using System.Threading.Tasks;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class ShoutApiManager : ApiManager
	{
		/*---------- SINGLETON ----------*/
		//TODO: one-time access (returns null after first access)
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
			response.EnsureValid ();
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

		public async Task<DictModel> LeaveProject (int projectId)
		{
			return await MakeRequestAsync (Globals.PROJECTS_URI + "/" + projectId + "/" + Globals.COLLABORATION_URI, null, RequestMethod.DELETE);
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

			return await MakeRequestAsync (Globals.PROJECTS_URI + "/" + projectId + "/" + Globals.INVITATIONS_URI, dict, RequestMethod.POST);
		}

		public async Task<DictModel> CreateTask (DictModel taskDict)
		{
			DictModel dict = new DictModel ("task");
			dict.Add ("title", taskDict.s ("title"));
			dict.Add ("description", taskDict.s ("description"));
			dict.Add ("list", taskDict.s ("list"));

			string uri = Globals.PROJECTS_URI + "/" + taskDict.i ("project_id") + "/" + Globals.TASKS_URI;
			return await MakeRequestAsync (uri, dict, RequestMethod.POST);
		}

		public async Task<DictModel> UpdateTask (TaskModel task)
		{
			//TODO: is there a way to bake this in?
			if (task.ProjectId == default(int))
				throw new Exception ("Task doesn't belong to a project: aborting save.");
			
			DictModel dict = new DictModel ("task");
			dict.Add ("title", task.Title);
			dict.Add ("description", task.Description);

			//TODO: use dynamic uri's like _path's in Ruby
			string uri = Globals.PROJECTS_URI + "/" + task.ProjectId + "/" + Globals.TASKS_URI + "/" + task.Id;
			return await MakeRequestAsync (uri, dict, RequestMethod.PUT);
		}

		public async Task<DictModel> GetInvitations ()
		{
			return await MakeRequestAsync (Globals.INVITATIONS_URI, null, RequestMethod.GET);
		}

		public async Task<DictModel> AcceptInvitation (int projectId)
		{
			return await MakeRequestAsync (Globals.PROJECTS_URI + "/" + projectId + "/" + Globals.ACCEPT_INVITATION_URI, null, RequestMethod.PUT);
		}

		public async Task<DictModel> DeclineInvitation (int projectId)
		{
			return await MakeRequestAsync (Globals.PROJECTS_URI + "/" + projectId + "/" + Globals.DECLINE_INVITATION_URI, null, RequestMethod.DELETE);
		}
	}
}