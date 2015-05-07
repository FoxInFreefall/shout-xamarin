using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

namespace Shout
{
	public class TaskForm : FormView
	{
		private Entry titleEntry;
		private Entry descriptionEntry;

		public TaskForm () : base ()
		{
			AddView (new BoxView { BackgroundColor = Color.Green });

			var form = new StackLayout {
				Padding = new Thickness (15)
			};

			var title = new Label { 
				Text = "CREATE TASK",
				VerticalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
				FontSize = 25
			};
			form.Children.Add (title);

			var fields = new StackLayout {
				Padding = new Thickness (15, 7),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			titleEntry = new Entry { Placeholder = "Title", Keyboard = Keyboard.Create (KeyboardFlags.CapitalizeSentence) };
			descriptionEntry = new Entry { Placeholder = "Description" };

			titleEntry.Completed += (sender, e) => descriptionEntry.Focus ();
			descriptionEntry.Completed += (sender, e) => Submit ();

			fields.Children.Add (titleEntry);
			fields.Children.Add (descriptionEntry);
			form.Children.Add (fields);

			var buttons = new BaseRelativeLayout ();

			var cancelButton = ButtonFactory.Make ("Cancel");
			cancelButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			cancelButton.Clicked += (sender, e) => Cancel ();
			buttons.AddView (cancelButton, 0.25, 0, 0.47, 40);

			var submitButton = ButtonFactory.Make ("Create");
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
			dict.Add ("title", titleEntry.Text);
			dict.Add ("description", descriptionEntry.Text);

			return dict;
		}
	}
}


