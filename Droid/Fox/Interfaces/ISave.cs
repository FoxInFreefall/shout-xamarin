using System;
using Android.Content;
using Android.Preferences;

using Shout.Droid;
using System.Diagnostics;

namespace Fox
{
	public class ISave
	{
		public static readonly ISave Instance = new ISave ();

		public string GetString (string key)
		{
			var prefs = PreferenceManager.GetDefaultSharedPreferences (MainActivity.Context);
			return prefs.GetString (key, "");
		}

		public void PutString (string key, string value)
		{
			var prefs = PreferenceManager.GetDefaultSharedPreferences (MainActivity.Context);
			var editor = prefs.Edit ();
			editor.PutString (key, value);
			editor.Apply ();
		}

		public void Remove (string key)
		{
			var prefs = PreferenceManager.GetDefaultSharedPreferences (MainActivity.Context);
			var editor = prefs.Edit ();
			editor.Remove (key);
			editor.Apply ();
		}
	}
}

