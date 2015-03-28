using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fox
{
	public abstract class FormView : BaseRelativeLayout
	{
		public abstract Task<DictModel> GetResponse ();
	}
}


