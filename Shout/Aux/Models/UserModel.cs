using System;
using System.Collections.Generic;
using Fox;

using System.Diagnostics;

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

		private List<ProjectModel> _Projects = new List<ProjectModel> ();
		private List<ProjectModel> _PotentialProjects = new List<ProjectModel> ();


		/********** CONSTRUCTOR **********/

		public UserModel () { }

		public void UpdateWithJson (string json)
		{
			DictModel dict = json;

			Id = dict.i ("id");
			Email = dict.s ("email");

			foreach (var p in dict.l ("projects")) {
				AddProject (p.ToString ());
			}
			foreach (var p in dict.l ("potential_projects"))
				AddPotentialProject (p.ToString ());
		}

		public ProjectModel AddProject (DictModel dict)
		{
			var project = new ProjectModel (dict);
			_Projects.Add (project);
			return project;
		}

		public void RemoveProject (ProjectModel p)
		{
			_Projects.Remove (p);
		}

		public ProjectModel AddPotentialProject (DictModel dict)
		{
			var project = new ProjectModel (dict);
			_PotentialProjects.Add (project);
			return project;
		}

		public void Reset ()
		{
			Id = default (int);
			Email = default (string);
			_Projects = new List<ProjectModel> ();
			_PotentialProjects = new List<ProjectModel> ();
		}
	}
}

