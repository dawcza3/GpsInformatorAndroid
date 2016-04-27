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

namespace ProjektZaliczeniowyXamarin.Models
{
    public class Singleton
    {
        private static Singleton myInfo;

        public static Singleton MyInfo
        {
            get
            {
                if (myInfo == null)
                {
                    myInfo = new Singleton();
                }
                return myInfo;
            }
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}