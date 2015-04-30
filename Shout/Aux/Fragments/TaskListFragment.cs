using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class TaskListFragment : BasePage
	{
		public TaskListFragment ()
		{
			BackgroundColor = Color.White;

			var addTaskButton = ButtonFactory.Make ("+");
			addTaskButton.Clicked += async (sender, e) => await AddTask ();
			Content.AddView (addTaskButton, 0.5, -75, 50, 50);
		}

		private async Task AddTask ()
		{
			var project = BindingContext as ProjectModel;
			await App.GotoModalPage (new ModifyTaskPage (project.Id));
		}

		protected override void ReceivedObject (object obj)
		{
			if (obj is TaskModel) {
				var task = obj as TaskModel;

			}
		}
	}
}


