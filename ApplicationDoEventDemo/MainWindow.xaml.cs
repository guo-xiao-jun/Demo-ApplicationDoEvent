using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ApplicationDoEventDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDoEvent_Click(object sender, RoutedEventArgs e)
        {
            var isWorkComplete = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                isWorkComplete = true;
            });
            while (!isWorkComplete)
            {
                DoEvent();
            }
            MessageBox.Show("WorkComplete");
        }

        public void DoEvent()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        public object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }

        private void ProcessMsgs()
        {
            Action<object> cb = obj =>
            {
                var f = obj as DispatcherFrame;
                f.Continue = false;
            };

            var frame = new DispatcherFrame();
            Dispatcher.BeginInvoke(cb, DispatcherPriority.Background, frame);
            Dispatcher.PushFrame(frame);
        }
    }
}
