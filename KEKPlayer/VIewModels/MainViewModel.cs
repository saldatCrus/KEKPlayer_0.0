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
using System.Text.RegularExpressions;
using System.Diagnostics;   

namespace KEKPlayer.ViewModels
{

    class MainViewModel : BindableBase
    {
        #region

        public Page MemberTrackPage { get; set; } 

        public string Status { get; set; }

        static object locker = new object();

        public string PlayerModeContent { get; set; }

        public bool StatusFlag = true;

        private float _MusicVolumeSlider=1;

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


        public enum ModePlayer
        {
            loopThisTrack, loopTrakList, StandartMode
        }

        private ModePlayer _modePlayer;

        public enum ShuffleMode 
        {
            Shuffle, No 
        }

        private ShuffleMode _shuffleMode;
        
        public string SelectedFile { get; set; }

        private BackgroundWorker _bgWorker = new BackgroundWorker();

        public double CurrentTrackLenght { get; set; }
        
        public string TimerOfCOmpostion{ get; set; }

        AudioControl audioControl = new AudioControl();

        private readonly MessageBus _messageBus;

        public AlbomModel CurrentlyPlayingTrack { get; set; }

        public AlbomModel CurrentlySelectedTrack { get; set; }

        public ObservableCollection<AlbomModel> AlbomModels { get; set; } = new ObservableCollection<AlbomModel>();

        public ObservableCollection<AlbomModel> PastMusicAlbomModel { get; set; } = new ObservableCollection<AlbomModel>();

        #endregion  

        public MainViewModel(NavigationService navigation, MessageBus messageBus) 
        {
            navigation.OnPageChanged += page => MemberTrackPage = page;
            navigation.Navigate(new TrackOnAirPage()); //Frame NAvigator    

            _messageBus = messageBus; //Massege bus

            _playbackState = PlaybackState.Stopped;

            MusicVolumeSlider = 1;

            Trace.WriteLine("+1 Поток");

             _bgWorker.DoWork += async(s, e) =>
            {
                while (true)
                {

                    if ((CurrentTrackPosition != audioControl.GetPositionInSeconds()) & (audioControl.GetPositionInSeconds() <= audioControl.GetLenghtInSeconds()))
                    {
                        CurrentTrackPosition = audioControl.GetPositionInSeconds();

                        TimerOfCOmpostion = $"{ audioControl.GetLenghtInSeconds():0} : {audioControl.GetPositionInSeconds():0}";
                                           
                        Thread.Sleep(1000);
                    }

                    if (AlbomModels.Count != 0 && audioControl.GetLenghtInSeconds() == audioControl.GetPositionInSeconds())
                    {

                        _playbackState = PlaybackState.Stopped;

                        audioControl.PlaybackStopType = AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;

                    }


                   

                    if (CurrentlyPlayingTrack != null)
                    {
                        
                        if (_playbackState == PlaybackState.Stopped)
                        {
                            audioControl.TakeAndPlayMetod(CurrentlyPlayingTrack.CompositionSource, MusicVolumeSlider);

                            await _messageBus.SendTo<TrackOnAirPageViewModel>(new ImageMessage(CurrentlyPlayingTrack.CompositionSource, CurrentlyPlayingTrack.Title));

                            audioControl.PlaybackStopType = AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                            audioControl.PlaybackPaused += _audioPlayer_PlaybackPaused;
                            audioControl.PlaybackResumed += _audioPlayer_PlaybackResumed;
                            audioControl.PlaybackStopped += _audioPlayer_PlaybackStopped;
                            CurrentTrackLenght = audioControl.GetLenghtInSeconds();
                            PastMusicAlbomModel.Add(CurrentlyPlayingTrack);                            
                            CurrentlyPlayingTrack = CurrentlySelectedTrack;                 
                                                 
                            Status = "<";

                            Trace.WriteLine("Произошла смена композиций");
                            if (PastMusicAlbomModel.Count > 2 & PastMusicAlbomModel[PastMusicAlbomModel.Count-1] != null)
                            { 
                                Trace.WriteLine("Предыдущий трек" + PastMusicAlbomModel[PastMusicAlbomModel.Count-2].Title); 
                            }
                           

                            //_playbackState = PlaybackState.Playing;

                            Thread.Sleep(1000);
                        }

                        if (CurrentlySelectedTrack == CurrentlyPlayingTrack )
                        {
                            //audioControl.TogglePlayPause(_MusicVolumeSlider);

                            //CurrentTrackLenght = audioControl.GetLenghtInSeconds();
                            Trace.WriteLine("Произошла установка 0 вермени");
                            _playbackState = PlaybackState.Playing;
                            audioControl.TogglePlayPause(_MusicVolumeSlider);



                            if (_playbackState == PlaybackState.Stopped)
                            {
                                Trace.WriteLine("Произошла установка 0 вермени");
                                _playbackState = PlaybackState.Playing;
                                audioControl.TogglePlayPause(_MusicVolumeSlider);
                            }

                        }
                    }
                    
                    if (AlbomModels.Count != 0 & CurrentlyPlayingTrack == null) // Problem
                    {

                        CurrentlyPlayingTrack = AlbomModels[0];

                        Thread.Sleep(1000);
                    }

                }

            }; //Body of Backgroun Worker

            _bgWorker.RunWorkerAsync(); //Run Background AsynWork

        }
        
        public ICommand ChangeStatusMusikOnAir => new DelegateCommand(() => 
        {          

            if (CurrentlyPlayingTrack != null)
            {
                if (_playbackState == PlaybackState.Paused)
                {
                    Status = "<";

                    _playbackState = PlaybackState.Playing;
                }
                else
                {
                    Status = "#";
                    _playbackState = PlaybackState.Paused;
                }

                // AsyncImageBass(CurrentlyPlayingTrack.CompositionSource);
                audioControl.TogglePlayPause(_MusicVolumeSlider);   
            }
        }); //PausePlayCommand

        private void _audioPlayer_PlaybackStopped()
        {
            _playbackState = PlaybackState.Stopped;
            CommandManager.InvalidateRequerySuggested();
            audioControl.SetPosition(0);

            if (audioControl.PlaybackStopType == AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile)
            {
                CurrentlyPlayingTrack = NextItem(AlbomModels, CurrentlyPlayingTrack);
                _playbackState = PlaybackState.Playing;

            }

        }

        private AlbomModel NextItem(ObservableCollection<AlbomModel> albomModel, AlbomModel currentItem) 
        {
            Trace.WriteLine("Произошла установка следуйщего трека");
            var currentIndex = albomModel.IndexOf(currentItem);
            if (currentIndex < albomModel.Count - 1)
            {
                return albomModel[currentIndex + 1];
            }

            return albomModel[0];
        }

        //private AlbomModel PastItem(ObservableCollection<AlbomModel> albomModel, AlbomModel currentItem) 
        //{
        //    var currentIndex = albomModel.IndexOf(currentItem);
        //    if (currentIndex > albomModel.Count - 1)
        //    {
        //        return albomModel[currentIndex - 1];
        //    }
        //    return albomModel[0];
        //}

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
            lock (locker)
            {
                if (audioControl != null)
                {
                    audioControl.Dispose();

                    audioControl.PlaybackStopType = AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;

                    _playbackState = PlaybackState.Stopped;
                }
            }
            
        });

        public ICommand PastMusicOAir => new DelegateCommand(() =>
        {
            lock (locker)
            {
                if (audioControl.GetPositionInSeconds() < (audioControl.GetLenghtInSeconds() / 4))
                {
                    if (PastMusicAlbomModel.Count > 2 & PastMusicAlbomModel[PastMusicAlbomModel.Count-1] != null)
                    {
                        
                        
                    }
                    else
                    {
                        if (AlbomModels[0] != null)
                        {
                           

                            audioControl.Dispose();

                            audioControl.PlaybackStopType = AudioControl.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;

                            _playbackState = PlaybackState.Stopped;
                        }


                    }
                   
                }
                else
                {
                    CurrentTrackPosition = 0;
                }
            }
        });

        public ICommand OpenFileDialog => new DelegateCommand(() =>
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Audio files (*.wav, *.mp3, *.wma, *.ogg, *.flac) | *.wav; *.mp3; *.wma; *.ogg; *.flac";
            ofd.Multiselect = true;
            var result = ofd.ShowDialog();
            if (result == true)
            {
                foreach (string  FileName in ofd.FileNames)
                {
                    foreach(string SafeFileName in ofd.SafeFileNames)
                    {
                        if (FileName.Contains(SafeFileName))
                        {
                            Trace.WriteLine("Была добавлена композиция"+ SafeFileName);
                            AlbomModels.Add(new AlbomModel
                            {
                                Title = SafeFileName,
                                CompositionSource = FileName,
                                CompostionName = FileName,

                            });

                        }
                            

                    }
                    
                }
            }


        }); // Open File Dialog Metod

        public ICommand PlayerMode => new DelegateCommand(() => 
        {
            if(_modePlayer == ModePlayer.StandartMode) 
            {
                _modePlayer = ModePlayer.loopTrakList;

                PlayerModeContent = "loopThisTrack";
                                
            }
            else if (_modePlayer == ModePlayer.loopThisTrack) 
            {
                _modePlayer = ModePlayer.loopTrakList;

                PlayerModeContent = "loopTrakList";
            }
            else if (_modePlayer == ModePlayer.loopTrakList) 
            {
                _modePlayer = ModePlayer.StandartMode;

                PlayerModeContent = "StandartMode";
            }

        });

        public async void AsyncImageBass(string Source, string Name) // Send Patch of File
        {
            await _messageBus.SendTo<TrackOnAirPageViewModel>(new ImageMessage(Source, Name));
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
