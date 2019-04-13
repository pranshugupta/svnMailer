using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SVNMailer
{
    /// <summary>
    /// Interaction logic for LogDetailWindow.xaml
    /// </summary>
    public partial class LogDetailWindow : Window
    {
        public LogDetailWindow(SVNLog svnLog)
        {
            InitializeComponent();
            this.DataContext = svnLog;
        }
    }
}
