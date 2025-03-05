using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Wpf.Views;
using Nito.AsyncEx;

namespace ItemsProject.Wpf.Views
{
    /// <summary>
    /// Interaction logic for LoadListCollectionView.xaml
    /// </summary>
    public partial class LoadListCollectionView : MvxWpfView
    {
        public LoadListCollectionView()
        {
            InitializeComponent();
        }

    }
}
