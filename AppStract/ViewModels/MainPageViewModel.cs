using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoiceIt2API;

namespace AppStract.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            VoiceIt2 myVoiceIt = new VoiceIt2("key_140b7c835a984de2a4921ea57d128245", "tok_c3d96357247149479e5b13375c13e124");
            var users = myVoiceIt.GetAllUsers();
        }
    }
}
