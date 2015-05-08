using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class MemberListPage : BasePage
	{
		private ProjectModel project;
		private ListView list;
		public MemberListPage (ProjectModel project)
		{
			this.project = project;

			Title = "Members on " + project.Name;
			NavigationPage.SetTitleIcon (this, "bar_icon");
			BackgroundColor = Color.White;

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Email");

			list = new ListView {
				ItemsSource = project.Members,
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			list.ItemSelected += (sender, e) => ItemSelected (sender as ListView);
			list.RefreshCommand = new Command (async () => await RefreshList ());

			Content.AddView (list, 0, 0, 1, 1);
		}

		private async Task RefreshList ()
		{
			list.ItemsSource = project.Members;
			list.EndRefresh ();
		}

		private void ItemSelected (ListView sender)
		{
			var i = (sender.SelectedItem);
			if (i != null)
				sender.SelectedItem = null;
		}
	}
}


