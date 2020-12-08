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
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;

namespace KEKPlayer.ViewModels
{
    class TrackOnAirPageViewModel : BindableBase
    {
        private readonly MessageBus _messageBus;

        public string CompostionName {get; set;}

        public BitmapSource ImageSource { get; set; }

        string DefaultCompostionImage = @"default.png"; //Defult compostion png file

        public TrackOnAirPageViewModel(MessageBus messageBus) 
        {
            _messageBus = messageBus;

            Bitmap bitmap = (Bitmap)Bitmap.FromFile(DefaultCompostionImage, true);


            _messageBus.Receive<TextMessage>(this, async CompostionTitle =>
            {
                CompostionName = (CompostionTitle.Text);
            });

            _messageBus.Receive<ImageMessage>(this, async AlbomArtCompostiononAir =>
            {
                ImageSource = BitmapConversion.BitmapToBitmapSource(AlbomArtCompostiononAir.CompostionImage);
            });       

           

        }
        public static class BitmapConversion
        {
            public static BitmapSource BitmapToBitmapSource(Bitmap source)
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                              source.GetHbitmap(),
                              IntPtr.Zero,
                              Int32Rect.Empty,
                              BitmapSizeOptions.FromEmptyOptions());
            }
        }

        public async void AsyncMessageBass(string Source)
        {
            await _messageBus.SendTo<MainViewModel>(new TextMessage(Source));
        }


    }
}
