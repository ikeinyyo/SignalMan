using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SignalMan.App.SignalR;
using System;
using Windows.UI.Popups;

namespace SignalMan.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Const
        private const string errorTitle = "SignalMan Error";
        private const string connectionError = "Error to connect to Game Server";
        private const string actionError = "Error to execute action. Are you connected to Game Server?";
        #endregion

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

        private RelayCommand disconnect;

        public RelayCommand Disconnect
        {
            get { return disconnect; }
            set { disconnect = value; RaisePropertyChanged(); }
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

        private string gameTag;

        public string GameTag
        {
            get { return gameTag; }
            set { gameTag = value; RaisePropertyChanged(); }
        }

        private int totalDots;

        public int TotalDots
        {
            get { return totalDots; }
            set { totalDots = value; RaisePropertyChanged(); }
        }

        private int points;

        public int Points
        {
            get { return points; }
            set { points = value; RaisePropertyChanged(); }
        }

        private bool connected;

        public bool Connected
        {
            get { return connected; }
            set { connected = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Initialize Methods
        public MainViewModel()
        {
            Connect = new RelayCommand(connectAction);
            Disconnect = new RelayCommand(disconnectAction);
            Move = new RelayCommand<string>(moveAction);
            Points = 0;
            TotalDots = 0;
            signalRHelper = new SignalRHelper();
            Connected = false;
        }
        #endregion

        #region Commands
        private void connectAction()
        {
            // Clear last connection
            clearConnection();

            // Attach to closing app
            App.Current.Suspending += OnAppSuspending;
            try
            {
                // Connect
                signalRHelper.Initialize(ConnectionId);
                signalRHelper.Connect();
                signalRHelper.PointsChanged += OnPointsChanged;
                signalRHelper.TotalDotsChanged += OnTotalDotsChanged;
                Connected = signalRHelper.Connected;
            }
            catch
            {
                showError(connectionError);
            }

        }
        private void disconnectAction()
        {
            // Clear last connection
            clearConnection();
        }

        private void clearConnection()
        {
            App.Current.Suspending -= OnAppSuspending;
            if (signalRHelper.Connected)
            {
                signalRHelper.PointsChanged -= OnPointsChanged;
                signalRHelper.TotalDotsChanged -= OnTotalDotsChanged;
                signalRHelper.Dispose();
                Connected = signalRHelper.Connected;
            }
        }

        private void moveAction(string direction)
        {
            if (signalRHelper.Connected)
            {
                try
                {
                    signalRHelper.MovePlayer(direction);
                }
                catch
                {
                    showError(actionError);
                }
            }
            else
            {
                showError(actionError);
            }
        }
        #endregion

        #region Update Properties
        private void OnPointsChanged(object sender, int e)
        {
            Points = e;
        }

        private void OnTotalDotsChanged(object sender, int e)
        {
            TotalDots = e;
        }
        #endregion

        #region Events
        void OnAppSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            clearConnection();

            deferral.Complete();
        }
        #endregion

        #region Private Methods
        private void showError(string errorMessage)
        {
            MessageDialog errorDialog = new MessageDialog(errorMessage);
            errorDialog.Title = errorTitle;
            var taks = errorDialog.ShowAsync();
        }
        #endregion

    }
}