using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Util
{
    internal class DRPCManager
    {
        public static DiscordRpcClient client;

        public static void StartRPC()
        {
            client = new DiscordRpcClient("1108466255897296916");

            // Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            // Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            // Connect to the RPC
            client.Initialize();
        }

        public static void StopRPC()
        {
            client.Deinitialize();
        }

        public static void SetRPC(RichPresence rpc) => client.SetPresence(rpc);
    }
}
