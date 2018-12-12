using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Mobile.Services
{
    public class JukeBoxService : IJukeBoxService
    {
        JukeBox.Common.JukeBoxServer server;

        public JukeBoxService()
        {
            server = new JukeBox.Common.JukeBoxServer(Common.Constants.IoTHubServerConnectionString);
        }

        public Task PlayAsync(Uri mediaFileUri)
        {
            return server.SendMessageToDeviceAsync<JukeBox.Common.JukeBoxMessage>(Common.Constants.DeviceId,
                new JukeBox.Common.JukeBoxMessage {
                    Type = JukeBox.Common.MessageType.PlayMusic,
                    Arguments = mediaFileUri.ToString()
                });
        }
        public Task PauseAsync()
        {
            return server.SendMessageToDeviceAsync<JukeBox.Common.JukeBoxMessage>(Common.Constants.DeviceId, 
                new JukeBox.Common.JukeBoxMessage {
                    Type = JukeBox.Common.MessageType.PauseMusic,
                    Arguments = String.Empty
                });
        }

        public Task StopAsync()
        {
            return server.SendMessageToDeviceAsync<JukeBox.Common.JukeBoxMessage>(Common.Constants.DeviceId,
                new JukeBox.Common.JukeBoxMessage
                {
                    Type = JukeBox.Common.MessageType.StopMusic,
                    Arguments = String.Empty
                });
        }
    }
}
