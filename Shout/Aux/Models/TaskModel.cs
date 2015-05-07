using System;
using Fox;

namespace Shout
{
	public class TaskModel
	{
		/********** PUBLIC **********/

		public int Id { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public int ProjectId { get; private set; }


		/********** CONSTRUCTOR **********/

		public TaskModel (DictModel dict)
		{
			Id = dict.i ("id");
			Title = dict.s ("title");
			Description = dict.s ("description");
			ProjectId = dict.i ("project_id");
		}

		//TODO: TaskModel can contain a "Save()" function as long as there's a resources system set up
		//Until then, App should handle it
	}
}

