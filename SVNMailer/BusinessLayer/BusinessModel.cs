using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace SVNMailer
{
    class BusinessModel
    {
        #region Load Grid
        public ObservableCollection<SVNLog> LoadSVNLoad(List<string> lines)
        {
            int _CurrentLine = 0;
            ObservableCollection<SVNLog> svnLogList = new ObservableCollection<SVNLog>();
            while (_CurrentLine < lines.Count - 1)
            {
                AddRecord(lines, ref svnLogList, ref _CurrentLine);
            }

            return svnLogList;
        }

        private void AddRecord(List<string> lines, ref ObservableCollection<SVNLog> svnLogList, ref int _CurrentLine)
        {
            SVNLog SVNData;
            string _Line;
            _Line = lines[_CurrentLine++];
            if (string.IsNullOrWhiteSpace(_Line))
                return;

            SVNData = new SVNLog();
            string[] revisionLine = _Line.Split(':');
            {
                SVNData.Revision = Convert.ToInt32(revisionLine[1].Trim());
            }

            _Line = lines[_CurrentLine++];
            string[] authorLine = _Line.Split(':');
            {
                SVNData.Author = authorLine[1].Trim();
            }

            _Line = lines[_CurrentLine++];
            SVNData.Date = _Line;


            //discard Message:
            _CurrentLine++;

            _Line = lines[_CurrentLine++];
            while (!_Line.Contains("----"))
            {
                SVNData.Message += _Line + "\n";
                _Line = lines[_CurrentLine++];
            }

            _Line = lines[_CurrentLine++];
            while (!string.IsNullOrWhiteSpace(_Line))
            {
                SVNData.Actions += _Line + "\n";
                _Line = lines[_CurrentLine++];
            }

            //Remove Blank Lines
            {
                if (SVNData.Date.Length > 2)
                    SVNData.Date = SVNData.Date.Substring(0, SVNData.Date.Length - 2);

                if (SVNData.Message.Length > 3)
                    SVNData.Message = SVNData.Message.Substring(0, SVNData.Message.Length - 3);

                if (SVNData.Actions.Length > 3)
                    SVNData.Actions = SVNData.Actions.Substring(0, SVNData.Actions.Length - 3);
            }

            svnLogList.Add(SVNData);
        }

        #endregion

        #region Mail Sender

        StringBuilder _ExceptionMessage;
        public void SendNotification(ObservableCollection<SVNLog> svnLogList, Preferences preferences)
        {
            try
            {
                Dictionary<string, List<SVNLog>> mailerDictionary = GetsLogsForEachIndividual(svnLogList);

                foreach (var item in mailerDictionary)
                    SendNotifications(item, preferences, ExceptionCallback);

                if (_ExceptionMessage != null)
                    throw new Exception(_ExceptionMessage.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _ExceptionMessage = null;
            }
        }

        private Dictionary<string, List<SVNLog>> GetsLogsForEachIndividual(ObservableCollection<SVNLog> svnLogList)
        {
            IEnumerable<SVNLog> logList = svnLogList.Where(log => !string.IsNullOrWhiteSpace(log.LazyMessage));

            Dictionary<string, List<SVNLog>> mailerDictionary = new Dictionary<string, List<SVNLog>>();
            List<SVNLog> logs;
            foreach (var log in logList)
            {
                if (mailerDictionary.ContainsKey(log.Author))
                {
                    mailerDictionary[log.Author].Add(log);
                }
                else
                {
                    logs = new List<SVNLog>();
                    logs.Add(log);
                    mailerDictionary.Add(log.Author, logs);
                }
            }

            return mailerDictionary;
        }

        private void SendNotifications(KeyValuePair<string, List<SVNLog>> item, Preferences preferences, Action<Exception> ExceptionCallback)
        {
            Outlook.Application application = null;
            Outlook.MailItem mailItem = null;
            Outlook.Recipients recipients = null;
            Outlook.Recipient recipient = null;
            try
            {
                if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
                    application = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
                else
                    application = new Outlook.Application();

                mailItem = (Outlook.MailItem)application.CreateItem(Outlook.OlItemType.olMailItem);

                recipients = (Outlook.Recipients)mailItem.Recipients;

                recipient = (Outlook.Recipient)recipients.Add(string.Format("{0}@{1}",item.Key, preferences.Domain));
                recipient.Type = (int)Outlook.OlMailRecipientType.olTo;
                recipient.Resolve();

                recipient = (Outlook.Recipient)recipients.Add(preferences.CCMailTo);
                recipient.Type = (int)Outlook.OlMailRecipientType.olCC;
                recipient.Resolve();

                mailItem.Subject = preferences.MailSubject;
                mailItem.HTMLBody = string.Format("<html><body style=\"color: #005180;\">Hi,<br/>{0}<br/><br/><table border=\"1\"><col widtd=\"55\"><col widtd=\"100\"><col widtd=\"135\"><col widtd=\"225\"><col widtd=\"225\"><tr><td BGCOLOR=\"#0C2654;\" style=\"color: #000000;\"><b>Revision<b></td><td BGCOLOR=\"#0C2654;\" style=\"color: #000000;\"><b>Author<b></td> <td BGCOLOR=\"#0C2654;\" style=\"color: #000000;\"><b>Date<b></td><td BGCOLOR=\"#0C2654;\" style=\"color: #000000;\"><b>Message<b></td><td BGCOLOR=\"#0C2654;\" style=\"color: #000000;\"<b>Reviewer Comment<b></td><td BGCOLOR=\"#0C2654;\" style=\"color: #000000;\"><b>Action(s)<b></td></tr>{1}</table><br/>Regards,<br/>{2}</body></html>","Please correct below SVN log comment(s):", FormatMailBody(item.Value), preferences.Signature);

                (mailItem as Outlook._MailItem).Send();
            }
            catch (Exception exeption)
            {
                ExceptionCallback(exeption);
            }
            finally
            {
                recipient = null;
                recipients = null;
                mailItem = null;
                application = null;
            }
        }

        private string FormatMailBody(List<SVNLog> list)
        {
            string rowData = string.Empty;
            foreach (var svnLog in list)
            {
                string msg = string.Format("<tr><td BGCOLOR=\"#ccccdd;\">{0}</td><td BGCOLOR=\"#ccccdd;\">{1}</td><td BGCOLOR=\"#ccccdd;\">{2}</td><td BGCOLOR=\"#FAFCFF;\"><b>{3}<b></td><td BGCOLOR=\"#FAFCFF;\"><b>{4}<b></td><td BGCOLOR=\"#ccccdd;\">{5}</td></tr>",
                    svnLog.Revision, svnLog.Author, svnLog.Date, svnLog.Message, svnLog.LazyMessage, svnLog.Actions);
                rowData += msg;
            }

            rowData = rowData.Replace("\n", "<br/>");
            return rowData;
        }

        void ExceptionCallback(Exception exception)
        {
            if (_ExceptionMessage == null)
                _ExceptionMessage = new StringBuilder();
            _ExceptionMessage.AppendLine(exception.Message);
        }
        #endregion

        #region Preferences

        public Preferences ReadPreferences()
        {
            Preferences preferences = new Preferences();
            string directory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(directory, @"Preferences.xml");
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode preferencesNode = xmlDoc.SelectSingleNode("//Preferences");
                foreach (XmlNode node in preferencesNode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "IsRevisionColumnVisible":
                            preferences.IsRevisionColumnVisible = bool.Parse(node.InnerText);
                            break;
                        case "IsAuthorColumnVisible":
                            preferences.IsAuthorColumnVisible = bool.Parse(node.InnerText);
                            break;
                        case "IsDateColumnVisible":
                            preferences.IsDateColumnVisible = bool.Parse(node.InnerText);
                            break;
                        case "IsActionsColumnVisible":
                            preferences.IsActionsColumnVisible = bool.Parse(node.InnerText);
                            break;
                        case "CCMailTo":
                            preferences.CCMailTo = node.InnerText;
                            break;
                        case "MailSubject":
                            preferences.MailSubject = node.InnerText;
                            break;
                        case "Signature":
                            preferences.Signature = node.InnerText;
                            break;
                        case "Domain":
                            preferences.Domain = node.InnerText;
                            break;

                        default: break;
                    }
                }
            }
            return preferences;
        }
        internal void SavePreferences(Preferences _Preferences)
        {
            string directory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(directory, @"Preferences.xml");
            if (!File.Exists(filePath))
            {
                XDocument xmlDoc = new XDocument(new XElement("Preferences",
                                                    new XElement("IsRevisionColumnVisible", _Preferences.IsRevisionColumnVisible ? "true" : "false"),
                                                    new XElement("IsAuthorColumnVisible", _Preferences.IsAuthorColumnVisible ? "true" : "false"),
                                                    new XElement("IsDateColumnVisible", _Preferences.IsDateColumnVisible ? "true" : "false"),
                                                    new XElement("IsActionsColumnVisible", _Preferences.IsActionsColumnVisible ? "true" : "false"),
                                                    new XElement("CCMailTo", _Preferences.CCMailTo),
                                                    new XElement("MailSubject", _Preferences.MailSubject),
                                                    new XElement("Signature", _Preferences.Signature),
                                                    new XElement("Domain", _Preferences.Domain)));
                xmlDoc.Save(filePath);
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode preferencesNode = xmlDoc.SelectSingleNode("//Preferences");
                foreach (XmlNode node in preferencesNode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "IsRevisionColumnVisible":
                            node.InnerText = _Preferences.IsRevisionColumnVisible ? "true" : "false";
                            break;
                        case "IsAuthorColumnVisible":
                            node.InnerText = _Preferences.IsAuthorColumnVisible ? "true" : "false";
                            break;
                        case "IsDateColumnVisible":
                            node.InnerText = _Preferences.IsDateColumnVisible ? "true" : "false";
                            break;
                        case "IsActionsColumnVisible":
                            node.InnerText = _Preferences.IsActionsColumnVisible ? "true" : "false";
                            break;
                        case "CCMailTo":
                            node.InnerText = _Preferences.CCMailTo;
                            break;
                        case "MailSubject":
                            node.InnerText = _Preferences.MailSubject;
                            break;
                        case "Signature":
                            node.InnerText = _Preferences.Signature;
                            break;
                        case "Domain":
                            node.InnerText = _Preferences.Domain;
                            break;

                        default: break;
                    }
                }
                xmlDoc.Save(filePath);
            }
        }

        #endregion
    }
}
