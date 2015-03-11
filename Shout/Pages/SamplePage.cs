using System;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shout
{
	public class SamplePage : ContentPage
	{
		private Entry projectNameField;

		public SamplePage ()
		{
			Content = new StackLayout { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label { Text = "Email:\n" + App.User.Email },
					(projectNameField = new Entry { Placeholder = "Project name" }),
					new Button { Image = "icon", Command = new Command (async () => await Submit (projectNameField.Text)) }
				}
			};
		}

		private void AddProjectLabel (ProjectModel project)
		{
			var projectLabel = new Label { Text = project.Name };
			var tap = new TapGestureRecognizer ();
			tap.Tapped += (object sender, EventArgs e) => App.GotoPage (new SampleProjectPage (project));
			projectLabel.GestureRecognizers.Add (tap);
			(Content as StackLayout).Children.Add (projectLabel);
		}

		private async Task Submit (string projectName)
		{
			var project = await App.CreateProject (projectName);
			if (project != null) {
				AddProjectLabel (project);
				projectNameField.Text = "";
			}
		}
	}
}


