using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DevExpress.Mvvm;

namespace KEKPlayer.Models
{
    class AlbomModel : BindableBase
    {

        public string Title { get; set; }

        // public int CompostionCount { get; set; }

        public string CompostionName { get; set; }

        public string CompositionSource { get; set; }

        


        //public ObservableCollection<Composition> Compositions { get; set; } = new ObservableCollection<Composition>();


    }

    public class Composition
    {
        //public string CompostionName { get; set; }

        //public  string CompositionSource { get; set; }


    }

    

}
