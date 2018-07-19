using System;
using System.Threading.Tasks;
using Prism.Commands;
using System.Diagnostics;
using VoiceIt2API;
using AppStract.Dependencies;
using System.IO;
using Xamarin.Essentials;
using Prism.Navigation;
using Newtonsoft.Json;

namespace AppStract.ViewModels
{
    public class Authentication
    {
        public string Message { get; set; }
        public int Status { get; set; }
    }

    public class DashboardPageViewModel : ViewModelBase
    {
        private IAudioRecorder _audioRecorder;
        private IAudioPlayer _audioPlayer;
        private bool _isRecording;
        private VoiceIt2 _myVoiceIt;

        public string RecordVoiceText { get; set; }
        public string SpeechIcon { get; set; }

        private DelegateCommand _recordVoiceCommand;
        public DelegateCommand RecordVoiceCommand =>
        _recordVoiceCommand ?? (_recordVoiceCommand = new DelegateCommand(() =>
        {
            RecordAudio();
        }));

        public DashboardPageViewModel(INavigationService navigationService, IAudioRecorder audioRecorder, IAudioPlayer audioPlayer) : base(navigationService)
        {
            _audioRecorder = audioRecorder;
            _audioPlayer = audioPlayer;
            _myVoiceIt = new VoiceIt2("key_140b7c835a984de2a4921ea57d128245", "tok_c3d96357247149479e5b13375c13e124");

            SpeechIcon = "speech_icon";
            RecordVoiceText = "Start Recording";
        }


        private void RecordAudio()
        {
            if (!_isRecording)
            {
                var cacheDir = FileSystem.CacheDirectory;
                string audioFileName = Guid.NewGuid().ToString();
                _audioRecorder.AudioFilePath = cacheDir;
                _audioPlayer.AudioFilePath = cacheDir;
                _audioRecorder.AudioFileName = audioFileName;
                _audioPlayer.AudioFileName = audioFileName;
                _audioRecorder.StartRecord();
                RecordVoiceText = "Recording.......";
                SpeechIcon = "speech_recording_Icon";
                _isRecording = true;

            }
            else
            {
                SpeechIcon = "speech_icon";
                RecordVoiceText = "Start Recording";
                _audioRecorder.StopRecord();
                _isRecording = false;
            }
        }

       
    }
}
