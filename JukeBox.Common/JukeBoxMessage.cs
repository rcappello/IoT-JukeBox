using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Common
{
    public class JukeBoxMessage
    {
        public string Arguments { get; set; }
        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
      PlayMusic,
      PauseMusic,
      StopMusic,
    }
}
