using System;
using System.Collections.Generic;

namespace Shout
{
	public class UserModel
	{
		public string Email { get; set; }
		public List<ProjectModel> Projects { get; private set; }
		public UserModel ()
		{
			Projects = new List<ProjectModel> ();
		}
	}
}

