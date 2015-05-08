using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class App : Application
	{
		/********** PUBLIC **********/

		public static UserModel User { get; private set; }


		/********** PRIVATE **********/

		private ShoutApiManager ApiManager { get; set; }
		private INavigation Navigation { get; set; }


		/********** CONSTRUCTOR **********/

		private static App Instance { get; set; }

		public App ()
		{
			Instance = this;

			User = new UserModel ();
			ApiManager = ShoutApiManager.Instance;

			ButtonFactory.Template.BackgroundColor = Color.Gray;
			ButtonFactory.Template.BorderRadius = 5;

			if (ApiManager.AuthenticationToken != "")
				UseRootPage (new RootSessionPage ());
			else
				UseRootPage (new LandingPage ());
		}


		/********** LIFE CYCLE **********/

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}


		/********** NAVIGATION **********/

		public static void UseRootPage (Page page)
		{
			Instance.MainPage = page;
		}


		/********** CONTROL PANEL **********/

		public static async Task TryAutoLogin ()
		{
			DictModel response = await Instance.ApiManager.AutoLogin ();
			Debug.WriteLine ("AutoLogin response: " + response.ToString ());
			response.EnsureValid ();

			User.UpdateWithJson (response.s ("user"));
		}

		public static async Task Login (string email, string password)
		{
			DictModel response = await Instance.ApiManager.Login (email, password);
			Debug.WriteLine ("Login response: " + response.ToString ());
			response.EnsureValid ();

			User.UpdateWithJson (response.s ("user"));
			UseRootPage (new RootSessionPage ());
		}

		public static void Logout ()
		{
			Instance.ApiManager.Logout ();
			User.Reset ();
			UseRootPage (new LandingPage ());
		}

		public static async Task Register (string email, string password, string username)
		{
			DictModel response = await Instance.ApiManager.Register (email, password, username);
			Debug.WriteLine ("Register response: " + response.ToString ());
			response.EnsureValid ();

			User.UpdateWithJson (response.s ("user"));
			UseRootPage (new RootSessionPage ());
		}

		public static async Task<ProjectModel> CreateProject (string projectName)
		{
			DictModel response = await Instance.ApiManager.CreateProject (projectName);
			response.EnsureValid ();

			return User.AddProject (response.s ("project"));
		}

		public static async Task LeaveProject (ProjectModel project)
		{
			DictModel response = await Instance.ApiManager.LeaveProject (project.Id);
			response.EnsureValid ();

			User.RemoveProject (project);
		}

		public static async Task InviteUserToProject (string email, int projectId)
		{
			DictModel response = await Instance.ApiManager.InviteUserToProject (email, projectId);
			response.EnsureValid ();
		}

		public static async Task<TaskModel> CreateTask (DictModel taskDict, ProjectModel project)
		{
			taskDict.Add ("project_id", project.Id);
			DictModel response = await Instance.ApiManager.CreateTask (taskDict);
			var task = project.AddTask (response.s ("task"), taskDict.s ("list"));
			return task;
		}

		public static async Task<TaskModel> UpdateTask (TaskModel task)
		{
			DictModel response = await Instance.ApiManager.UpdateTask (task);

			return task;
		}

		public static async Task UpdateInvitations ()
		{
			DictModel response = await Instance.ApiManager.GetInvitations ();
			App.User.UpdatePotentialProjects (response);
		}

		public static async Task AcceptInvitation (ProjectModel project)
		{
			DictModel response = await Instance.ApiManager.AcceptInvitation (project.Id);
			App.User.JoinPotentialProject (project);
		}

		public static async Task DeclineInvitation (ProjectModel project)
		{
			DictModel response = await Instance.ApiManager.DeclineInvitation (project.Id);
			Debug.WriteLine (response.ToString ());
			App.User.RemovePotentialProject (project);
		}
	}
}

