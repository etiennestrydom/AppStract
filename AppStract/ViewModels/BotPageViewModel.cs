using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using AppStract.Models;
using Amazon.Lex;
using Amazon;
using Amazon.Lex.Model;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.SpeechRecognition;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Text.RegularExpressions;

namespace AppStract.ViewModels
{
    public class BotPageViewModel : ViewModelBase
    {
        private AmazonLexClient _amazonLex;
        private string _botName;
        private string _botAlias;
        private string _userId;

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public string Phrase { get; set; }

        public ICommand ListenForCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var listener = CrossSpeechRecognition.Current.ContinuousDictation().Subscribe(phrase =>
                        {
                            Phrase = Phrase + " " + phrase;
                        });

                        await Task.Delay(5000);
                        listener.Dispose();

                        await ExecuteTask();
                    }
                    catch (Exception e)
                    {

                    }
                });
            }
        }

        private async Task ExecuteTask()
        {
            if (!String.IsNullOrEmpty(Phrase))
            {
                var result = Regex.Replace(Phrase, " {2,}", " ");
                string textToSend = result.Substring(1);

                Messages.Insert(0, new Message() { Text = textToSend, User = App.User });
                await SendTextToBot(Phrase);

                Phrase = String.Empty;
            }
        }

        public BotPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _amazonLex = new AmazonLexClient("AKIAI6DDT5M37MGR7GRQ", "woEMaYL7PN9mcELs2kyRH0bXJw+Ooavf0T6bnUmI", RegionEndpoint.EUWest1);
            _botName = "BuyPackages";
            _botAlias = "FirstTest";
            _userId = Guid.NewGuid().ToString();

            Messages.Insert(0, new Message() { Text = "Hi" });
            Messages.Insert(0, new Message() { Text = "How are you?" });

            //OnSendCommand = new Command(async () =>
            //{
            //    if (!string.IsNullOrEmpty(TextToSend))
            //    {
            //        Messages.Insert(0, new Message() { Text = TextToSend, User = App.User });
            //        await SendTextToBot(TextToSend);

            //        //TextToSend = string.Empty;
            //    }
            //});

            //var listener = CrossSpeechRecognition
            //.Current
            //.ListenUntilPause()
            //.Subscribe(async phrase =>
            //{
            //    //var stopWatch = new Stopwatch();
            //    //stopWatch.Start();
            //    //Phrase = Phrase + " " + phrase;

            //    //var myTimer = new System.Timers.Timer();
            //    //myTimer.Elapsed += new ElapsedEventHandler(async delegate
            //    //{

            //    //    stopWatch.Reset();
            //    //});

            //    //myTimer.Interval = 1000;
            //    //myTimer.Enabled = true;

            //    await ExecuteSendPhrase(phrase);
            //});
        }

        //private async Task ExecuteSendPhrase(string phrase)
        //{
        //    Messages.Insert(0, new Message() { Text = phrase, User = App.User });
        //    await SendTextToBot(phrase);

        //    Phrase = String.Empty;
        //}

        private async Task SendTextToBot(string textToSend)
        {
            var postTextRequest = new PostTextRequest()
            {
                BotAlias = _botAlias,
                BotName = _botName,
                UserId = _userId,
                InputText = textToSend
            };

            var response = await _amazonLex.PostTextAsync(postTextRequest);

            Messages.Insert(0, new Message() { Text = response.Message });
            await TextToSpeech.SpeakAsync(response.Message);
        }
    }
}
