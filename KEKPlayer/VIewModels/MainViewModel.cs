using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using DevExpress.Mvvm;
using System.Windows.Controls;
using KEKPlayer.Services;
using KEKPlayer.Pages;
using System.Windows.Input;
using KEKPlayer.AudioControls;
using KEKPlayer.Models;
using System.Drawing;
using KEKPlayer.Messages;
using System.IO;
using System;
using System.ComponentModel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading;

namespace KEKPlayer.ViewModels
{

    class MainViewModel : BindableBase
    {
        public AlbomModel CurrentlySelectedTrack{get;set;}

        public Page MemberTrackPage { get; set; } //

        public string Status { get; set; }

        public bool StatusFlag = true;

        private float _MusicVolumeSlider;

        public float MusicVolumeSlider
        {
            get { return audioControl.GetVolume(); }

            set
            {
                if (value.Equals(_MusicVolumeSlider)) return;
                _MusicVolumeSlider = value;
                audioControl.SetVolume(_MusicVolumeSlider);

            }
        } //Volume slider

        private double _currentTrackPosition;

        public double CurrentTrackPosition
        {
            get
            {
                return _currentTrackPosition ;
            }
            set
            {
                if (value.Equals(_currentTrackPosition)) return;
                
                _currentTrackPosition = value;          

                audioControl.SetPosition(_currentTrackPosition);

            }
        } // Track postion 

        private enum PlaybackState
        {
            Playing, Stopped, Paused
        }

        private PlaybackState _playbackState;

        private BackgroundWorker _bgWorker = new BackgroundWorker();
        private BackgroundWorker _bgPlayer = new BackgroundWorker();

        public double CurrentTrackLenght { get; set; }
        
        public string TimerOfCOmpostion{ get; set; }

        AudioControl audioControl = new AudioControl();

        private readonly MessageBus _messageBus;

        public AlbomModel CurrentlyPlayingTrack { get; set; }

        string DefaultCompostionImage = @"default.png"; //Defult compostion png file

        public ObservableCollection<AlbomModel> AlbomModels { get; set; } = new ObservableCollection<AlbomModel>();

        public MainViewModel(NavigationService navigation, MessageBus messageBus) 
        {
            navigation.OnPageChanged += page => MemberTrackPage = page;
            navigation.Navigate(new TrackOnAirPage()); //Frame NAvigator

            //AlbomArtCompostiononAir = SetAlbumArt(Source);
            //audioControl.TakeAndPlayMetod(Source,100);

            Status = "<";

            _messageBus = messageBus; //Massege bus

            _playbackState = PlaybackState.Stopped;



            _bgWorker.DoWork += (s, e) =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    if ((CurrentTrackPosition != audioControl.GetPositionInSeconds()) & (audioControl.GetPositionInSeconds() <= audioControl.GetLenghtInSeconds()))
                    {
                        CurrentTrackPosition = audioControl.GetPositionInSeconds();

                        TimerOfCOmpostion = $"{ audioControl.GetLenghtInSeconds():0} : {audioControl.GetPositionInSeconds():0}";

                        if(CurrentTrackPosition == CurrentTrackLenght) 
                        {
                            Thread.Sleep(500);
                            _playbackState = PlaybackState.Stopped;
                            // audioControl.PlaybackStopped += _audioPlayer_PlaybackStopped;
                            CurrentlySelectedTrack = NextItem(AlbomModels, CurrentlyPlayingTrack);
                            //_playbackState = PlaybackState.Playing;

                        }
                    }

                    if (CurrentlyPlayingTrack != null)
                    {
                        if (_playbackState == PlaybackState.Stopped)
                        {
                            audioControl.TakeAndPlayMetod(CurrentlyPlayingTrack.CompositionSource, MusicVolumeSlider);
                            audioControl.PlaybackStopType = AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                            audioControl.PlaybackPaused += _audioPlayer_PlaybackPaused;
                            audioControl.PlaybackResumed += _audioPlayer_PlaybackResumed;
                            audioControl.PlaybackStopped += _audioPlayer_PlaybackStopped;
                            CurrentTrackLenght = audioControl.GetLenghtInSeconds();
                            CurrentlyPlayingTrack = CurrentlySelectedTrack;
                        }
                        if (CurrentlySelectedTrack == CurrentlyPlayingTrack)
                        {
                            audioControl.TogglePlayPause(_MusicVolumeSlider);
                        }
                    }


                }

            }; //Body of Backgroun Worker

       

            //Body of Backgroun Worker


            _bgWorker.RunWorkerAsync(); //Run Background AsynWork

        }



        public ICommand ChangeStatusMusikOnAir => new DelegateCommand(() => 
        {          

            if (CurrentlyPlayingTrack != null)
            {
                if (StatusFlag == false)
                {
                    Status = "<";
                    StatusFlag = true;
                }
                else
                {
                    Status = "#";
                    StatusFlag = false;
                }

                audioControl.TogglePlayPause(1);
            }

            if (CurrentlyPlayingTrack == null)
            {
                CurrentlyPlayingTrack = AlbomModels[0];
            }

        }); //PausePlayCommand

        private void _audioPlayer_PlaybackStopped()
        {
            _playbackState = PlaybackState.Stopped;
            CommandManager.InvalidateRequerySuggested();
            CurrentTrackPosition = 0;

            if (audioControl.PlaybackStopType == AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile)
            {
                CurrentlyPlayingTrack = NextItem(AlbomModels, CurrentlyPlayingTrack); 
                _playbackState = PlaybackState.Playing;
            }
        }

        private AlbomModel NextItem(ObservableCollection<AlbomModel> albomModel, AlbomModel currentItem) 
        {
            var currentIndex = albomModel.IndexOf(currentItem);
            if (currentIndex < albomModel.Count - 1)
            {
                return albomModel[currentIndex + 1];
            }
            return albomModel[0];
        }

        private void _audioPlayer_PlaybackResumed()
        {
            _playbackState = PlaybackState.Playing;
            
        }

        private void _audioPlayer_PlaybackPaused()
        {
            _playbackState = PlaybackState.Paused;
            
        }

        public ICommand NextMusicOAir => new DelegateCommand(() =>
        {
            


        });

        public ICommand PastMusicOAir => new DelegateCommand(() =>
        {
            
        });

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

        public ICommand OpenFileDialog => new DelegateCommand(() =>
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Audio files (*.wav, *.mp3, *.wma, *.ogg, *.flac) | *.wav; *.mp3; *.wma; *.ogg; *.flac";
            var result = ofd.ShowDialog();
            if (result == true)
            {
                AlbomModels.Add(new AlbomModel
                {
                    Title = ofd.SafeFileName,
                    CompositionSource = ofd.FileName,
                    CompostionName = ofd.FileName,

                });
            }

        });

        public async void AsyncMessageBass(string Source) // Send Patch of File
        {
            await _messageBus.SendTo<TrackOnAirPageViewModel>(new TextMessage(Source));
        }

        public async void AsyncImageMessageBass(Bitmap AlbomArtCompostiononAir) //Send Image Source
        {
            await _messageBus.SendTo<TrackOnAirPageViewModel>(new ImageMessage(AlbomArtCompostiononAir));
        }

        private void RewindToStart()
        {
            audioControl.SetPosition(0); // set position to zero
        }
        private bool CanRewindToStart()
        {
            if (_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }
    }
}
