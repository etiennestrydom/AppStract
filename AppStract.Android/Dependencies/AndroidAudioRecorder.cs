using System;
using Android;
using Android.Content.PM;
using Android.Media;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using AppStract.Dependencies;
using AppStract.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidAudioRecorder))]
namespace AppStract.Droid.Dependencies
{
    public class AndroidAudioRecorder : IAudioRecorder
    {
        public MediaRecorder Recorder { get; set; }
        public AndroidAudioRecorder()
        {
            Recorder = new MediaRecorder();
        }

        #region AudioRecorder implementation
        public void StartRecord()
        {
            //if (ContextCompat.CheckSelfPermission(, Manifest.Permission.RecordAudio) != Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.RecordAudio }, 1);
            //}

            Recorder.SetAudioSource(AudioSource.Mic);
            Recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            Recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            Recorder.SetOutputFile(AudioFilePath + "/" + AudioFileName + ".wav");
            Recorder.Prepare();
            Recorder.Start();
        }

        public void StopRecord()
        {
            Recorder.Stop();
            Recorder.Reset();
        }

        private string audioFileName;
        public string AudioFileName
        {
            get
            {
                return audioFileName;
            }
            set
            {
                audioFileName = value;
            }
        }

        private string audioFilePath;
        public string AudioFilePath
        {
            get
            {
                return audioFilePath;
            }
            set
            {
                audioFilePath = value;
            }
        }
        #endregion
    }
}
