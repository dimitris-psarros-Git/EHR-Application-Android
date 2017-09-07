using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using EHR_Application.Helper;

namespace EHR_Application.Activities
{
    [Activity(Label = "    Notes  ",Theme = "@style/Theme.AppCompat.Light" )]
    public class ToDoActivity : AppCompatActivity
    {
        EditText taskEditText;
        DbHelper dbHelper;
        Customadapter adapter;
        ListView lstTask;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_item, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    taskEditText = new EditText(this);
                    Android.Support.V7.App.AlertDialog dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                        .SetTitle("Add New Task")
                        .SetMessage("What do you want to do ?")
                        .SetView(taskEditText)
                        .SetPositiveButton("Add", OkAction)
                        .SetNegativeButton("Cancel", CancelAction)
                        .Create();
                    dialog.Show();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CancelAction(object sender, DialogClickEventArgs e)
        {

        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            string task = taskEditText.Text;
            dbHelper.InsertNewTask(task);
            LoadTaskList();
        }

        public void LoadTaskList()
        {
            List<string> taskList = dbHelper.getTaskList();
            adapter = new Customadapter(this, taskList, dbHelper);
            lstTask.Adapter = adapter;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mainToDoLayout);
            // Create your application here

            dbHelper = new DbHelper(this,1002);
            lstTask = FindViewById<ListView>(Resource.Id.lstTask);

            // Load Data
            LoadTaskList();
        }
    }
}