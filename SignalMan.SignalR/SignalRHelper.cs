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
        public bool Connected { get; private set; }
        private int steps;

        public int Steps
        {
            get { return steps; }
            private set { steps = value; OnStepsChanged(); }
        }
        private int points;

        public int Points
        {
            get { return points; }
            private set { points = value; OnPointsChanged(); }
        }
        #endregion

        public SignalRHelper()
        {
            Connected = false;
            Steps = 0;
            Points = 0;
        }

        #region Initialize Methods
        public void Initialize(string connectionId)
        {

        }
        public void Dispose()
        {

        }
        #endregion

        #region Public Methods
        public void Connect()
        {
            Connected = true;
        }

        public void MovePlayer(string direction)
        {
            Steps++;
            Points = Steps * 5;
        }
        #endregion

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
