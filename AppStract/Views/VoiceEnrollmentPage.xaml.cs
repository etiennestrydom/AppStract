using Xamarin.Forms;
using System;
using AppStract.ViewModels;

namespace AppStract.Views
{
    public partial class VoiceEnrollmentPage : ContentPage
    {
        public VoiceEnrollmentPage()
        {
            InitializeComponent();
        }

        public void Handle_Pressed(object sender, EventArgs e)
        {
            (BindingContext as VoiceEnrollmentPageViewModel).RecordAudio();
        }

        public void Handle_Released(object sender, EventArgs e)
        {
            (BindingContext as VoiceEnrollmentPageViewModel).RecordAudio();
        }
    }
}
