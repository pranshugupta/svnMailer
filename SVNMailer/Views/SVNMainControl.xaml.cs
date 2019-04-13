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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SVNMailer
{
    /// <summary>
    /// Interaction logic for SVNMainControlxaml.xaml
    /// </summary>
    public partial class SVNMainControl : UserControl
    {
        public SVNMainControl()
        {
            InitializeComponent();
            LogTextCtrl.SVNLogLoaded += LogTextCtrl_SVNLogLoaded;
            MailerCtrl.ClearButtonPressed += MailerCtrl_ClearButtonPressed;
        }

        void MailerCtrl_ClearButtonPressed(object sender, EventArgs e)
        {
            LogTextCtrl.Visibility = Visibility.Visible;
            MailerCtrl.Visibility = Visibility.Collapsed;
        }

        void LogTextCtrl_SVNLogLoaded(object sender, SVNLogArgs e)
        {
            MailerCtrl.SetDataGridItemSource(e.SVNLogList);
            LogTextCtrl.Visibility = Visibility.Collapsed;
            MailerCtrl.Visibility = Visibility.Visible;
        }
    }
}
