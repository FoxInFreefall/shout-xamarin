using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

namespace Shout
{
	public class LoginForm : FormView
	{
		private Entry emailEntry;
		private Entry passwordEntry;
		private int success = -1;

		public LoginForm ()
		{
			var tap = new TapGestureRecognizer ();
			tap.Tapped += (sender, e) => Cancel ();
			GestureRecognizers.Add (tap);

			var subLayout = new BaseRelativeLayout ();

			subLayout.AddView (new BoxView { BackgroundColor = Color.Green });

			var form = new StackLayout {
				Padding = new Thickness (15)
			};

			var title = new Label { 
				Text = "LOG IN",
				VerticalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
				FontSize = 25
			};
			form.Children.Add (title);

			var fields = new StackLayout {
				Padding = new Thickness (15, 7),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			emailEntry = new Entry { Placeholder = "email" };
			passwordEntry = new Entry { Placeholder = "password", IsPassword = true };
			emailEntry.Completed += (sender, e) => passwordEntry.Focus ();
			fields.Children.Add (emailEntry);
			fields.Children.Add (passwordEntry);
			form.Children.Add (fields);

			var buttons = new BaseRelativeLayout ();

			var cancelButton = ButtonFactory.Make ("Cancel");
			cancelButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			cancelButton.Clicked += (sender, e) => Cancel ();
			buttons.AddView (cancelButton, 0.25, 0, 0.47, 40);

			var submitButton = ButtonFactory.Make ("Log in");
			submitButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			submitButton.Clicked += (sender, e) => Submit ();
			buttons.AddView (submitButton, 0.75, 0, 0.47, 40);

			form.Children.Add (buttons);

			subLayout.AddView (form);

			AddView (subLayout, 0.5, 0.3, 0.9, 0.35);
		}

		private void Submit ()
		{
			success = 1;
		}

		private void Cancel ()
		{
			success = 0;
		}

		public override async Task<DictModel> GetResponse ()
		{
			while (success == -1)
				await Task.Delay (10);
			int s = success;
			success = -1;

			if (s == 0)
				return null;

			DictModel dict = new DictModel ();
			dict.Add ("email", emailEntry.Text);
			dict.Add ("password", passwordEntry.Text);

			return dict;
		}
	}
}


