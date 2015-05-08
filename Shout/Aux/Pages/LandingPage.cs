using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class LandingPage : BasePage
	{
		private FormView loginForm = new LoginForm ();
		public LandingPage ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			BackgroundImage = "landing_bg";

			var buttonGroup = new StackLayout {
				Orientation = StackOrientation.Vertical,
				WidthRequest = 200
			};

			var loginButton = ButtonFactory.Make ("Login");
			loginButton.Clicked += async (sender, e) => await ShowLogin ();
			buttonGroup.Children.Add (loginButton);

			var regButton = ButtonFactory.Make ("Register");
			//TODO: make this a form
//			regButton.Clicked += async (sender, e) => await App.GotoPage (new RegistrationPage ());
			buttonGroup.Children.Add (regButton);

			Content.AddView (buttonGroup, 0.5, 0.75, 200);
		}

		private async Task ShowLogin ()
		{
			DictModel dict = await OverlayForm (loginForm);

			try {
				if (dict != null)
					await App.Login (dict.s ("email"), dict.s ("password"));
			} catch (Exception ex) {
				Debug.WriteLine (ex.ToString ());
				await DisplayAlert ("Error", ex.Message, "OK");
			}
		}
	}
}


