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

namespace ProjektZaliczeniowyXamarin.Models
{
    [Table("Items")]
    public class DataBaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(30)]
        public string cityName { get; set; }

        public string cityArea { get; set; }
        public string streetAddress { get; set; }
        public string dateTime { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}