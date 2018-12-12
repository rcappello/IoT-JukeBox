using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Common
{
    public class JukeBoxServer
    {
        private static ServiceClient s_serviceClient;

        public JukeBoxServer(string connectionString)
        {
            s_serviceClient = ServiceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.TransportType.Amqp);
        }

        public Task SendMessageToDeviceAsync<T>(string deviceId, T message) where T : class
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            var messageToSend = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(jsonMessage));

            return s_serviceClient.SendAsync(deviceId, messageToSend);
        }
    }
}
