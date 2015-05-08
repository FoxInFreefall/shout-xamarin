using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class InvitationPage : BasePage
	{
		private ListView list;
		public InvitationPage ()
		{
			Title = "My Invitations";
			NavigationPage.SetTitleIcon (this, "bar_icon");
			BackgroundColor = Color.White;

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Name");

			list = new ListView {
				ItemsSource = App.User.PotentialProjects,
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			list.ItemSelected += async (sender, e) => await ProjectSelected (sender as ListView);
			list.RefreshCommand = new Command (async () => await RefreshList ());

			Content.AddView (list, 0, 0, 1, 1);
		}

		private async Task ProjectSelected (ListView sender)
		{
			var p = (sender.SelectedItem as ProjectModel);
			if (p != null) {
				bool success = await DisplayAlert ("Joining " + p.Name, "You've been invited to join this project! Would you like to?", "Certainly", "No thank you");
				sender.SelectedItem = null;
				IsBusy = true;
				try {
					if (success) {
						await App.AcceptInvitation (p);
						LocalRefresh ();
						await DisplayAlert ("Sweet!", "You can check out your new project on the project page.", "OK");
					} else {
						await App.DeclineInvitation (p);
						LocalRefresh ();
						await DisplayAlert ("Done", "You've declined the invitation to the project.", "OK");
					}
				} catch (Exception ex) {
					await DisplayAlert ("Sorry", ex.Message, "OK");
				} finally {
					IsBusy = false;
				}
			}
		}

		private void LocalRefresh ()
		{
			list.ItemsSource = App.User.PotentialProjects;
		}

		private async Task RefreshList ()
		{
			await App.UpdateInvitations ();
			list.ItemsSource = App.User.PotentialProjects;
			list.EndRefresh ();
		}
	}
}


