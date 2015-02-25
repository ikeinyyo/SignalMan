using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalMan.Web.Hubs
{
    public class HubMan : Hub
    {
        private static ConcurrentDictionary<string, string> games = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();
        public void CreateGame()
        {
            // Create random Id
            string gameId = string.Empty;
            Random random = new Random((int)DateTime.UtcNow.Ticks);
            do
            {
                gameId = random.Next(0, 10000).ToString();
            } while(games.ContainsKey(gameId));
            
            // Add Game server to group
            Groups.Add(Context.ConnectionId, gameId);

            // Add Game to games array
            games.TryAdd(gameId, Context.ConnectionId);

            // Send gameId to game server
            Clients.Client(Context.ConnectionId).setGameId(gameId);
        }


        public void JoinGame(string gameId, string name)
        {
            if (games.ContainsKey(gameId))
            {
                Groups.Add(Context.ConnectionId, gameId);
                var serverId = string.Empty;
                games.TryGetValue(gameId, out serverId);

                if (!string.IsNullOrEmpty(serverId))
                {
                    Clients.Client(serverId).addPlayer(Context.ConnectionId, name);
                    users.TryAdd(Context.ConnectionId, gameId);
                }
            }
        }

        public void MovePlayer(string direction)
        {
            string gameId = string.Empty;
            users.TryGetValue(Context.ConnectionId, out gameId);

            if (!string.IsNullOrEmpty(gameId))
            {
                string serverId = string.Empty;
                games.TryGetValue(gameId, out serverId);

                if (!string.IsNullOrEmpty(serverId))
                {
                    Clients.Client(serverId).movePlayer(Context.ConnectionId, direction);
                }
            }
        }

    }

}