using System;
using Fox;

namespace Shout
{
	public class TaskModel
	{
		/********** PUBLIC **********/

		public int Id { get; private set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int ProjectId { get; set; }


		/********** CONSTRUCTOR **********/

		public TaskModel (DictModel dict)
		{
			Id = dict.i ("id");
			Title = dict.s ("title");
			Description = dict.s ("description");
			ProjectId = dict.i ("project_id");
		}

		public TaskModel ()
		{

		}

		//TODO: TaskModel can contain a "Save()" function as long as there's a resources system set up
		//Until then, App should handle it
	}
}

