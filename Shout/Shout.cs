using System;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shout
{
	public class App : Application
	{
		private static App Instance;
		private INavigation Navigation;

		public static UserModel User { get; private set; }
		public static ApiManager ApiManager { get; private set; }

		public App ()
		{
			Instance = this;

			ApiManager = ApiManager.Instance;

			// The root page of your application
			UseRootPage (new LandingPage ());
		}

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

		public static async Task Login (string email, string password)
		{
			var response = await App.ApiManager.Login (email, password);
			Debug.WriteLine (response);
			if (response == "{}") {
				User = new UserModel ();
				User.Email = email;
				UseRootPage (new SamplePage ());
			}
		}

		public static async Task Register (string email, string password)
		{
			var response = await App.ApiManager.Register (email, password);
			if (response == "{}") {
				User = new UserModel ();
				User.Email = email;
				UseRootPage (new SamplePage ());
			}
		}

		public static async Task<ProjectModel> CreateProject (string projectName)
		{
			var response = await App.ApiManager.CreateProject (projectName);
			Debug.WriteLine ("Project response: " + response);
			if (response.ToString ().Contains ("id:")) {
				var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>> (response);

				var project = new ProjectModel ();
				project.Id = int.Parse (responseDict ["id"].ToString ());
				project.Name = projectName;
				User.Projects.Add (project);
				return project;
			}
			return null;
		}

		public static async Task DestroyProject (int projectId)
		{
			var response = await App.ApiManager.DestroyProject (projectId);
			Debug.WriteLine ("Project response: " + response);
		}
	}
}

