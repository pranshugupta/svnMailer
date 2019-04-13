using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace SVNMailer
{
    class SVNUserControlAdapter : INotifyPropertyChanged
    {
        #region Private Variables;

        ObservableCollection<SVNLog> _SVNLogList =null;
        SVNLog _SelectedSVNLog = null;

        Preferences _Preferences = null;

        BusinessModel _BusinessModel;

        #endregion

        #region Public Properties
        public ObservableCollection<SVNLog> SVNLogList
        {
            get
            {
                return _SVNLogList;
            }
            set
            {
                _SVNLogList = value;
                NotifyChange("SVNLogList");
            }
        }
        public SVNLog SelectedSVNLog
        {
            get
            {
                return _SelectedSVNLog;
            }
            set
            {
                _SelectedSVNLog = value;
                NotifyChange("SelectedSVNLog");
            }
        }

        public Preferences Preferences
        {
            get
            {
                return _Preferences;
            }
            set
            {
                _Preferences = value;
                NotifyChange("Preferences");
            }
        }

        public ICommand SavePreferencesCommand { get; set; }
        public ICommand ShowDetailCommand { get; set; }
        public ICommand SendNotificationCommand { get; set; }

        #endregion

        #region Constructor
        public SVNUserControlAdapter()
        {
            _BusinessModel = new BusinessModel();
            _Preferences = _BusinessModel.ReadPreferences();

            SavePreferencesCommand = new RelayCommand(CanSavePreference, SavePreferences);
            ShowDetailCommand = new RelayCommand(CanShowDetails, ShowDetails);
            SendNotificationCommand = new RelayCommand(CanSendNotification, SendNotification);
        }
        #endregion

        #region Save Preferences
        private bool CanSavePreference()
        {
            return true;
        }
        private void SavePreferences()
        {
            _BusinessModel.SavePreferences(_Preferences);
        }
        #endregion

        #region Send Notification Mail
        private bool CanSendNotification()
        {
            return true;
        }
        private void SendNotification()
        {
            _BusinessModel.SendNotification(_SVNLogList, _Preferences);
        }
        #endregion

        #region Show Selected Log Details
        private bool CanShowDetails()
        {
            return true;
        }
        private void ShowDetails()
        {
            LogDetailWindow logDetails = new LogDetailWindow(SelectedSVNLog);
            logDetails.ShowDialog();
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}