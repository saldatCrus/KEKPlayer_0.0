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
using KEKPlayer.Messages;
using KEKPlayer.Services;


namespace KEKPlayer.ViewModels
{
    class TrackOnAirPageViewModel : BindableBase
    {
        private readonly MessageBus _messageBus;

        public string CompostionName {get; set;}
        

        public TrackOnAirPageViewModel(MessageBus messageBus) 
        {
            _messageBus = messageBus;

            
            CompostionName = "Fuck yea !";

            _messageBus.Receive<TextMessage>(this, async CompostionTitle =>
            {
                CompostionName = (CompostionTitle.Text);
            });
           
            
        }

        public ICommand AddNewCompostion => new DelegateCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.ShowDialog();

        });
    }
}


//~TrackOnAirPageViewModel()
//{
//    Bass.Free();
//    Bass.PluginFree(0); разобраться в диструкторах и регионах

//    _timer.Stop();
//}