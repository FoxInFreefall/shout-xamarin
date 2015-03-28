using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fox;

namespace Shout
{
	public class UserModel
	{
		/********** PUBLIC **********/

		public int Id { get; private set; }
		public string Email { get; private set; }
		public IReadOnlyList<ProjectModel> Projects { get { return _Projects.AsReadOnly (); } }
		public IReadOnlyList<ProjectModel> PotentialProjects { get { return _PotentialProjects.AsReadOnly (); } }


		/********** PRIVATE **********/

		private List<ProjectModel> _Projects { get; set; }
		private List<ProjectModel> _PotentialProjects { get; set; }


		/********** CONSTRUCTOR **********/

		public UserModel (string json)
		{
			_Projects = new List<ProjectModel> ();
			_PotentialProjects = new List<ProjectModel> ();

			DictModel dict = json;

			Id = dict.i ("id");
			Email = dict.s ("email");

//			foreach (string pot in dict.l ("potential_projects")) {
//				AddPotentialProject (pot);
//			}
		}

		public ProjectModel AddProject (DictModel dict)
		{
			var project = new ProjectModel (dict);
			_Projects.Add (project);
			return project;
		}

		public ProjectModel AddPotentialProject (string json)
		{
			var project = new ProjectModel (json);
			_PotentialProjects.Add (project);
			return project;
		}
	}
}

