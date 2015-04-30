using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class ModifyTaskPage : BasePage
	{
		// Create
		public ModifyTaskPage (int projectId)
		{
			Initialize ();
			BindingContext = new TaskModel { ProjectId = projectId };
		}

		// Edit
		public ModifyTaskPage (TaskModel model)
		{
			Initialize ();
			BindingContext = model;
		}

		private void Initialize ()
		{
			NavigationPage.SetTitleIcon (this, "no_icon");
			BackgroundColor = Color.White;

			SetBinding (Page.TitleProperty, new Binding ("Title"));

			// Motors
			var titleEntry = new Entry ();
			var descriptionEntry = new Entry ();
			var cancelButton = ButtonFactory.Make ("Cancel");
			var submitButton = ButtonFactory.Make ("Save");

			// Actions
			var submitAction = new EventHandler (async (sender, e) => await Submit (titleEntry.Text, descriptionEntry.Text));

			// Paths
			titleEntry.TextChanged += (sender, e) => TitleChanged (e.NewTextValue);
			titleEntry.Completed += (sender, e) => descriptionEntry.Focus ();
			descriptionEntry.Completed += submitAction;
			cancelButton.Clicked += async (sender, e) => await Cancel ();
			submitButton.Clicked += submitAction;

			var layout = new StackLayout {
				Children = {
					titleEntry,
					descriptionEntry,
					cancelButton,
					submitButton
				}
			};
			Content.AddView (layout, 0, 0, 1, 1);
		}

		private void TitleChanged (string newTitle)
		{
			Title = newTitle;
		}

		private async Task Cancel ()
		{
			await App.PopPage ();
		}

		private async Task Submit (string title, string description)
		{
			var task = BindingContext as TaskModel;
			task.Title = title;
			task.Description = description;
			//TODO: need the project_id somehow
			await App.SaveTask (task);
			await App.PopModalPage (task);
		}
	}
}


