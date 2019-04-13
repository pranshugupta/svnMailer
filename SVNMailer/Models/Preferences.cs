using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SVNMailer
{
    public class Preferences:INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Private Variables
        bool _IsRevisionColumnVisible;
        bool _IsAuthorColumnVisible;
        bool _IsDateColumnVisible;
        bool _IsActionsColumnVisible;

        string _CCMailTo;
        string _MailSubject = "Correct SVN Log Comments";
        string _Signature;
        string _Domain;
        #endregion

        #region Public Properties
        public bool IsRevisionColumnVisible
        {
            get
            {
                return _IsRevisionColumnVisible;
            }
            set
            {
                _IsRevisionColumnVisible = value;
                NotifyChange("_IsRevisionColumnVisible");
            }
        }
        public bool IsAuthorColumnVisible
        {
            get
            {
                return _IsAuthorColumnVisible;
            }
            set
            {
                _IsAuthorColumnVisible = value;
                NotifyChange("IsAuthorColumnVisible");
            }
        }
        public bool IsDateColumnVisible
        {
            get
            {
                return _IsDateColumnVisible;
            }
            set
            {
                _IsDateColumnVisible = value;
                NotifyChange("IsDateColumnVisible");
            }
        }
        public bool IsActionsColumnVisible
        {
            get
            {
                return _IsActionsColumnVisible;
            }
            set
            {
                _IsActionsColumnVisible = value;
                NotifyChange("IsActionsColumnVisible");
            }
        }

        public string CCMailTo
        {
            get
            {
                return _CCMailTo;
            }
            set
            {
                _CCMailTo = value;
                NotifyChange("CCMailTo");
            }
        }
        public string MailSubject
        {
            get
            {
                return _MailSubject;
            }
            set
            {
                _MailSubject = value;
                NotifyChange("MailSubject");
            }
        }
        public string Signature
        {
            get
            {
                return _Signature;
            }
            set
            {
                _Signature = value;
                NotifyChange("Signature");
            }
        }

        public string Domain
        {
            get
            {
                return _Domain;
            }
            set
            {
                _Domain = value;
                NotifyChange("Domain");
            }
        }
        #endregion
    }
}
