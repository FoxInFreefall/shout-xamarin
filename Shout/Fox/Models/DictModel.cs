using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

using System.Diagnostics;

namespace Fox
{
	public class DictModel : Dictionary<string, object>
	{
		/********** CONVERTER **********/
		public static implicit operator DictModel (string s)
		{
			return (JsonConvert.DeserializeObject<DictModel> (s));
		}


		/********** PRIVATE **********/
		private DictModel root = null;


		/********** CONSTRUCTORS **********/
		public DictModel () {}
		public DictModel (string rootName)
		{
			root = new DictModel ();
			base.Add (rootName, root);
		}


		/********** GET **********/
		public DictModel d (string key, bool shouldRemove = false)
		{
			object obj = this [key];
			if (! (obj is DictModel))
				this [key] = (DictModel)obj.ToString ();
			var give = this [key] as DictModel;
			if (shouldRemove)
				this.Remove (key);
			return give;
		}

		public List<object> l (string key, bool shouldRemove = false)
		{
			object obj = this [key];
			if (! (obj is List<object>))
				this [key] = JsonConvert.DeserializeObject<List<object>> (this [key].ToString ()) ?? new List<object> ();
			var give = this [key] as List<object>;
			if (shouldRemove)
				this.Remove (key);
			return give;
		}

		public string s (string key, bool shouldRemove = false)
		{
			string give = (this.ContainsKey (key) && this [key] != null) ? this [key].ToString () : null;
			if (shouldRemove)
				this.Remove (key);
			return give;
		}

		public int i (string key, bool shouldRemove = false)
		{
			int give = int.Parse (this [key].ToString ());
			if (shouldRemove)
				this.Remove (key);
			return give;
		}

		public bool b (string key, bool shouldRemove = false)
		{
			bool give = bool.Parse (this [key].ToString ());
			if (shouldRemove)
				this.Remove (key);
			return give;
		}

		public void EnsureValid ()
		{
			if (ContainsKey ("error"))
				throw new Exception (s ("message"));
		}


		/********** SET **********/
		public new void Add (string key, object value)
		{
			var which = root ?? this;
			which [key] = value;
		}


		/********** EXPORT **********/
		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this);
		}

		public StringContent ToContent ()
		{
			return new StringContent (ToString ());
		}
	}
}

