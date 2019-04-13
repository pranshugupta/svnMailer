using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SVNUserControl.xaml
    /// </summary>
    public partial class MailerControl : UserControl
    {
        SVNUserControlAdapter _DataContext;
        public MailerControl()
        {
            InitializeComponent();
            _DataContext = new SVNUserControlAdapter();
            this.DataContext = _DataContext;
        }

        public void SetDataGridItemSource(ObservableCollection<SVNLog> svnLogList)
        {
            _DataContext.SVNLogList = svnLogList;
        }

        #region Clear Screen
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            OnClearButtonPressed(new EventArgs());
        }

        public event EventHandler ClearButtonPressed;

        protected virtual void OnClearButtonPressed(EventArgs e)
        {
            EventHandler handler = ClearButtonPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
