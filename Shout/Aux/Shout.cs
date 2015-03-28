using System;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

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

			ApiManager = ShoutApiManager.Instance;

			ButtonFactory.Template.BackgroundColor = Color.Gray;
			ButtonFactory.Template.BorderRadius = 5;

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
			Instance.MainPage = new NavigationPage (page);
			Instance.Navigation = Instance.MainPage.Navigation;
		}

		public static async Task GotoPage (Page page)
		{
			await Instance.Navigation.PushAsync (page);
		}

		public static async Task PopPage ()
		{
			await Instance.Navigation.PopAsync ();
		}


		/********** CONTROL PANEL **********/

		public static async Task Login (string email, string password)
		{
			DictModel response = await Instance.ApiManager.Login (email, password);
			Debug.WriteLine ("Login response: " + response.ToString ());
			response.EnsureValid ();

			User = new UserModel (response.s ("user"));
			UseRootPage (new SamplePage ());
		}

		public static async Task Register (string email, string password)
		{
			//TODO: fix
			DictModel response = await Instance.ApiManager.Register (email, password);
			response.EnsureValid ();

			User = new UserModel (response.s ("data"));
			UseRootPage (new SamplePage ());
		}

		public static async Task<ProjectModel> CreateProject (string projectName)
		{
			//TODO: pls no, fix
			DictModel response = await Instance.ApiManager.CreateProject (projectName);
			if (response.ToString ().Contains ("id:")) {
				return User.AddProject (response);
			}
			return null;
		}

		public static async Task DestroyProject (int projectId)
		{
			//TODO: pls fix
			var response = await Instance.ApiManager.DestroyProject (projectId);
		}

		public static async Task<bool> InviteUserToProject (string email, int projectId)
		{
			//TODO: also pls fix
			var response = await Instance.ApiManager.InviteUserToProject (email, projectId);
			return !response.ToString ().Contains ("errors:");
		}
	}
}

