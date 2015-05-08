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
		private Action<Page> PageSelectedCallback;
		private RootSessionPage root;
		private static ListView projectActionList;
		private Page[] cache;

		private static ProjectModel projectContext;

		public MasterFragment (RootSessionPage root, Action<Page> PageSelected)
		{
			Title = "Root master";

			this.root = root;
			PageSelectedCallback = PageSelected;

			LoadCache ();

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Title");

			var pageList = new ListView {
				ItemsSource = cache,
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			pageList.ItemSelected += (sender, e) => ItemSelected (sender as ListView);
			Content.AddView (pageList, 0, 0, 1);
			pageList.BackgroundColor = Color.White;
			pageList.HeightRequest = 89;



			template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Text");
			template.SetValue (TextCell.TextColorProperty, Color.Black);

			projectActionList = new ListView {
				ItemsSource = new List<Label> {
					new Label { Text = "Member list" },
					new Label { Text = "Invite user to project" }
				},
				SeparatorColor = Color.Gray,
				ItemTemplate = template
			};
			projectActionList.ItemSelected += async (sender, e) => await ProjectActionSelected (sender as ListView);
			Content.Children.Add (projectActionList,
				Constraint.Constant (0),
				Constraint.RelativeToView (pageList, (parent, sibling) => {
					return sibling.Y + sibling.Height + 10;
				})
			);
			projectActionList.BackgroundColor = Color.FromHex ("F6B11A");
			projectActionList.HeightRequest = 89;



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
			Content.AddView (utilityList, 0, -45, 1);
			utilityList.BackgroundColor = Color.White;
		}

		private void LoadCache ()
		{
			cache = new Page[2];
			cache [0] = MakeNavPage (new ProjectListPage ());
			cache [1] = MakeNavPage (new InvitationPage ());
		}

		private NavigationPage MakeNavPage (Page p)
		{
			return new NavigationPage (p) { 
				Title = p.Title,
				BarBackgroundColor = Color.FromHex ("F6B11A"),
				BarTextColor = Color.White
			};
		}

		private void ItemSelected (ListView sender)
		{
			var p = (sender.SelectedItem as Page);
			if (p != null) {
				sender.SelectedItem = null;
				PageSelectedCallback.Invoke (p);
			}
		}

		private async Task ProjectActionSelected (ListView sender)
		{
			var a = (sender.SelectedItem as Label);
			if (a != null) {
				root.IsPresented = false;
				sender.SelectedItem = null;
				//TODO: don't check by title
				if (a.Text == "Member list") {
					var memberListPage = MakeNavPage (new MemberListPage (projectContext));
					PageSelectedCallback.Invoke (memberListPage);
				} else if (a.Text == "Invite user to project") {
					var currentPage = (root.Detail as NavigationPage).CurrentPage;
					BasePage theCurrentestPage;
					if (currentPage is TabbedPage) {
						var theTabbedPage = currentPage as TabbedPage;
						theCurrentestPage = theTabbedPage.CurrentPage as BasePage;
					} else {
						theCurrentestPage = currentPage as BasePage;
					}

					//TODO: WHAT'S THIS??
//					theTabbedPage.ToolbarItems

					DictModel response = await theCurrentestPage.OverlayForm (new InvitationForm (projectContext));
					if (response != null) {
						string email = response.s ("email");
						try {
							await App.InviteUserToProject (email, projectContext.Id);
							await DisplayAlert ("Sweet!", "Successfully invited " + email + " to " + projectContext.Name, "OK");
						} catch (Exception ex) {
							await DisplayAlert ("Sorry", ex.Message, "OK");
						}
					}
				}
			}
		}

		private void UtilitySelected (ListView sender)
		{
			var o = (sender.SelectedItem as Label);
			if (o != null) {
				sender.SelectedItem = null;
				if (o.Text == "Logout") {
					App.Logout ();
				}
			}
		}

		//TODO: don't use static
		public static void EnterProjectContext (ProjectModel project)
		{
			projectContext = project;
			projectActionList.IsVisible = true;
		}

		public static void ExitProjectContext ()
		{
			projectActionList.IsVisible = false;
		}
	}
}


