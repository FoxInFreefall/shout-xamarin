using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fox;

namespace Shout
{
	public class TaskMemberForm : FormView
	{
		private UserModel selectedUser;
		public TaskMemberForm (ProjectModel project, TaskModel task) : base ()
		{
			AddView (new BoxView { BackgroundColor = Color.Green });

			var form = new StackLayout {
				Padding = new Thickness (15)
			};

			var title = new Label { 
				Text = "ADD MEMBER",
				VerticalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
				FontSize = 25
			};
			form.Children.Add (title);

			TableSection members;
			var table = new TableView { 
				Intent = TableIntent.Form,
				Root = new TableRoot () {
					(members = new TableSection () {})
				}
			};

			var list = new List<UserModel> (project.Members);
			foreach (var m in task.Members)
				list.Remove (m);
			foreach (var m in list)
				members.Add (new TextCell { Text = m.Email, Command = new Command (() => SelectMember (m)) });

			form.Children.Add (table);


			var buttons = new BaseRelativeLayout ();

			var cancelButton = ButtonFactory.Make ("Cancel");
			cancelButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			cancelButton.Clicked += (sender, e) => Cancel ();
			buttons.AddView (cancelButton, 0.5, 0, 0.47, 40);

			form.Children.Add (buttons);

			AddView (form);
		}

		private void SelectMember (UserModel user)
		{
			selectedUser = user;
			Submit ();
		}

		public override async Task<DictModel> GetResponse ()
		{
			int success = await Success ();

			if (success == 0)
				return null;

			DictModel dict = new DictModel ();
			dict.Add ("id", selectedUser.Id);

			return dict;
		}
	}
}


