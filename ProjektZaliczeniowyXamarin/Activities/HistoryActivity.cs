using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using ProjektZaliczeniowyXamarin.Models;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyXamarin.Activities
{
    [Activity(Label = "aha")]
    public class HistoryActivity : Activity
    {
        View mLoadingView;
        ListView listView;
        SQLiteConnection connection;
        int mShortAnimationDuration;
        private string dbPath = Path.Combine(System.Environment.GetFolderPath(
        System.Environment.SpecialFolder.Personal), "myDatabase.db");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.historyLayout);
            SetLabel();

            listView = FindViewById<ListView>(Resource.Id.myListView);
            mLoadingView = FindViewById(Resource.Id.loading_spinner);
            mShortAnimationDuration = Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);
            listView.Visibility = ViewStates.Gone;
            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<DataBaseItem>();
            WorkWithDataBase();
        }

        private void SetLabel()
        {
            Context context = this;
            Android.Content.Res.Resources res = context.Resources;
            string recordTable = this.Resources.GetString(Resource.String.historyText);
            this.Title = recordTable;
        }

        private async void WorkWithDataBase()
        {
            var list = await Task.Run(() => ReadFromDatabase(connection));

            listView.Adapter = new ListViewAdapter(this, list);
            crossFade(listView, mLoadingView);
        }

        private List<DataBaseItem> ReadFromDatabase(SQLiteConnection connection)
        {
            var table = connection.Table<DataBaseItem>();
            List<DataBaseItem> list = new List<DataBaseItem>();
            foreach (var item in table)
            {
                list.Add(item);
            }
            connection.Close();
            return list;
        }

        #region AnimMethods
        private void crossFade(View fadeIn, View fadeOut)
        {

            fadeIn.Alpha = 0f;
            fadeIn.Visibility = ViewStates.Visible;

            fadeIn.Animate()
                .Alpha(1f)
                .SetDuration(mShortAnimationDuration)
                .SetListener(null);

            fadeOut.Animate()
                .Alpha(0f)
                .SetDuration(mShortAnimationDuration)
                .SetListener(new MyAnimatorListenerAdapter(fadeOut, ViewStates.Gone));
        }
        #endregion

    }

}