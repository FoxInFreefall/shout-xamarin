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
		private FormView registerForm = new RegistrationForm ();
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
			regButton.Clicked += async (sender, e) => await ShowRegister ();
			buttonGroup.Children.Add (regButton);

			Content.AddView (buttonGroup, 0.5, 0.75, 200);
		}

		private async Task ShowLogin ()
		{
			bool success = false;

			while (!success) {
				success = true;
				DictModel dict = await OverlayForm (loginForm);

				try {
					if (dict != null)
						await App.Login (dict.s ("email"), dict.s ("password"));
				} catch (Exception ex) {
					Debug.WriteLine (ex.ToString ());
					await DisplayAlert ("Error", ex.Message, "OK");
					success = false;
				}
			}
		}

		private async Task ShowRegister ()
		{
			bool success = false;

			while (!success) {
				success = true;
				DictModel dict = await OverlayForm (registerForm);
				try {
					if (dict != null) {
						if (dict.ContainsKey ("email") &&
						    dict.ContainsKey ("username") &&
						    dict.ContainsKey ("password") &&
						    dict.ContainsKey ("confPassword")) {

							if (dict.s ("password") == dict.s ("confPassword")) {
								await App.Register (dict.s ("email"), dict.s ("password"), dict.s ("username"));
							} else {
								throw new Exception ("Passwords don't match.");
							}
						} else {
							throw new Exception ("All fields much be completed.");
						}
					}
				} catch (Exception ex) {
					Debug.WriteLine ("ShowRegister(): " + ex.ToString ());
					await DisplayAlert ("Error", ex.Message, "OK");
					success = false;
				}
			}
		}
	}
}


