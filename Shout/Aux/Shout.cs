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
			Instance.MainPage = new NavigationPage (page) {
				BarBackgroundColor = Color.FromHex ("F6B11A"),
				BarTextColor = Color.White
			};
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

		public static async Task Register (string email, string password)
		{
			//TODO: fix
			DictModel response = await Instance.ApiManager.Register (email, password);
			response.EnsureValid ();

			User.UpdateWithJson (response.s ("user"));
		}

		public static async Task<ProjectModel> CreateProject (string projectName)
		{
			DictModel response = await Instance.ApiManager.CreateProject (projectName);
			response.EnsureValid ();

			return User.AddProject (response);
		}

		public static async Task DestroyProject (int projectId)
		{
			DictModel response = await Instance.ApiManager.DestroyProject (projectId);
			response.EnsureValid ();
		}

		public static async Task<bool> InviteUserToProject (string email, int projectId)
		{
			//TODO: also pls fix
			var response = await Instance.ApiManager.InviteUserToProject (email, projectId);
			return !response.ToString ().Contains ("errors:");
		}
	}
}

