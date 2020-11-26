using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace KEKPlayer.Models
{
    public class CompostionModel : BindableBase
    {
        public string CompostionName { get; set; }

        public DateTime Time { get; set; }

        public string ImageSource { get; set; }

        public string CompositionSource { get; set; }    

    }
}
