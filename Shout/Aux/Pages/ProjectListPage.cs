using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class ProjectListPage : BasePage
	{
		ListView list;

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

			Content.AddView (list, 0, 0, 1, 1);
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


