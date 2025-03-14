﻿using System;
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
using MvvmCross.Platforms.Wpf.Views;

namespace ItemsProject.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SuccessNotificationView.xaml
    /// </summary>
    public partial class SuccessNotificationView : MvxWindow
    {
        public SuccessNotificationView()
        {
            InitializeComponent();
            var desktopArea = System.Windows.SystemParameters.WorkArea;
            Left = desktopArea.Left + 15;
            Top = desktopArea.Top + 15;
        }
    }
}
