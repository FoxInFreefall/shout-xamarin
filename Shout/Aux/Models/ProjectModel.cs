using System;
using System.Collections.Generic;
using Fox;

namespace Shout
{
	public class ProjectModel
	{
		/********** PUBLIC **********/

		public int Id { get; private set; }
		public string Name { get; private set; }
		public IReadOnlyList<TaskModel> Tasks { get { return _Tasks.AsReadOnly (); } }


		/********** PRIVATE **********/

		private List<TaskModel> _Tasks = new List<TaskModel> ();


		/********** CONSTRUCTOR **********/

		public ProjectModel (DictModel dict)
		{
			Id = dict.i ("id");
			Name = dict.s ("name");
		}
	}
}

