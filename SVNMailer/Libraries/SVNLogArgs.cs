using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVNMailer
{
    public class SVNLogArgs : EventArgs
    {
        public ObservableCollection<SVNLog> SVNLogList { get; set; }
    }
}
