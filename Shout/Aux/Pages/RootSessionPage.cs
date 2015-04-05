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
		
			Master = new MasterFragment (PageSelected);
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
			Detail = new ProjectListPage ();
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


