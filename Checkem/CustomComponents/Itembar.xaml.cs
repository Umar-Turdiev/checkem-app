﻿using Checkem.Assets.ColorConverters;
using Cyclops.Models.Objects;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Visual;

namespace Checkem.CustomComponents
{
    public partial class Itembar : UserControl, INotifyPropertyChanged
    {
        public Itembar(ToDoItem item)
        {
            DataContext = this;

            ItemProperties = item;

            InitializeComponent();
        }

        #region Properties

        private ToDoItem ItemProperties;

        public string Title
        {
            get
            {
                return ItemProperties.Title;
            }
            set
            {
                if (ItemProperties.Title != value)
                {
                    ItemProperties.Title = value;

                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Variables

        Binding ItemCompletedTextColorBindings = new Binding()
        {
            Source = Properties.Settings.Default,
            Path = new PropertyPath("ItemCompletedTextColor", Properties.Settings.Default),
            Converter = new ColorToBrushConverter()
        };

        Binding NormalTextColorBindings = new Binding()
        {
            Source = Properties.Settings.Default,
            Path = new PropertyPath("NormalTextColor", Properties.Settings.Default),
            Converter = new ColorToBrushConverter()
        };

        Binding OverDueTextColorBindings = new Binding()
        {
            Source = Properties.Settings.Default,
            Path = new PropertyPath("OverDueTextColor", Properties.Settings.Default),
            Converter = new ColorToBrushConverter()
        };

        Binding ItembarHighlightColorBindings = new Binding()
        {
            Source = Properties.Settings.Default,
            Path = new PropertyPath("ItembarHighlightColor", Properties.Settings.Default),
            Converter = new ColorToBrushConverter()
        };

        Binding ControlColorBindings = new Binding()
        {
            Source = Properties.Settings.Default,
            Path = new PropertyPath("ControlColor", Properties.Settings.Default),
            Converter = new ColorToBrushConverter()
        };

        PackIcon icon = new PackIcon();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void Itembar_Loaded(object sender, RoutedEventArgs e)
        {
            VisualUpdate();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void VisualUpdate()
        {
            if (ItemProperties.IsReminderOn)
            {
                this.Height = 70;

                ReminderIcon.Visibility = Visibility.Visible;
                ReminderDetailTextBlock.Visibility = Visibility.Visible;

                if (this.ItemProperties.IsAdvanceReminderOn)
                {
                    ReminderDetailTextBlock.Text = "Start on: " + DateTimeManipulator.SimplifiedDate(this.ItemProperties.BeginDateTime.Value) + "\tEnd on: " + DateTimeManipulator.SimplifiedDate(this.ItemProperties.EndDateTime.Value);
                }
                else
                {
                    ReminderDetailTextBlock.Text = DateTimeManipulator.SimplifiedDate(this.ItemProperties.EndDateTime.Value);
                }

                if (DateTimeManipulator.IsPassed(this.ItemProperties.EndDateTime.Value))
                {
                    ReminderDetailTextBlock.SetBinding(TextBlock.ForegroundProperty, OverDueTextColorBindings);
                }
                else
                {
                    ReminderDetailTextBlock.SetBinding(TextBlock.ForegroundProperty, NormalTextColorBindings);
                }
            }
            else
            {
                this.Height = 50;

                ReminderIcon.Visibility = Visibility.Hidden;
                ReminderDetailTextBlock.Visibility = Visibility.Hidden;
            }
        }

        private void ItembarBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ItembarBorder.SetBinding(Border.BackgroundProperty, ItembarHighlightColorBindings);
        }

        private void ItembarBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ItembarBorder.SetBinding(Border.BackgroundProperty, ControlColorBindings);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Title = "binding works!!";
        }
    }
}
