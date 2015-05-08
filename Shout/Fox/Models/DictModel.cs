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
			return (s != null && s != "") ? JsonConvert.DeserializeObject<DictModel> (s) : new DictModel ();
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
			int give = (this.ContainsKey (key)) ? int.Parse (this [key].ToString ()) : default (int);
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
			if (ContainsKey ("error")) {
				Debug.WriteLine (ToString ());
				if (ContainsKey ("message"))
					throw new Exception (s ("message"));
				else
					throw new Exception (s ("error"));
			} else if (ContainsKey ("errors")) {
				DictModel errors = d ("errors");
				string subject = null;
				foreach (var s in errors.Keys) {
					subject = s;
					break;
				}
				var compaints = errors.l (subject);
				string firstComplaint = compaints [0].ToString ();
				string error = subject [0].ToString ().ToUpper () + ((subject.Length > 1) ? subject.Substring (1, subject.Length - 1) : "") + " " + firstComplaint + ".";
				throw new Exception (error);	
			} else if (ContainsKey ("status_code") && !b ("status_code")) {
				Debug.WriteLine (ToString ());
				throw new Exception ("Something went wrong.");
			}
			Remove ("status_code");
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

