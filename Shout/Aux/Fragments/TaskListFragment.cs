using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class TaskListFragment : BasePage
	{
		private ProjectModel project;
		private string listName;
		private FormView taskForm = new TaskForm ();
		private ListView list;
		public TaskListFragment (ProjectModel project, string listName)
		{
			this.project = project;
			this.listName = listName;

			BackgroundColor = Color.White;

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Title");

			list = new ListView {
				ItemsSource = project.GetList (listName),
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			list.ItemSelected += async (sender, e) => await TaskSelected (sender as ListView);
			list.RefreshCommand = new Command (() => RefreshList ());

			Content.AddView (list, 0, 0, 1, 1);

			var addTaskButton = ButtonFactory.Make ("+");
			addTaskButton.TextColor = Color.White;
			addTaskButton.Clicked += async (sender, e) => await AddTask ();
			Content.AddView (addTaskButton, 0.5, -75, 50, 50);
		}

		private void RefreshList ()
		{
			//TODO: going to have to pull from server
			list.ItemsSource = project.GetList (listName);
			list.EndRefresh ();
		}

		private async Task AddTask ()
		{
			DictModel dict = await OverlayForm (taskForm);
			if (dict != null) {
				dict.Add ("list", listName);

				await App.CreateTask (dict, project);
				list.BeginRefresh ();
			}
		}

		private async Task TaskSelected (ListView sender)
		{
			var t = (sender.SelectedItem as TaskModel);
			if (t != null) {
				sender.SelectedItem = null;
				Debug.WriteLine ("Selected task: " + t.Title);
				await Navigation.PushAsync (new TaskPage (t));
			}
		}
	}
}


