using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace JukeBox
{
    public sealed partial class MainPage : Page, IDisposable
    {
        private readonly RemoteConnection connection;

        private string CONNECT = "Connected";

        private SpeechSynthesizer synth;


        public MainPage()
        {
            InitializeComponent();

            synth = new SpeechSynthesizer();
            connection = new RemoteConnection();
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                await SpeechAsync("JukeBox: There's no internet connection");
            }
            else
            {
                await SpeechAsync("Welcome to JukeBox");
            }

            await connection.ConnectAsync((message) =>
            {
                switch (message.Type)
                {
                    case Common.MessageType.PlayMusic:
                        mediaPlayer.Source = new Uri(message.Arguments);
                        mediaPlayer.Play();
                        textMessage.Text = "Playing...";
                        break;
                    case Common.MessageType.PauseMusic:
                        if (mediaPlayer.CurrentState == MediaElementState.Paused)
                        {
                            mediaPlayer.Play();
                            textMessage.Text = "Resumed";
                        }
                        else
                        {
                            mediaPlayer.Pause();
                            textMessage.Text = "Pause";
                        }
                        break;
                    case Common.MessageType.StopMusic:
                        mediaPlayer.Stop();
                        textMessage.Text = "Stop";
                        break;
                    default:
                        break;
                }
            });

            textMessage.Text = CONNECT;

            base.OnNavigatedTo(e);
        }

        private async Task SpeechAsync(string message)
        {
            var stream = await synth.SynthesizeTextToStreamAsync(message);
            mediaPlayer.SetSource(stream, stream.ContentType);
            mediaPlayer.Play();
        }

        public void Dispose() => synth.Dispose();
    }
}