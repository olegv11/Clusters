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
using Clusters.ViewModel;

namespace Clusters.Views
{
    /// <summary>
    /// Логика взаимодействия для DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : Window
    {
        public DatabaseView()
        {
            InitializeComponent();
            DataContext = new DatabaseViewModel();
        }
    }
}
