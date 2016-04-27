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

namespace ProjektZaliczeniowyXamarin.Activities
{
    [Activity(Label = "AboutActivity")]
    public class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //   string znaki = "ê æ ó ³ ñ";
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.aboutLayout);
            SetLabel();
        }

        private void SetLabel()
        {
            Context context = this;
            Android.Content.Res.Resources res = context.Resources;
            string recordTable = this.Resources.GetString(Resource.String.aboutText);
            this.Title = recordTable;
        }
    }
}