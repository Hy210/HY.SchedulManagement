﻿
using System.Windows;
using System.Windows.Input;


namespace HY.SchedulManagement
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            MouseLeftButtonDown += Handler_DragMove;
            Loaded += Handler_Loaded;
        }

        private void Handler_DragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Handler_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
