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
        //public ICommand OnSendCommand
        //{
        //    get
        //    {
        //        return new Command<string>(async (textToSend) =>
        //        {
        //            if (!string.IsNullOrEmpty(textToSend))
        //            {
        //                Messages.Insert(0, new Message() { Text = textToSend, User = App.User });
        //                TextToSend = string.Empty;

        //                await SendTextToBot(textToSend);
        //            }
        //        });
        //    }
        //}

        public BotPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _amazonLex = new AmazonLexClient("AKIAI6DDT5M37MGR7GRQ", "woEMaYL7PN9mcELs2kyRH0bXJw+Ooavf0T6bnUmI", RegionEndpoint.EUWest1);
            _botName = "BuyPackages";
            _botAlias = "FirstTest";
            _userId = Guid.NewGuid().ToString();

            Messages.Insert(0, new Message() { Text = "Hi" });
            Messages.Insert(0, new Message() { Text = "How are you?" });

            OnSendCommand = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Messages.Insert(0, new Message() { Text = TextToSend, User = App.User });
                    await SendTextToBot(TextToSend);

                    //TextToSend = string.Empty;
                }
            });
        }

        private async Task SendTextToBot(string textToSend)
        {
            var postTextRequest = new PostTextRequest()
            {
                BotAlias = _botAlias,
                BotName = _botName,
                UserId = _userId,
                InputText = textToSend
            };

            TextToSend = string.Empty;

            var response = await _amazonLex.PostTextAsync(postTextRequest);

            Messages.Insert(0, new Message() { Text = response.Message });
            await TextToSpeech.SpeakAsync(response.Message);
        }
    }
}
