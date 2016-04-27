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
    class ViewHolder : Java.Lang.Object
    {
        public TextView cityName { get; set; }

        public TextView cityArea { get; set; }

        public TextView streetAddress { get; set; }

        public TextView dateTime { get; set; }

    }
}