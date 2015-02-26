using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalMan.App.SignalR
{
    public class SignalRHelper
    {

        #region Fields
        private string connectionId;
        private string gameTag;
        IHubProxy hubManProxy;
        #endregion
        #region Properties
        /// <summary>
        /// Status of connection to server 
        /// </summary>
        public bool Connected { get; private set; }
        /// <summary>
        /// Steps in game.
        /// </summary>
        private int totalDots;

        public int TotalDots
        {
            get { return totalDots; }
            private set { totalDots = value; OnTotalDotsChanged(); }
        }

        /// <summary>
        /// Game Points
        /// </summary>
        private int points;

        public int Points
        {
            get { return points; }
            private set { points = value; OnPointsChanged(); }
        }
        #endregion

        /// <summary>
        /// Constructor. Set default values.
        /// </summary>
        public SignalRHelper()
        {
            Connected = false;
            TotalDots = 0;
            Points = 0;
            connectionId = string.Empty;
            gameTag = string.Empty;
        }

        #region Initialize Methods
        /// <summary>
        /// Initialize the SignalR Helper. Need connectionId.
        /// </summary>
        /// <param name="connectionId">SignalR server Id</param>
        public void Initialize(string connectionId, string gameTag)
        {
            this.connectionId = connectionId;
            this.gameTag = gameTag;
        }

        /// <summary>
        /// Close and dispose the SignalR server connection 
        /// </summary>
        public void Dispose()
        {
            Connected = false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Connect to SignalR server. It need a valid ConnectionId.
        /// </summary>
        public async void Connect()
        {
            try
            {
                //var hubConnection = new HubConnection("http://signalrchatexample.azurewebsites.net/signalr");
                var hubConnection = new HubConnection("http://localhost:23555/");

                hubManProxy = hubConnection.CreateHubProxy("HubMan");

                hubManProxy.On<int>("UpdateRemainingDots", updateRemainingDotsAction);
                hubManProxy.On<int>("UpdateDots", updateDotsAction);

                await hubConnection.Start();
                await hubManProxy.Invoke("JoinGame", connectionId, gameTag);
                Connected = true;
            }
            catch(Exception ex)
            {
                Connected = false;
                throw ex;
            }
        }

        /// <summary>
        /// Send Move command to Game.
        /// </summary>
        /// <param name="direction">Direction to move player.</param>
        public void MovePlayer(string direction)
        {
           if(Connected)
           {
               hubManProxy.Invoke("MovePlayer", direction);
           }
        }
        #endregion

        /// <summary>
        /// Methods to Notify Changed in Properties
        /// </summary>
        #region NotifyPropertyChanged
        public event EventHandler<int> PointsChanged;
        protected void OnPointsChanged()
        {
            EventHandler<int> handler = PointsChanged;
            if (handler != null)
            {
                handler(this, Points);
            }
        }

        public event EventHandler<int> TotalDotsChanged;
        protected void OnTotalDotsChanged()
        {
            EventHandler<int> handler = TotalDotsChanged;
            if (handler != null)
            {
                handler(this, TotalDots);
            }
        }

        private void updateRemainingDotsAction(int remaining)
        {
            TotalDots = remaining;
        }

        private void updateDotsAction(int dots)
        {
            Points = dots;
        }
        #endregion

    }
}
