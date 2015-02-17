using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SignalMan.SignalR
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
        private int steps;

        public int Steps
        {
            get { return steps; }
            private set { steps = value; OnStepsChanged(); }
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
            Steps = 0;
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
            Steps++;
            Points = Steps * 5;
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

        public event EventHandler<int> StepsChanged;
        protected void OnStepsChanged()
        {
            EventHandler<int> handler = StepsChanged;
            if (handler != null)
            {
                handler(this, Steps);
            }
        }

        #endregion

    }
}
