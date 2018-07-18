using System;
using Android.Media;
using AppStract.Dependencies;
using AppStract.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidAudioPlayer))]
namespace AppStract.Droid.Dependencies
{
    public class AndroidAudioPlayer : IAudioPlayer
    {
        public MediaPlayer Player { get; set; }
        public AndroidAudioPlayer()
        {
            Player = new MediaPlayer();
        }

        #region AudioPlayer implementation

        public void PlayAudio()
        {
            Player.Completion += (sender, e) =>
            {
                Player.Reset();
            };
            Player.SetDataSource(AudioFilePath + "/" + AudioFileName + ".wav");
            Player.Prepare();
            Player.Start();
        }

        public void StopAudio()
        {
            Player.Stop();
        }

        public void PauseAudio()
        {
            Player.Pause();
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
