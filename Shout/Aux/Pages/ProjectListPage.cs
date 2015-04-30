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

			var addTaskButton = ButtonFactory.Make ("+");
			addTaskButton.Clicked += async (sender, e) => await ShowProjectForm ();
			Content.AddView (addTaskButton, 0.5, -75, 50, 50);
		}

		private void RefreshList ()
		{
			list.ItemsSource = App.User.Projects;
			list.EndRefresh ();
		}

		private async Task ShowProjectForm ()
		{
			DictModel dict = await OverlayForm (projectForm);

			try {
				if (dict != null) {
					Debug.WriteLine (dict.ToString ());
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
				sender.SelectedItem = null;
				await App.GotoPage (new ProjectPage (p));
			}
		}
	}
}


