using System;
using Xamarin.Forms;

using System.Diagnostics;

namespace Shout
{
	public class ButtonFactory
	{
		public static readonly Button Template = new Button ();
		static ButtonFactory ()
		{
			
		}

		public static Button Make (string text)
		{
			var product = new Button ();
			product.BackgroundColor = Template.BackgroundColor;
			product.BorderColor = Template.BorderColor;
			product.BorderRadius = Template.BorderRadius;
			product.BorderWidth = Template.BorderWidth;
			product.Text = text;
			return product;
		}
	}
}

