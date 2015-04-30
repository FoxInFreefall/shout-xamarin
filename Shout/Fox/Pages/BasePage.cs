using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fox
{
	public class BasePage : ContentPage
	{
		private uint fadeTime = 150;

		private View obscure;
		private View form;

		public new BaseRelativeLayout Content { 
			get { return base.Content as BaseRelativeLayout; }
			private set { base.Content = value; }
		}

		public BasePage ()
		{
			Content = new BaseRelativeLayout ();
			obscure = new BoxView {
				BackgroundColor = Color.Black,
				Opacity = 0
			};

			var tap = new TapGestureRecognizer ();
			tap.Tapped += (sender, e) => { if (form != null) RemoveForm (); };
			obscure.GestureRecognizers.Add (tap);

			Content.AddView (obscure);
		}

		public async Task<DictModel> OverlayForm (FormView v, double x = 0, double y = 0, double w = 1, double h = 1)
		{
			var formScroll = new ScrollView ();
			formScroll.Content = v;

			form = formScroll;

			Content.RaiseChild (obscure);
			Content.AddView (form, 0, 0, 1, 1);

			form.Opacity = 0;

			await obscure.FadeTo (0.6, fadeTime);
			await form.FadeTo (1, fadeTime, Easing.CubicOut);

			var response = await v.GetResponse ();

			await RemoveForm ();

			return response;
		}

		public async Task RemoveForm ()
		{
			await form.FadeTo (0, fadeTime, Easing.CubicIn);
			await obscure.FadeTo (0, fadeTime);
			Content.Children.Remove (form);
			Content.LowerChild (obscure);
		}

		public void SendObject (object obj)
		{
			ReceivedObject (obj);
		}

		protected virtual void ReceivedObject (object obj)
		{

		}
	}
}


