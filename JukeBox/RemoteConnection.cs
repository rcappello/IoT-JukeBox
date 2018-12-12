using JukeBox.Common;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace JukeBox
{
    public class RemoteConnection : IDisposable
    {
        private static JukeBoxClient<JukeBoxMessage> jukeBoxClient;
        private Action<JukeBoxMessage> requiredActionMessage;

        public Task ConnectAsync(Action<JukeBoxMessage> actionMessage)
        {
            requiredActionMessage = actionMessage;

            jukeBoxClient = new JukeBoxClient<JukeBoxMessage>(Constants.IoTHubClientConnectionString);
            jukeBoxClient.OnMessageReceivedEvent(async (message) => await RaiseActionAsync(message));

            return jukeBoxClient.StartReceiveAsync();
        }

        private async Task RaiseActionAsync(JukeBoxMessage message)
        {
            var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;
            if (dispatcher != null)
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => requiredActionMessage?.Invoke(message));
            else
                requiredActionMessage?.Invoke(message);
        }

        public void Dispose()
        {
            jukeBoxClient.StopReceiveAsync();
        }
    }
}
