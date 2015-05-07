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
		public IReadOnlyList<TaskModel> TasksTodo { get { return _TasksTodo.AsReadOnly (); } }
		public IReadOnlyList<TaskModel> TasksDoing { get { return _TasksDoing.AsReadOnly (); } }
		public IReadOnlyList<TaskModel> TasksDone { get { return _TasksDone.AsReadOnly (); } }


		/********** PRIVATE **********/

		private List<TaskModel> _TasksTodo = new List<TaskModel> ();
		private List<TaskModel> _TasksDoing = new List<TaskModel> ();
		private List<TaskModel> _TasksDone = new List<TaskModel> ();


		/********** CONSTRUCTOR **********/

		public ProjectModel (DictModel dict)
		{
			Id = dict.i ("id");
			Name = dict.s ("name");

			foreach (var t in dict.l ("tasks_todo"))
				AddTask (t.ToString (), "todo");
			foreach (var t in dict.l ("tasks_doing"))
				AddTask (t.ToString (), "doing");
			foreach (var t in dict.l ("tasks_done"))
				AddTask (t.ToString (), "done");
		}

		public TaskModel AddTask (DictModel dict, string listName)
		{
			List<TaskModel> list = null;
			if (listName == "todo")
				list = _TasksTodo;
			else if (listName == "doing")
				list = _TasksDoing;
			else if (listName == "done")
				list = _TasksDone;

			if (list != null) {
				var task = new TaskModel (dict);
				list.Add (task);
				return task;
			}
			return null;
		}

		public IReadOnlyList<TaskModel> GetList (string listName)
		{
			if (listName == "todo")
				return TasksTodo;
			else if (listName == "doing")
				return TasksDoing;
			else if (listName == "done")
				return TasksDone;
			else
				return null;
		}
	}
}

