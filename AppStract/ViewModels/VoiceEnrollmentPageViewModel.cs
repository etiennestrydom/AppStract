using System;
using System.Threading.Tasks;
using Prism.Commands;
using System.Diagnostics;
using VoiceIt2API;
using AppStract.Dependencies;
using System.IO;
using Xamarin.Essentials;
using Prism.Navigation;

namespace AppStract.ViewModels
{
    public class VoiceEnrollmentPageViewModel : ViewModelBase
    {
        private IAudioRecorder _audioRecorder;
        private IAudioPlayer _audioPlayer;
        private bool _isRecording;
        private VoiceIt2 _myVoiceIt;

        public string RecordVoiceText { get; set; }

        private DelegateCommand _recordVoiceCommand;
        public DelegateCommand RecordVoiceCommand =>
        _recordVoiceCommand ?? (_recordVoiceCommand = new DelegateCommand(() =>
        {
            RecordAudio();
        }));

        private DelegateCommand _playAudioCommand;
        public DelegateCommand PlayAudioCommand =>
        _playAudioCommand ?? (_playAudioCommand = new DelegateCommand(() =>
        {
            PlayAudio();
        }));

        private DelegateCommand _stopAudioCommand;
        public DelegateCommand StopAudioCommand =>
        _stopAudioCommand ?? (_stopAudioCommand = new DelegateCommand(() =>
        {
            _audioPlayer.StopAudio();
        }));

        private DelegateCommand _sendVoiceRecording;
        public DelegateCommand SendVoiceRecording =>
        _sendVoiceRecording ?? (_sendVoiceRecording = new DelegateCommand(() =>
        {
            SendAudioForEnrollmentAudio();
        }));

        private DelegateCommand _verifyUsersVoiceCommand;
        public DelegateCommand VerifyUsersVoiceCommand =>
        _verifyUsersVoiceCommand ?? (_verifyUsersVoiceCommand = new DelegateCommand(() =>
        {
            VerifyUsersVoice();
        }));

        public VoiceEnrollmentPageViewModel(INavigationService navigationService, IAudioRecorder audioRecorder, IAudioPlayer audioPlayer) : base(navigationService)
        {
            _audioRecorder = audioRecorder;
            _audioPlayer = audioPlayer;
            _myVoiceIt = new VoiceIt2("key_140b7c835a984de2a4921ea57d128245", "tok_c3d96357247149479e5b13375c13e124");

            RecordVoiceText = "Start Recording";
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            await TextToSpeech.SpeakAsync("Please say the passphrase");

            RecordAudio();
        }

        private void VerifyUsersVoice()
        {
            var result = _myVoiceIt.VoiceVerification("usr_9bbb34d6228b45e7b62521b18708a66a", "en-US", _audioRecorder.AudioFilePath + "/" + _audioRecorder.AudioFileName + ".wav");
        }

        private void SendAudioForEnrollmentAudio()
        {
            var result = _myVoiceIt.CreateVoiceEnrollment("usr_9bbb34d6228b45e7b62521b18708a66a", "en-US", _audioRecorder.AudioFilePath + "/" + _audioRecorder.AudioFileName + ".wav");
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
                _isRecording = true;
            }
            else
            {
                RecordVoiceText = "Start Recording";
                _audioRecorder.StopRecord();
                _isRecording = false;
            }
        }

        void PlayAudio()
        {
            if (File.Exists(_audioRecorder.AudioFilePath + "/" + _audioRecorder.AudioFileName + ".wav"))
            {
                _audioPlayer.PlayAudio();
            }
            else
            {
                //DisplayAlert("Whooops!", "Record something first", "Ok");
                Debug.WriteLine("Whooops! Record something first");
            }
        }

        //private async void Recorder_AudioInputReceived(object sender, string audioFile)
        //{
        //    VoiceIt2 myVoiceIt = new VoiceIt2("key_140b7c835a984de2a4921ea57d128245", "tok_c3d96357247149479e5b13375c13e124");

        //    //myVoiceIt.CreateVoiceEnrollment("usr_9bbb34d6228b45e7b62521b18708a66a", "English", "<recording>");
        //}
    }
}
