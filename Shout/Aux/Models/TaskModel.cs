using System;
using System.Collections.Generic;
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
		public IReadOnlyList<UserModel> Members { get { return _Members.AsReadOnly (); } }


		/********** PRIVATE **********/

		private List<UserModel> _Members = new List<UserModel> ();


		/********** CONSTRUCTOR **********/

		public TaskModel (DictModel dict, IReadOnlyList<UserModel> memberList)
		{
			Id = dict.i ("id");
			Title = dict.s ("title");
			Description = dict.s ("description");
			ProjectId = dict.i ("project_id");

			foreach (var i in dict.l ("members")) {
				DictModel d = i.ToString ();
				UserModel member = null;
				foreach (var m in memberList) {
					if (m.Id == d.i ("id")) {
						member = m;
						break;
					}
				}
				if (member != null)
					AddMember (member);
			}
		}

		public void AddMember (UserModel member)
		{
			_Members.Add (member);
		}

		public void RemoveMember (UserModel member)
		{
			_Members.Remove (member);
		}

		//TODO: TaskModel can contain a "Save()" function as long as there's a resources system set up
		//Until then, App should handle it
	}
}

