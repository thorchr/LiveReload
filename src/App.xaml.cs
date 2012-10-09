﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace LiveReload
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow window;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            NodeRPC nodeFoo = new NodeRPC(Dispatcher.CurrentDispatcher);
            nodeFoo.RaiseNodeLineEvent += HandleNodeLineEvent;

            window = new MainWindow();
            window.Show();

            TrayIconController trayIcon = new TrayIconController();
            //trayIcon.MainWindowHideEvent += HandleMainWindowShowEvent;
            trayIcon.MainWindowShowEvent += HandleMainWindowShowEvent;
            trayIcon.MainWindowToggleEvent  += HandleMainWindowToggleEvent;
        }

        void HandleNodeLineEvent(string nodeLine)
        {
            JArray a = JArray.Parse(nodeLine);
            window.DisplayNodeResult(nodeLine);
            Console.WriteLine(a.ToString());
            if (a.First.ToString() == "update")
            {
                JArray treeData = (JArray)a[1]["projects"];
                window.updateTreeView(treeData);
            }
        }

        void HandleMainWindowHideEvent()
        {
            window.Hide();
        }
        void HandleMainWindowShowEvent()
        {
            window.Show();
        }
        void HandleMainWindowToggleEvent()
        {
            if (window.IsVisible)
            {
                window.Hide();
            }
            else
            {
                window.Show();
            }
        }
    }
}