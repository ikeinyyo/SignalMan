using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SignalMan.SignalR;
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
            Move = new RelayCommand<string>(moveAction);
            Points = 0;
            Steps = 0;
            signalRHelper = new SignalRHelper();
            Connected = false;
        }
        #endregion

        #region Commands
        private void connectAction()
        {
            // Clear last connection
            disconnect();

            // Attach to closing app
            App.Current.Suspending += OnAppSuspending;
            try
            {
                // Connect
                signalRHelper.Initialize(ConnectionId);
                signalRHelper.Connect();
                signalRHelper.PointsChanged += OnPointsChanged;
                signalRHelper.StepsChanged += OnStepsChanged;
                Connected = signalRHelper.Connected;
            }
            catch
            {
                showError(connectionError);
            }

        }

      

        private void disconnect()
        {
            App.Current.Suspending -= OnAppSuspending;
            if (signalRHelper.Connected)
            {
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

        private void OnStepsChanged(object sender, int e)
        {
            Steps = e;
        }
        #endregion

        #region Events
        void OnAppSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            disconnect();

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