using System;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shout
{
	public class SampleProjectPage : ContentPage
	{
		private ProjectModel project;

		private Entry invitationField;

		public SampleProjectPage (ProjectModel project)
		{
			this.project = project;
			Content = new StackLayout { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label { Text = "Project id:\n" + project.Id },
					new Label { Text = "Project name:\n" + project.Name },
					new Button { Image = "icon", Command = new Command (async () => await Destroy ()) },
					(invitationField = new Entry { Placeholder = "User email" }),
					new Button { Image = "icon", Command = new Command (async () => await Submit (invitationField.Text)) }
				}
			};
		}

		private void AddInvitationLabel (string email)
		{
			var invitationLabel = new Label { Text = email };
//			var tap = new TapGestureRecognizer ();
//			tap.Tapped += (object sender, EventArgs e) => App.GotoPage (new SampleProjectPage (project));
//			invitationLabel.GestureRecognizers.Add (tap);
			(Content as StackLayout).Children.Add (invitationLabel);
		}

		private async Task Destroy ()
		{
			await App.DestroyProject (project.Id);
		}

		private async Task Submit (string email)
		{
			bool success = await App.InviteUserToProject (email, project.Id);
			if (success) {
				AddInvitationLabel (email);
				invitationField.Text = "";
			}
		}
	}
}


