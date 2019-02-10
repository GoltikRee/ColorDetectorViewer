using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace ColorDetectorViewer
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SerialPort _currentPort = null;
        private delegate void UpdateDelegate(byte[] values);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateColor()
        {
            while (_currentPort.IsOpen)
            {
                if (!_currentPort.IsOpen) return;
                try 
                {
                    //_currentPort.DiscardInBuffer();
                    string strFromPort = _currentPort.ReadLine();
                    var values = strFromPort
                        .Replace("$#$", "")
                        .Replace("@!@", "")
                        .Replace("\r", "")
                        .Split(',')
                        .Select(x => Convert.ToByte(Convert.ToInt32(x))).ToArray();

                    ColorCanvas.Dispatcher.BeginInvoke(new UpdateDelegate(UpdateCanvas), values);
                }
                catch (Exception exception)
                {
                    throw exception; //TODO: Implement exception handling
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _currentPort = new SerialPort("COM5", 9600);
            _currentPort.Open();
            Thread receiverThread = new Thread(UpdateColor);
            receiverThread.Start();
        }

        private void UpdateCanvas(byte[] values)
        {
            ColorCanvas.R = values[0];
            ColorCanvas.G = values[1];
            ColorCanvas.B = values[2];
        }
    }
}
