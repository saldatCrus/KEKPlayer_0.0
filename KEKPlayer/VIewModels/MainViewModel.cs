using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using DevExpress.Mvvm;
using System.Windows.Controls;
using KEKPlayer.Services;
using KEKPlayer.Pages;
using KEKPlayer.ViewModels;
using DevExpress.Mvvm.POCO;
using System.Windows.Input;
using NAudio.MediaFoundation;
using NAudio.Wave.SampleProviders;
using KEKPlayer.AudioControls;
using KEKPlayer.Models;
using Microsoft.Win32;
using System.Drawing.Printing;
using System.Drawing;
using TagLib;
using System.IO;
using System;

namespace KEKPlayer.ViewModels
{
    class MainViewModel : BindableBase
    {
        public Page MemberTrackPage { get; set; }

        public string Status { get; set; }

        public bool StatusFlag = true;

        public CompostionModel compostion = new CompostionModel();

        public string ImageSource { get; set; }

        public string CompostionName { get; set; }

        public string CompositionSource { get; set; }

        AudioControl audioControl = new AudioControl();

        public System.Drawing.Image AlbomArtCompostiononAir;

        public string Source = "MP3Test.mp3";

        public MainViewModel(NavigationService navigation) 
        {
            navigation.OnPageChanged += page => MemberTrackPage = page;
            navigation.Navigate(new TrackOnAirPage());
            

            AlbomArtCompostiononAir = SetAlbumArt(Source);
            audioControl.TakeAndPlayMetod(Source);


            Status = "<";

        }

      

        public ICommand ChangeStatusMusikOnAir => new DelegateCommand(() =>
        {
            if(StatusFlag == false) 
            {
                Status = "<";
                StatusFlag = true;
            }
            else
            {
                Status = "#";
                StatusFlag = false;
            }

            audioControl.PausePlayMetod();
        });



        public ICommand NextMusicOAir => new DelegateCommand(() =>
        {

        });

        public ICommand PastMusicOAir => new DelegateCommand(() =>
        {

        });

        public System.Drawing.Image SetAlbumArt(string Source)
        {
           
            TagLib.File file = TagLib.File.Create(Source);
            var mStream = new MemoryStream();
            var firstPicture = file.Tag.Pictures.FirstOrDefault();
            if (firstPicture != null)
            {
                byte[] pData = firstPicture.Data.Data;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                var bm = new Bitmap(mStream, false);
                mStream.Dispose();
                return bm;
            }
            else
            {
                return default;
            }

        }

    }
}
