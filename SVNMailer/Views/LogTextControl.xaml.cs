using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SVNMailer
{
    /// <summary>
    /// Interaction logic for LogTextControl.xaml
    /// </summary>
    public partial class LogTextControl : UserControl
    {
        public LogTextControl()
        {
            InitializeComponent();
        }

        private void LoadSVNLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lines;
                int lineCount = SVNLogTextBox.LineCount;
                BusinessModel businessModel = new BusinessModel();

                if (lineCount > 1)
                {
                    lines = new List<string>();
                    ObservableCollection<SVNLog> svnLogList;
                    SVNLogArgs svnLogArgs;

                    for (int line = 0; line < lineCount; line++)
                        lines.Add(SVNLogTextBox.GetLineText(line));

                    svnLogList = businessModel.LoadSVNLoad(lines);

                    svnLogArgs = new SVNLogArgs();
                    svnLogArgs.SVNLogList = svnLogList;
                    OnSVNLogLoaded(svnLogArgs);
                }
                else
                {
                    MessageBox.Show("Copy and paste SVN log content in the textbox and then click the Load Button");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally 
            {
                SVNLogTextBox.Clear();
            }
        }


        public event EventHandler<SVNLogArgs> SVNLogLoaded;

        protected virtual void OnSVNLogLoaded(SVNLogArgs svnLogList)
        {
            EventHandler<SVNLogArgs> handler = SVNLogLoaded;
            if (handler != null)
            {
                handler(this, svnLogList);
            }
        }
    }
}
