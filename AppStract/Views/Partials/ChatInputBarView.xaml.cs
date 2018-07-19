using System;
using Xamarin.Forms;
using AppStract.ViewModels;

namespace AppStract.Views.Partials
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            (BindingContext as BotPageViewModel).OnSendCommand.Execute(null);
            chatTextInput.Focus();

            // (this.Parent.Parent as ChatPage).ScrollListCommand.Execute(null);
        }

    }
}
