using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVNMailer
{
    public class SVNLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }


        int _Revision = 0;
        string _Author = string.Empty;
        string _Date = string.Empty;
        string _Message = string.Empty;
        string _Actions = string.Empty;
        string _LazyMessage = string.Empty;

        public int Revision
        {
            get
            {
                return _Revision;
            }
            set
            {
                _Revision = value;
                NotifyChange("Revision");
            }
        }

        public string Author
        {
            get
            {
                return _Author;
            }
            set
            {
                _Author = value;
                NotifyChange("Author");
            }
        }

        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                NotifyChange("Date");
            }
        }

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                NotifyChange("Message");
            }
        }

        public string Actions
        {
            get
            {
                return _Actions;
            }
            set
            {
                _Actions = value;
                NotifyChange("Actions");
            }
        }

        public string LazyMessage
        {
            get
            {
                return _LazyMessage;
            }
            set
            {
                _LazyMessage = value;
                NotifyChange("LazyMessage");
            }
        }
    }
}
