using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToServerMessageBot {
    public class TokenManager {

        public TokenManager() {
            DiscordToken = "";
            DiscordMessageChannel = "";
            IPAddress = "";
            Port = 19132;
            RconPassWord = "";
        }

        public string DiscordToken { get; }

        public string DiscordMessageChannel { get; }

        public string IPAddress { get; }

        public ushort Port { get; }

        public string RconPassWord { get; }

    }
}
