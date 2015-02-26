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
        #region Fields
        private static ConcurrentDictionary<string, string> games = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();
        #endregion

        #region Connection Methods
        public void CreateGame()
        {
            // Create random Id
            string gameId = string.Empty;
            Random random = new Random((int)DateTime.UtcNow.Ticks);
            do
            {
                gameId = random.Next(0, 10000).ToString();
            } while(games.ContainsKey(gameId));

            // Remove last game with same connectionId.
            removeGame(Context.ConnectionId);

            // Add Game server to group
            Groups.Add(Context.ConnectionId, gameId);

            // Add Game to games array
            games.TryAdd(gameId, Context.ConnectionId);

            // Send gameId to game server
            Clients.Client(Context.ConnectionId).setGameId(gameId);
        }

        public void JoinGame(string gameId, string name)
        {
            // If exists game
            if (games.ContainsKey(gameId))
            {
                // Add user to group
                Groups.Add(Context.ConnectionId, gameId);

                // Send addPlayer to game server
                var serverId = string.Empty;
                games.TryGetValue(gameId, out serverId);

                if (!string.IsNullOrEmpty(serverId))
                {
                    Clients.Client(serverId).addPlayer(Context.ConnectionId, name);
                    users.TryAdd(Context.ConnectionId, gameId);
                }
            }
        }

        public void LeaveGame()
        {
            // Remove user from array
            string gameId = string.Empty;
            users.TryRemove(Context.ConnectionId, out gameId);

            // If player is removed
            if (!string.IsNullOrEmpty(gameId))
            {
                // Remove connection from group
                Groups.Remove(Context.ConnectionId, gameId);

                // Send notification to game server
                string serverId = string.Empty;
                games.TryGetValue(gameId, out serverId);

                if (!string.IsNullOrEmpty(serverId))
                {
                    Clients.Client(serverId).removePlayer(Context.ConnectionId);
                }

            }
        }
        #endregion

        #region Actions Methods
        public void MovePlayer(string direction)
        {
            // Get gameId value
            string gameId = string.Empty;
            users.TryGetValue(Context.ConnectionId, out gameId);

            if (!string.IsNullOrEmpty(gameId))
            {
                // Get server connection Id
                string serverId = string.Empty;
                games.TryGetValue(gameId, out serverId);

                // Invoke movePlayer in Game server
                if (!string.IsNullOrEmpty(serverId))
                {
                    Clients.Client(serverId).movePlayer(Context.ConnectionId, direction);
                }
            }
        }
        #endregion

        #region Notification Methods
        public void UpdateRemainingDots(int remaining)
        {
            // Get gameId
            var game = games.Where(g => g.Value.Equals(Context.ConnectionId)).FirstOrDefault();

            string gameId = string.Empty;
            if (!game.Equals(default(KeyValuePair<string, string>)))
            {
                gameId = game.Key;
            }

            // If exists gameId, send the remaining dots to other clients in group
            if(!string.IsNullOrEmpty(gameId))
            {
                Clients.OthersInGroup(gameId).updateRemainingDots(remaining);
            }
        }

        public void UpdateDots( string userId, int dots)
        {
            // If exists userId, send dots to client
            if (!string.IsNullOrEmpty(userId))
            {
                Clients.Client(userId).updateDots(dots);
            }
        }
        #endregion

        #region Private Methods
        private void removeGame(string gameServer)
        {
            // If the old gameServer exist, remove it and remove all users.
            if (games.Values.Contains(gameServer))
            {
                var game = games.Where(g => g.Value.Equals(gameServer)).FirstOrDefault();

                if(!game.Equals(default(KeyValuePair<string, string>)))
                {
                    string gameId = game.Key;

                    // Remove game of games array
                    games.TryRemove(gameId, out gameServer);

                    // Remove all users when Game Server is gameServer
                    var removeUsers = users.Where(user => user.Value.Equals(gameServer));

                    foreach (var user in removeUsers)
                    {
                        string outValue;
                        users.TryRemove(user.Key, out outValue);
                    }
                }
            }
        }
        #endregion

    }

}