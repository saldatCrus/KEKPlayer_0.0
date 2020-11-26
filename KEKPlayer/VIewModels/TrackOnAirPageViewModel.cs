using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Mvvm;
using KEKPlayer.Models;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Timers;
using System.IO;
using KEKPlayer.AudioControls;


namespace KEKPlayer.ViewModels
{
    class TrackOnAirPageViewModel : BindableBase
    {
        
               
        public TrackOnAirPageViewModel() 
        {
            

        }

    }
}


//~TrackOnAirPageViewModel()
//{
//    Bass.Free();
//    Bass.PluginFree(0); разобраться в диструкторах и регионах

//    _timer.Stop();
//}