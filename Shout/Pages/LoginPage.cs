using System;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shout
{
	public class LoginPage : ContentPage
	{
		private Entry emailField;
		private Entry passwordField;

		public LoginPage ()
		{
			BackgroundColor = Color.Silver;
			Content = new StackLayout { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					(emailField = new Entry { Placeholder = "Email" }),
					(passwordField = new Entry { Placeholder = "Password", IsPassword = true }),
					new Button { Image = "icon", Command = new Command (async () => await Submit (emailField.Text, passwordField.Text)) }
				}
			};
		}

		private async Task Submit (string email, string password)
		{
			await App.Login (email, password);
		}
	}
}


