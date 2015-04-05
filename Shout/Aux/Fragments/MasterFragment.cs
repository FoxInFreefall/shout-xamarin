using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class MasterFragment : BasePage
	{
		Action<Page> PageSelectedCallback;

		public MasterFragment (Action<Page> PageSelected)
		{
			Title = "Root master";

			PageSelectedCallback = PageSelected;

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Title");

			var pageList = new ListView {
				ItemsSource = new List<Page> {
					new ProjectListPage ()
				},
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			pageList.ItemSelected += (sender, e) => ItemSelected (sender as ListView);
			Content.AddView (pageList, 0, 0, 1);


			template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Text");

			var utilityList = new ListView {
				ItemsSource = new List<Label> {
					new Label { Text = "Logout" }
				},
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			utilityList.ItemSelected += (sender, e) => UtilitySelected (sender as ListView);
			Content.AddView (utilityList, 0, -44, 1);
		}

		private void ItemSelected (ListView sender)
		{
			var p = (sender.SelectedItem as Page);
			if (p != null) {
				sender.SelectedItem = null;
				PageSelectedCallback.Invoke (p);
			}
		}

		private void UtilitySelected (ListView sender)
		{
			var p = (sender.SelectedItem as Label);
			if (p != null) {
				sender.SelectedItem = null;
				if (p.Text == "Logout") {
					App.Logout ();
				}
			}
		}
	}
}


