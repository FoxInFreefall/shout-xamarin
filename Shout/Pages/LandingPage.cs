using System;

using Xamarin.Forms;

namespace Shout
{
	public class LandingPage : ContentPage
	{
		public LandingPage ()
		{
			BackgroundImage = "icon";
			Content = new StackLayout { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Button { Image = "icon", Command = new Command (async (s) => await App.GotoPage (new LoginPage ())) },
					new Button { Image = "icon", Command = new Command (async (s) => await App.GotoPage (new RegistrationPage ())) }
				}
			};
		}
	}
}


