using System;
using System.Threading.Tasks;

namespace JukeBox.Mobile.Services
{
    public interface INetworkService
    {
        bool IsConnected { get; }

        event EventHandler ConnectivityChanged;

        Task<bool> IsInternetAvailableAsync();
    }
}
