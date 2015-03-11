using System;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shout
{
	public class RegistrationPage : ContentPage
	{
		private Entry emailField;
		private Entry passwordField;

		public RegistrationPage ()
		{
			BackgroundColor = Color.Olive;
			Content = new StackLayout { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					(emailField = new Entry { Placeholder = "Email" }),
					(passwordField = new Entry { Placeholder = "Password", IsPassword = true }),
					new Entry { Placeholder = "Confirm Password", IsPassword = true },
					new Button { Image = "icon", Command = new Command (async () => await Submit (emailField.Text, passwordField.Text)) }
				}
			};
		}

		private async Task Submit (string email, string password)
		{
			await App.Register (email, password);
		}
	}
}


