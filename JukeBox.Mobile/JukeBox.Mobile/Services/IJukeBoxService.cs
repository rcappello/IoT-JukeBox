using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Mobile.Services
{
    public interface IJukeBoxService
    {
        Task PlayAsync(Uri mediaFileUri);
        Task PauseAsync();
        Task StopAsync();
    }
}
