using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SignalMan.SignalR;
using System;

namespace SignalMan.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private SignalRHelper signalRHelper;
        #endregion
        #region Properties
        private RelayCommand connect;

        public RelayCommand Connect
        {
            get { return connect; }
            set { connect = value; RaisePropertyChanged(); }
        }

        private RelayCommand<string> move;

        public RelayCommand<string> Move
        {
            get { return move; }
            set { move = value; RaisePropertyChanged(); }
        }

        private string connectionId;

        public string ConnectionId
        {
            get { return connectionId; }
            set { connectionId = value; RaisePropertyChanged(); }
        }

        private int steps;

        public int Steps
        {
            get { return steps; }
            set { steps = value; RaisePropertyChanged(); }
        }

        private int points;

        public int Points
        {
            get { return points; }
            set { points = value; RaisePropertyChanged(); }
        }

        #endregion

        public MainViewModel()
        {
            Connect = new RelayCommand(connectAction);
            Move = new RelayCommand<string>(moveAction);
            Points = 0;
            Steps = 0;
            signalRHelper = new SignalRHelper();
        }

        #region Commands
        private void connectAction()
        {
            if(signalRHelper.Connected)
            {
                signalRHelper.Dispose();
            }

            signalRHelper.Initialize(ConnectionId);
            signalRHelper.Connect();
            signalRHelper.PointsChanged += OnPointsChanged;
            signalRHelper.StepsChanged += OnStepsChanged;

        }

        private void moveAction(string direction)
        {
            if (signalRHelper.Connected)
            {
                signalRHelper.MovePlayer(direction);
            }
        }
        #endregion

        #region Update Properties
        private void OnPointsChanged(object sender, int e)
        {
            Points = e;
        }

        private void OnStepsChanged(object sender, int e)
        {
            Steps = e;
        }
        #endregion

    }
}