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
using Android.Animation;

namespace ProjektZaliczeniowyXamarin.Models
{
    class MyAnimatorListenerAdapter : AnimatorListenerAdapter
    {
        private View view;
        ViewStates state;
        public MyAnimatorListenerAdapter(View view, ViewStates state)
        {
            this.view = view;
            this.state = state;
        }

        public override void OnAnimationEnd(Animator animation)
        {
            view.Visibility = state;
        }
    }
}