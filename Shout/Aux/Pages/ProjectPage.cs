using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class ProjectPage : TabbedPage
	{
		public ProjectPage (ProjectModel model)
		{
			SetBinding (Page.TitleProperty, new Binding ("Name"));
			BindingContext = model;

			NavigationPage.SetTitleIcon (this, "no_icon");
			BackgroundColor = Color.White;

			Children.Add (new TaskListFragment { Title = "To Do", BindingContext = model });
			Children.Add (new TaskListFragment { Title = "Doing", BindingContext = model });
			Children.Add (new TaskListFragment { Title = "Done", BindingContext = model });

			SelectedItem = Children [1];
		}
	}
}


