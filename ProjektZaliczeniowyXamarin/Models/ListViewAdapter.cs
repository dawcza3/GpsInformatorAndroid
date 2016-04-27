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
    public class ListViewAdapter : BaseAdapter<DataBaseItem>
    {
        #region Fields

        private List<DataBaseItem> items;
        private Activity context;

        #endregion

        #region Constructor

        public ListViewAdapter(Activity context, List<DataBaseItem> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        #endregion

        #region Methods [public]

        public override long GetItemId(int position)
        {
            return position;
        }

        public override DataBaseItem this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            ViewHolder viewHolder;

            if (view == null) // otherwise create a new one
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.view_row, null);

                viewHolder = new ViewHolder();
                viewHolder.cityName = view.FindViewById<TextView>(Resource.Id.CityName);
                viewHolder.streetAddress = view.FindViewById<TextView>(Resource.Id.StreetAddress);
                viewHolder.cityArea = view.FindViewById<TextView>(Resource.Id.CityArea);
                viewHolder.dateTime = view.FindViewById<TextView>(Resource.Id.DateTime);

                view.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)view.Tag;
            }

            var item = items[position];

            viewHolder.cityName.Text = item.cityName;
            viewHolder.cityArea.Text = item.cityArea;
            viewHolder.streetAddress.Text = item.streetAddress;
            viewHolder.dateTime.Text = item.dateTime;

            return view;
        }

        #endregion

        #region Methods [private]

        #endregion
    }
}