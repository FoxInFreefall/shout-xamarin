using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

using System.Diagnostics;

namespace Shout
{
	public class RootSessionPage : MasterDetailPage
	{
		public RootSessionPage ()
		{
			Title = "SHOUT!";
			NavigationPage.SetTitleIcon (this, "bar_icon");
		
			Master = new MasterFragment (this, PageSelected);
			Detail = new ContentPage ();

			//TODO: don't hardcode 1000ms
			try {
				if (App.User.Id == default (int))
					App.TryAutoLogin ().Wait (1000);
				SucceededAutoLogin ();
			} catch {
				FailedAutoLogin ();
			}
		}

		private void SucceededAutoLogin ()
		{
			//TODO: use cache
			Detail = new NavigationPage (new ProjectListPage ()) { 
				Title = "My Projects",
				BarBackgroundColor = Color.FromHex ("F6B11A"),
				BarTextColor = Color.White
			};
		}

		private void FailedAutoLogin ()
		{
			App.UseRootPage (new LandingPage ());
		}

		private void PageSelected (Page page)
		{
			if (page != null) {
				Detail = page;
				IsPresented = false;
			}
		}
	}
}


