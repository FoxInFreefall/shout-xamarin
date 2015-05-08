using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

namespace Shout
{
	public class RegistrationForm : FormView
	{
		private Entry emailEntry;
		private Entry usernameEntry;
		private Entry passwordEntry;
		private Entry confPasswordEntry;

		public RegistrationForm () : base ()
		{
			AddView (new BoxView { BackgroundColor = Color.Green });

			var form = new StackLayout {
				Padding = new Thickness (15)
			};

			var title = new Label { 
				Text = "REGISTRATION",
				VerticalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
				FontSize = 25
			};
			form.Children.Add (title);

			var fields = new StackLayout {
				Padding = new Thickness (15, 7),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			emailEntry = new Entry { Placeholder = "email", Keyboard = Keyboard.Email };
			usernameEntry = new Entry { Placeholder = "username" };
			passwordEntry = new Entry { Placeholder = "password", IsPassword = true };
			confPasswordEntry = new Entry { Placeholder = "confirm password", IsPassword = true };

			emailEntry.Completed += (sender, e) => usernameEntry.Focus ();
			usernameEntry.Completed += (sender, e) => passwordEntry.Focus ();
			passwordEntry.Completed += (sender, e) => confPasswordEntry.Focus ();
			confPasswordEntry.Completed += (sender, e) => Submit ();

			fields.Children.Add (emailEntry);
//			fields.Children.Add (usernameEntry);
			fields.Children.Add (passwordEntry);
			fields.Children.Add (confPasswordEntry);
			form.Children.Add (fields);

			var buttons = new BaseRelativeLayout ();

			var cancelButton = ButtonFactory.Make ("Cancel");
			cancelButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			cancelButton.Clicked += (sender, e) => Cancel ();
			buttons.AddView (cancelButton, 0.25, 0, 0.47, 40);

			var submitButton = ButtonFactory.Make ("Register");
			submitButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			submitButton.Clicked += (sender, e) => Submit ();
			buttons.AddView (submitButton, 0.75, 0, 0.47, 40);

			form.Children.Add (buttons);

			AddView (form);
		}

		public override async Task<DictModel> GetResponse ()
		{
			int success = await Success ();

			if (success == 0)
				return null;

			DictModel dict = new DictModel ();
			dict.Add ("email", emailEntry.Text);
//			dict.Add ("username", usernameEntry.Text);
			dict.Add ("username", "stub");
			dict.Add ("password", passwordEntry.Text);
			dict.Add ("confPassword", confPasswordEntry.Text);

			return dict;
		}
	}
}


