using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class TaskPage : BasePage
	{
		private TaskModel task;
		private TableSection members;
		public TaskPage (TaskModel task)
		{
			this.task = task;

			Title = task.Title;
			NavigationPage.SetTitleIcon (this, "bar_icon");
			BackgroundColor = Color.White;

			var table = new TableView { 
				Intent = TableIntent.Form,
				Root = new TableRoot () {
					new TableSection () {
						new TextCell { Text = task.Description, Detail = "Description" }
					},
					new TableSection () {},
					(members = new TableSection ("Members") {})
				}
			};

			Refresh ();

			Content.AddView (table, 0, 0, 1, 1);
		}

		private void Refresh ()
		{
			members.Clear ();
			members.Add (new TextCell { Text = "Add member", Command = new Command (async () => await AddMembers ()) });
			foreach (var m in task.Members)
				members.Add (new TextCell { Text = m.Email, Command = new Command (async () => await RemoveMember (m)) });
		}

		private async Task AddMembers ()
		{
			DictModel response = await OverlayForm (new TaskMemberForm (MasterFragment.projectContext, task));
			if (response != null) {
				UserModel member = null;
				foreach (var m in MasterFragment.projectContext.Members) {
					if (m.Id == response.i ("id")) {
						member = m;
						break;
					}
				}
				if (member != null) {
					IsBusy = true;
					try {
						await App.AddUserToTask (MasterFragment.projectContext, task, member);
						Refresh ();
					} catch (Exception ex) {
						await DisplayAlert ("Whoops", ex.Message, "OK");
					}
					IsBusy = false;
				}
			}
		}

		private async Task RemoveMember (UserModel member)
		{
			bool success = await DisplayAlert ("", "Remove " + member.Email + " from task?", "Yes", "No");
			if (success) {
				try {
					await App.RemoveUserFromTask (MasterFragment.projectContext, task, member);
					Refresh ();
				} catch (Exception ex) {
					await DisplayAlert ("Whoops", ex.Message, "OK");
				}
				Refresh ();
			}
		}
	}
}


