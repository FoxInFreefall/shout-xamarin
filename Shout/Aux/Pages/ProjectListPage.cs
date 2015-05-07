using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class ProjectListPage : BasePage
	{
		private FormView projectForm = new ProjectForm ();
		private ListView list;
		private Button leaveProjectButton;
		private Button addProjectButton;
		private bool leaveProjectMode = false;

		public ProjectListPage ()
		{
			Title = "My Projects";
			NavigationPage.SetTitleIcon (this, "bar_icon");
			BackgroundColor = Color.White;

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Name");

			list = new ListView {
				ItemsSource = App.User.Projects,
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			list.ItemSelected += async (sender, e) => await ProjectSelected (sender as ListView);
			list.RefreshCommand = new Command (() => RefreshList ());

			Content.AddView (list, 0, 0, 1, 1);

			leaveProjectButton = ButtonFactory.Make ("-");
			leaveProjectButton.TextColor = Color.White;
			leaveProjectButton.Clicked += (sender, e) => LeaveProjectPressed ();
			Content.AddView (leaveProjectButton, 0.4, -75, 50, 50);

			addProjectButton = ButtonFactory.Make ("+");
			addProjectButton.TextColor = Color.White;
			addProjectButton.Clicked += async (sender, e) => await ShowProjectForm ();
			Content.AddView (addProjectButton, 0.6, -75, 50, 50);
		}

		private void RefreshList ()
		{
			list.ItemsSource = App.User.Projects;
			list.EndRefresh ();
		}

		private void LeaveProjectPressed ()
		{
			if (!leaveProjectMode) {
				leaveProjectButton.Text = "X";
				addProjectButton.IsEnabled = false;
			} else {
				leaveProjectButton.Text = "-";
				addProjectButton.IsEnabled = true;
			}
			leaveProjectMode ^= true;
		}

		private async Task ShowProjectForm ()
		{
			DictModel dict = await OverlayForm (projectForm);

			try {
				if (dict != null) {
					await App.CreateProject (dict.s ("title"));
					list.BeginRefresh ();
				}
			} catch (Exception ex) {
				Debug.WriteLine (ex.ToString ());
				await DisplayAlert ("Error creating project", ex.Message, "OK");
			}
		}

		private async Task ProjectSelected (ListView sender)
		{
			var p = (sender.SelectedItem as ProjectModel);
			if (p != null) {
				
				if (!leaveProjectMode) {
					sender.SelectedItem = null;
					await App.GotoPage (new ProjectPage (p));
				} else {
					bool success = await DisplayAlert ("Leaving " + p.Name, "Are you sure you want to leave this project?", "Certainly", "Nevermind");
					sender.SelectedItem = null;
					if (success) {
						await App.LeaveProject (p);
						list.BeginRefresh ();
					}
				}
			}
		}
	}
}


