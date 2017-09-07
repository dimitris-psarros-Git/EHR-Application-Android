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
using EHR_Application.Activities;
using Java.Lang;

namespace EHR_Application.Helper
{
    public class Customadapter : Android.Widget.BaseAdapter
    {
        private ToDoActivity toDoActivity;
        private List<string> taskList;
        private DbHelper dbHelper;

        public Customadapter(ToDoActivity toDoActivity, List<string> taskList, DbHelper dbHelper)
        {
            this.toDoActivity = toDoActivity;
            this.taskList = taskList;
            this.dbHelper = dbHelper;
        }

        public override int Count
        {
            get
            {
                return taskList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)toDoActivity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.rowToDolayout,null);

            TextView txtTask = view.FindViewById<TextView>(Resource.Id.task_title);
            Button btnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);

            txtTask.Text = taskList[position];
            btnDelete.Click += delegate
            {
                string task = taskList[position];
                dbHelper.deleteTask(task);
                toDoActivity.LoadTaskList(); // Reload  
            };
            return view;
        }
    }
}