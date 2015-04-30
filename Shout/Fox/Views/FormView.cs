using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fox
{
	public abstract class FormView : BaseRelativeLayout
	{
		private int success = -1;

		protected FormView ()
		{
			var tap = new TapGestureRecognizer ();
			GestureRecognizers.Add (tap);
		}

		public void Submit ()
		{
			success = 1;
		}

		public void Cancel ()
		{
			success = 0;
		}

		protected async Task<int> Success ()
		{
			while (success == -1)
				await Task.Delay (10);
			int s = success;
			success = -1;
			return s;
		}

		public abstract Task<DictModel> GetResponse ();
	}
}


