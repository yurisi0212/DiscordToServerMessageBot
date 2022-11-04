using System.Net;
using System.Reflection;
using CoreRCON;
using CoreRCON.Parsers.Standard;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace DiscordToServerMessageBot {
    class Program {

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private TokenManager _token;
        private RCON _rcon;

        static void Main() {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() {
            _client = new DiscordSocketClient(new DiscordSocketConfig {
                LogLevel = LogSeverity.Info
            });
            _client.Log += Log;
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();
            _client.MessageReceived += CommandRecieved;
            _token = new TokenManager();
            _rcon = new RCON(IPAddress.Parse(_token.IPAddress), _token.Port, _token.RconPassWord);
            await _rcon.ConnectAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, _token.DiscordToken);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task CommandRecieved(SocketMessage messageParam) {
            if (messageParam == null)
                return;

            var message = messageParam as SocketUserMessage;

            if (message == null)
                return;

            if (message.Author.IsBot)
                return;

            if (messageParam.Channel.Id.ToString() != _token.DiscordMessageChannel) {
                return;
            }

            var nickname = (messageParam.Author as SocketGuildUser).Nickname;


            string response = await _rcon.SendCommandAsync("dtsm \"" + nickname + "\" \"" + message+ "\"");
        }

        private Task Log(LogMessage message) {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}