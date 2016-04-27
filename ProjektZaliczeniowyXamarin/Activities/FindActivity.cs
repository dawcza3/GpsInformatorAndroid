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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SQLite;

namespace ProjektZaliczeniowyXamarin.Activities
{
  
    [Activity(Label = "FindActivity",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class FindActivity : Activity
    {
        #region Fields
        View mLoadingView;
        TextView myTextView;

        SQLiteConnection connection;
        private string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDatabase.db");

        string cityRegion;
        string cityName;
        string streetAddress;
        string fullName;
        string currentDateTime;

        int mShortAnimationDuration;

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.findLayout);
            SetLabel();
            myTextView = FindViewById<TextView>(Resource.Id.myTextContent);
            mLoadingView = FindViewById(Resource.Id.loading_spinner);
            mShortAnimationDuration = Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);
            myTextView.Visibility = ViewStates.Gone;
            myTextView.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<DataBaseItem>();
            CreateRequest(Models.Singleton.MyInfo.Latitude, Models.Singleton.MyInfo.Longitude);
            FindInformationAboutPlace();
            this.Title = fullName;
        }

        #region OtherMethods
        private void SetLabel()
        {
            Context context = this;
            Android.Content.Res.Resources res = context.Resources;
            string recordTable = this.Resources.GetString(Resource.String.findText);

            this.Title = recordTable;
        }
        public static string GetTimeDate()
        {
            string DateTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            return DateTime;
        }
        #endregion

        #region GpsMethods
        private async void FindInformationAboutPlace()
        {
            //string cityName = "Torun";
            var webclient = new WebClient();
            string toDownload = "http://pl.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&titles=" + cityName + "&redirects=true";
            string pageSourceCode = await webclient.DownloadStringTaskAsync(toDownload);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pageSourceCode);

            var fnode = doc.GetElementsByTagName("extract")[0];

            try
            {
                string ss = fnode.InnerText;
                Regex regex = new Regex("\\<[^\\>]*\\>");
                string.Format("Beforce:{0}", ss);
                ss = regex.Replace(ss, string.Empty);
                string result = String.Format(ss);
                myTextView.Text = result;
            }
            catch (Exception ex)
            {
                myTextView.Text += "\nerror\n";
            }

            crossFade(myTextView, mLoadingView);
        }

        private void CreateRequest(double Latitude, double Longitude)
        {
            string latitude = Latitude.ToString().Replace(",", ".");
            string longitude = Longitude.ToString().Replace(",", ".");
            string UrlRequest = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude;
            string number = "", route = "";
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(UrlRequest);
                GoogleGeoCodeResponse test = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(json);
                foreach (var info in test.results[0].address_components)
                {
                    if (info.types[0] == "street_number")
                        number = info.long_name;
                    else if (info.types[0] == "route")
                        route = info.long_name;
                    else if (info.types[0] == "administrative_area_level_1")
                        cityRegion = info.long_name;
                    else if (info.types[0] == "locality")
                        cityName = info.long_name;
                }
                streetAddress = route + " " + number;
                currentDateTime = GetTimeDate();
                fullName = test.results[0].formatted_address;
            }
            UpdateDataBase(Latitude, Longitude);

        }

        private void UpdateDataBase(double lat, double lon)
        {
            var item = new DataBaseItem()
            {
                cityName = this.cityName,
                cityArea = this.cityRegion,
                streetAddress = this.streetAddress,
                Latitude = lat,
                Longitude = lon,
                dateTime = currentDateTime
            };
            connection.Insert(item);
            connection.Close();
        }
        #endregion

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