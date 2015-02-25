using System;
using System.Collections.Generic;
using System.Text;

namespace SignalMan.App.SignalR
{
    public class SignalRHelper
    {
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
        }

        #region Initialize Methods
        /// <summary>
        /// Initialize the SignalR Helper. Need connectionId.
        /// </summary>
        /// <param name="connectionId">SignalR server Id</param>
        public void Initialize(string connectionId)
        {

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
        public void Connect()
        {
            Connected = true;
        }

        /// <summary>
        /// Send Move command to Game.
        /// </summary>
        /// <param name="direction">Direction to move player.</param>
        public void MovePlayer(string direction)
        {
            TotalDots++;
            Points = TotalDots * 5;
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

        #endregion

    }
}
