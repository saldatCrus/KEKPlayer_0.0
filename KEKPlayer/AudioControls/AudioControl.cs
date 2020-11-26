using System;
using System.Collections.Generic;
using System.Text;
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

namespace KEKPlayer.AudioControls
{
    class AudioControl
    {
        private NAudio.Wave.BlockAlignReductionStream stream = null;

        private NAudio.Wave.DirectSoundOut output = null;

        public void TakeAndPlayMetod(string Source) 
        {
            OpenFileDialog Dialog = new OpenFileDialog();

            DisposeAudio();

            Dialog.FileName = Source;
            
            if (Dialog.FileName.EndsWith(".mp3"))
            {
                NAudio.Wave.WaveStream PcmData = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(Dialog.FileName));
                stream = new NAudio.Wave.BlockAlignReductionStream(PcmData);
            }
            else
            if (Dialog.FileName.EndsWith(".wav"))
            {
                NAudio.Wave.WaveStream PcmData = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(Dialog.FileName));

                stream = new NAudio.Wave.BlockAlignReductionStream(PcmData);
            }
            else throw new InvalidOperationException("Not a correct audio file type.");

            output = new NAudio.Wave.DirectSoundOut();

            output.Init(stream);

            output.Play();



        }

        public void PausePlayMetod()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Pause();

                else if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused) output.Play();

            }
        }

        public void DisposeAudio()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        

    }

    
}
