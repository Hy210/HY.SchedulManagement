using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HY.SchedulManagement
{
    public class WidowBehavior : Behavior<Button>
    {
        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }
        public WidowBehavior()
        {
         
        }
        public static readonly DependencyProperty ButtonCommandProperty =
           DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(WidowBehavior), new PropertyMetadata(null));
        protected override void OnAttached()
        {
            AssociatedObject.MouseDoubleClick += Handler_DoubleClick;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseDoubleClick -= Handler_DoubleClick;
        }

        private void Handler_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                var window = new ScheduleWindow();
                window.ShowDialog();
            }));
        }
    }
   


}
