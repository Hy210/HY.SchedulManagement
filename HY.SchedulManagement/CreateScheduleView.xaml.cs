using System;
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

namespace HY.SchedulManagement
{
    /// <summary>
    /// ScheduleWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public CreateScheduleViewModel ViewModel => DataContext as CreateScheduleViewModel;
        public ScheduleWindow()
        {
            InitializeComponent();
            DataContext = new CreateScheduleViewModel();
            Loaded += Handler_Loaded;
        }

        private void Handler_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
