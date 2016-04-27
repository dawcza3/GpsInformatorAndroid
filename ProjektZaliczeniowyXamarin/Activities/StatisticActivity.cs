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
using SQLite;
using System.IO;
using ProjektZaliczeniowyXamarin.Models;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyXamarin.Activities
{
    [Activity(Label = "StatisticActivity")]
    public class StatisticActivity : Activity
    {
        View mLoadingView;
        TextView textView;
        SQLiteConnection connection;
        int mShortAnimationDuration;
        private string dbPath = Path.Combine(System.Environment.GetFolderPath(
        System.Environment.SpecialFolder.Personal), "myDatabase.db");
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.statisticsLayout);
            SetLabel();
            textView = FindViewById<TextView>(Resource.Id.myTextView);
            mLoadingView = FindViewById(Resource.Id.loading_spinner);
            mShortAnimationDuration = Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);
            textView.Visibility = ViewStates.Gone;
            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<DataBaseItem>();
            MakeData();
        }

        private void SetLabel()
        {
            Context context = this;
            Android.Content.Res.Resources res = context.Resources;
            string recordTable = this.Resources.GetString(Resource.String.statisticsText);
            this.Title = recordTable;
        }

        private void CountAllVisitedArea()
        {
            var table = connection.Table<DataBaseItem>();
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (var item in table)
            {
                if (dictionary.ContainsKey(item.cityArea))
                {
                    dictionary[item.cityArea]++;
                }
                if (!dictionary.ContainsKey(item.cityArea))
                {
                    dictionary.Add(item.cityArea, 1);
                }
            }
            textView.Text += Resources.GetString(Resource.String.TotalArea);
            foreach (var item in dictionary)
            {
                textView.Text += item.Key + ":\t" + item.Value + "\n";
            }
        }

        private void MakeData()
        {
            CountAllVisitedArea();
            CalculateTotalDiscoveredDistance();
            crossFade(textView, mLoadingView);
        }

        private void CalculateTotalDiscoveredDistance()
        {
            double totalDistance = 0;
            var table = connection.Table<DataBaseItem>().ToList();
            for (int i = 0; i < table.Count() - 1; i++)
            {
                totalDistance += GetDistanceFromLatLonInKm(table[i].Latitude, table[i].Longitude,
                    table[i + 1].Latitude, table[i + 1].Longitude);
            }
            textView.Text += Resources.GetString(Resource.String.DistInfo);
            textView.Text += String.Format("{0:N2} km", totalDistance);
        }

        private double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            /*double R = 6371; // Radius of the earth in km
            double dLat = Geg2rad(lat2 - lat1);  // deg2rad below
            double dLon = Geg2rad(lon2 - lon1);
            double a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Geg2rad(lat1)) * Math.Cos(Geg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d; */


            float[] tab = new float[1];
            Android.Locations.Location.DistanceBetween(lat1, lon1, lat2, lon2,tab);
            return tab[0] / 1000; 
            
            /*
            double earthRadius = 6371; // kilometers
            double dLat = Geg2rad(lat2 - lat1);
            double dLng = Geg2rad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(Geg2rad(lat1)) * Math.Cos(Geg2rad(lat2)) * Math.Sin(dLng / 2)
                    * Math.Sin(dLng / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            float dist = (float)(earthRadius * c);

            return dist;
            */
        }

        private double Geg2rad(double deg)
        {
            return deg * (Math.PI / 180);
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