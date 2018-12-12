
using JukeBox.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Mobile.Services
{
    public interface IAzureService
    {
        Task<IEnumerable<MediaFile>> GetAllFilesAsync();
    }
}
