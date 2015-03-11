using System;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shout
{
	public class SampleProjectPage : ContentPage
	{
		private ProjectModel project;

		public SampleProjectPage (ProjectModel project)
		{
			this.project = project;
			Content = new StackLayout { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label { Text = "Project id:\n" + project.Id },
					new Label { Text = "Project name:\n" + project.Name },
					new Button { Image = "icon", Command = new Command (async () => await Destroy ()) }
				}
			};
		}

		private async Task Destroy ()
		{
			await App.DestroyProject (project.Id);
			App.UseRootPage (new SamplePage ());
		}
	}
}


