using System;
using Xamarin.Forms;

using System.Diagnostics;

namespace Fox
{
	public class BaseRelativeLayout : RelativeLayout
	{
		public BaseRelativeLayout ()
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand;
			VerticalOptions = LayoutOptions.CenterAndExpand;
		}

		public void AddView (View view)
		{
			AddView (view, 0, 0, 1, 1);
		}

		public void AddView (View view, double x = -1, double y = -1, double w = -1, double h = -1)
		{
			Constraint xConstraint = null;
			if (x > 0 && x <= 1)
				xConstraint = Constraint.RelativeToParent ((parent) => {
					return parent.Width * x - ((w>0&&w<=1)?(w*parent.Width):(w)) / 2;
				});
			else if (x >= 0)
				xConstraint = Constraint.Constant (x);
			else if (x <= -1)
				xConstraint = Constraint.RelativeToParent ((parent) => {
					return parent.Width + x;
				});

			Constraint yConstraint = null;
			if (y > 0 && y <= 1)
				yConstraint = Constraint.RelativeToParent ((parent) => {
					return parent.Height * y - ((h>0&&h<=1)?(h*parent.Height):(h)) / 2;
				});
			else if (y >= 0)
				yConstraint = Constraint.Constant (y);
			else if (y <= -1)
				yConstraint = Constraint.RelativeToParent ((parent) => {
					return parent.Height + y;
				});

			Constraint wConstraint = null;
			if (w > 0 && w <= 1)
				wConstraint = Constraint.RelativeToParent ((parent) => {
					return parent.Width * w;
				});
			else if (w >= 0)
				wConstraint = Constraint.Constant (w);

			Constraint hConstraint = null;
			if (h > 0 && h <= 1)
				hConstraint = Constraint.RelativeToParent ((parent) => {
					return parent.Height * h;
				});
			else if (h >= 0)
				hConstraint = Constraint.Constant (h);

			Children.Add (
				view,
				xConstraint,
				yConstraint,
				wConstraint,
				hConstraint
			);
		}
	}
}

