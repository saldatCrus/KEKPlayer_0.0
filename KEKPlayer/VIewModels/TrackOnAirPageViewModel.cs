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
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows.Controls;
using KEKPlayer.Pages;
using System.Threading;
using System.Windows.Interop;

namespace KEKPlayer.ViewModels
{
    class TrackOnAirPageViewModel : BindableBase
    {
        private readonly MessageBus _messageBus;

        public string CompostionName { get; set; }

        public BitmapSource ImageSource { get; set; }

        string DefaultCompostionImage = @"default.png"; //Defult compostion png file

        delegate void Update_Image_CallBack(int state);

        public TrackOnAirPageViewModel(MessageBus messageBus) 
        {

            _messageBus = messageBus;


            _messageBus.Receive<ImageMessage>(this, async AlbomArtCompostiononAir =>
            {
                //ImageSource = BitmapToImageSource(SetAlbumArt(AlbomArtCompostiononAir.ImageSource));
                App.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Background,
                    new Action(() => this.ImageSource = BitmapToImageSource(SetAlbumArt(AlbomArtCompostiononAir.ImageSource))));
                

                CompostionName = AlbomArtCompostiononAir.ImageName;
            });       

           

        }

        public BitmapSource BitmapToImageSource(Bitmap source)
        {
            BitmapSource ConvertedImage;


            ConvertedImage = Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
            return ConvertedImage;

        } // FUking err converter 

        public async void AsyncMessageBass(string Source)
        {
            await _messageBus.SendTo<MainViewModel>(new TextMessage(Source));
        }

        public Bitmap SetAlbumArt(string Source) // Get art of Compostion
        {

            TagLib.File file = TagLib.File.Create(Source);
            var mStream = new MemoryStream();
            var firstPicture = file.Tag.Pictures.FirstOrDefault();
            if (firstPicture != null)
            {
                byte[] pData = firstPicture.Data.Data;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                var bm = new Bitmap(mStream, true);
                mStream.Dispose();
                return bm;
            }
            else
            {
                Bitmap bitmap = (Bitmap)Bitmap.FromFile(DefaultCompostionImage, true);
                return bitmap;
            }

           

        }
        /// <summary>
        /// Метод для конвертаций битмап в BitmapImage
        /// </summary>
        //public BitmapImage BitmapToImageSource(Bitmap bitmap)
        //{
        //    using (MemoryStream memory = new MemoryStream())
        //    {

        //        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
        //        memory.Position = 0;
        //        BitmapImage bitmapimage = new BitmapImage();
        //        bitmapimage.BeginInit();
        //        bitmapimage.StreamSource = memory;
        //        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapimage.EndInit();
        //        return bitmapimage;

        //    }
        //}
    }
}
