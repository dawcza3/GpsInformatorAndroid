using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;

namespace ProjektZaliczeniowyXamarin.Activities
{

    [Activity(Label = "Gps Informator", MainLauncher = true, Icon = "@drawable/icon")]
    public class BeginActivity : Activity, ILocationListener
    {
        #region Fields
        Button FindButton, StatisticsButton, AboutButton, HistoryButton;
        double latitude = 0;
        double longitude = 0;

        Location _currentLocation;
        LocationManager _locationManager;
        String _locationProvider;
        #endregion
        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetLabel();
            SetContentView(Resource.Layout.beginLayout);
            InitializeLocationManager();

            FindButton = FindViewById<Button>(Resource.Id.findButton);
            StatisticsButton = FindViewById<Button>(Resource.Id.statisticsButton);
            HistoryButton = FindViewById<Button>(Resource.Id.historyButton);
            AboutButton = FindViewById<Button>(Resource.Id.aboutButton);

            FindButton.Click += (s, e) =>
            {
                if (longitude == 0 || latitude == 0)
                    Toast.MakeText(this, Resources.GetString(Resource.String.WaitForGpsInfo), ToastLength.Long).Show();
                else
                {
                    Models.Singleton.MyInfo.Latitude = latitude;
                    Models.Singleton.MyInfo.Longitude = longitude;
                    StartActivity(typeof(FindActivity));
                    OverridePendingTransition(Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);
                }
            };
            StatisticsButton.Click += (s, e) =>
            {
                StartActivity(typeof(StatisticActivity));
                OverridePendingTransition(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);
            };

            HistoryButton.Click += (s, e) =>
            {
                StartActivity(typeof(HistoryActivity));
                OverridePendingTransition(Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);
            };
            AboutButton.Click += (s, e) =>
            {
                StartActivity(typeof(AboutActivity));
                OverridePendingTransition(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);
            };

        }

        private void SetLabel()
        {
            Context context = this;
            Android.Content.Res.Resources res = context.Resources;
            string recordTable = this.Resources.GetString(Resource.String.beginText);
            this.Title = recordTable;
        }

        #region ILocationListenerMethods
        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation != null)
            {
                latitude = _currentLocation.Latitude;
                longitude = _currentLocation.Longitude;
            }
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        void InitializeLocationManager()
        {
            // location manager nasluchuje updatow z gpsu i informuje apke przez eventy

            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = String.Empty;
            }
        }

        #endregion

    }
}