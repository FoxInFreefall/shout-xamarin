using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

namespace Shout
{
	public class ProjectForm : FormView
	{
		private Entry titleEntry;

		public ProjectForm () : base ()
		{
			AddView (new BoxView { BackgroundColor = Color.Green });

			var form = new StackLayout {
				Padding = new Thickness (15)
			};

			var title = new Label { 
				Text = "CREATE PROJECT",
				VerticalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
				FontSize = 25
			};
			form.Children.Add (title);

			var fields = new StackLayout {
				Padding = new Thickness (15, 7),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			titleEntry = new Entry { Placeholder = "Title" };
			titleEntry.Completed += (sender, e) => Submit ();
			fields.Children.Add (titleEntry);
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

			return dict;
		}
	}
}


