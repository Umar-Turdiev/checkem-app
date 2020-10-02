﻿using ProjectSC.Models.DataAccess;
using ProjectSC.Models.Object.Notification;
using ProjectSC.ViewModels.Notification;
using ProjectSC.Views;
using ProjectSC.Views.Settings;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProjectSC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Variables
        private DataAccess_Json dataAccess = new DataAccess_Json();

        private ToDoList_View todoList_View = new ToDoList_View();

        private List<TimeRecord> timeRecord = new List<TimeRecord>();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int TimerOffset = 60 - DateTime.Now.Second;

        private int NavbarCursorIndex = 0;
        #endregion

        #region Propertes
        public int FilterMode { get; set; }
        #endregion

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();

            GridPrincipal.Children.Add(todoList_View);
            todoList_View.ListFilter(0);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerOffset = 60 - DateTime.Now.Second;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(TimerOffset);//Fix timer delay time

            dataAccess.RetrieveTimeData(ref timeRecord);


            foreach (var item in timeRecord)
            {
                if (DateTime.Now.Year == item.BeginDateTime.Year && DateTime.Now.Month == item.BeginDateTime.Month && DateTime.Now.Hour == item.BeginDateTime.Hour && DateTime.Now.Minute == item.BeginDateTime.Minute)
                {
                    NotificationController.Notify(item.Title, NotificationController.RandomMessage("begin"));
                }
                if (DateTime.Now.Year == item.EndDateTime.Year && DateTime.Now.Month == item.EndDateTime.Month && DateTime.Now.Hour == item.EndDateTime.Hour && DateTime.Now.Minute == item.EndDateTime.Minute)
                {
                    NotificationController.Notify(item.Title, NotificationController.RandomMessage("end"));
                }
            }//Check if the begin or end time is matched with the current time
        }

        private void ButtonToDoList_Click(object sender, RoutedEventArgs e)
        {
            this.FilterMode = 0;

            ChangeFIlter(this.FilterMode);
        }
        private void ButtonDueDateFilter_Click(object sender, RoutedEventArgs e)
        {
            this.FilterMode = 1;

            ChangeFIlter(this.FilterMode);
        }

        private void ButtonStarredFilter_Click(object sender, RoutedEventArgs e)
        {
            this.FilterMode = 2;

            ChangeFIlter(this.FilterMode);
        }

        private void ButtonUserPref_Click(object sender, RoutedEventArgs e)
        {
            GridPrincipal.Children.Clear();

            SettingsMenu settingsMenu = new SettingsMenu(this);

            GridPrincipal.Children.Add(settingsMenu);
        }

        private void MoveNavbarCursor(int index)
        {
            NavbarCursorIndex = index;
            Grid.SetRow(NavbarCursor, index);
        }

        private void ButtonColorTester1_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }

        private void ButtonColorTester2_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PrimaryColor = System.Drawing.Color.FromArgb(255, 218, 30, 99);
            Properties.Settings.Default.SecondaryColor = System.Drawing.Color.FromArgb(255, 117, 58, 136);

            todoList_View.ListFilter(NavbarCursorIndex);

            Properties.Settings.Default.Save();
        }

        public void ChangeFIlter(int mode)
        {
            MoveNavbarCursor(mode);
            todoList_View.ListFilter(mode);

            todoList_View.CloseDetailsPanel();
        }
    }
}
