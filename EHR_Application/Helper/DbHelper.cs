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
using Android.Database.Sqlite;
using Android.Database;

namespace EHR_Application.Helper
{
    public class DbHelper : SQLiteOpenHelper
    {
        private static String DB_NAME = "DimitrisDatabase";   // allakse to
        private static int DB_VER = 1;
        public static String DB_TABLE = "Task";
        public static String DB_COLUMN = "TaskName";

        public DbHelper(Context context, int idd) : base(context, DB_NAME, null, DB_VER)
        { Java.IO.File s = context.GetDatabasePath("DimitrisDatabase");      // see where your database is saved
        } 

        public override void OnCreate(SQLiteDatabase db)
        {
            string query = $"CREATE TABLE {DbHelper.DB_TABLE} (ID INTEGER PRIMARY KEY AUTOINCREMENT, {DbHelper.DB_COLUMN} TEXT NOT NULL);";
            
            db.ExecSQL(query);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            string query = $"DELETE TABLE IF EXISTS {DB_TABLE}";
            db.ExecSQL(query);
            OnCreate(db);
        }

        public void InsertNewTask(String task)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_COLUMN, task);
            db.InsertWithOnConflict(DB_TABLE, null, values, Android.Database.Sqlite.Conflict.Replace);
            db.Close();
        }

        public void deleteTask(String task)
        {
            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE, DB_COLUMN + " = ? ", new String[] { task });
            db.Close();
        }

        public List<string> getTaskList()
        {
            List<string> taskList = new List<string>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE, new string[] { DB_COLUMN }, null, null, null, null, null);
            while (cursor.MoveToNext())
            {
                int index = cursor.GetColumnIndex(DB_COLUMN);
                taskList.Add(cursor.GetString(index));
            }
            return taskList;
        }

    }
}