using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Fox;

namespace Shout
{
	public class ProjectModel
	{
		/********** PUBLIC **********/

		public int Id { get; private set; }
		public string Name { get; private set; }


		/********** CONSTRUCTOR **********/

		public ProjectModel (DictModel dict)
		{
			Id = dict.i ("id");
			Name = dict.s ("name");
		}
	}
}

