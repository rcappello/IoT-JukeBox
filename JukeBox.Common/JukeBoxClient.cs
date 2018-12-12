using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Common
{
    public class JukeBoxClient<T> : IDisposable
    {
        private static DeviceClient deviceClient;
        private bool receiveMessages = true;

        public JukeBoxClient(string connectionString)
        {
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
        }

        private Action<T> messageReceivedEvent;
        public JukeBoxClient<T> OnMessageReceivedEvent(Action<T> action)
        {
            messageReceivedEvent = action;
            return this;
        }

        public Task StartReceiveAsync()
        {
            receiveMessages = true;

            var task = Task.Run(async () =>
            {
                while (receiveMessages)
                {
                    var receivedMessage = await deviceClient.ReceiveAsync();
                    if (receivedMessage == null)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    var message = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                    var jsonMessage = JsonConvert.DeserializeObject<T>(message);

                    messageReceivedEvent?.Invoke(jsonMessage);

                    await deviceClient.CompleteAsync(receivedMessage);

                    await Task.Delay(100);
                }
            });

            return Task.CompletedTask;
        }

        public Task StopReceiveAsync()
        {
            receiveMessages = false;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            receiveMessages = false;
            deviceClient.CloseAsync();
        }
    }
}
