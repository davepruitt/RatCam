using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace RatCam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Display the window
            CameraSelector camera_selector = new CameraSelector();
            camera_selector.ShowDialog();

            //Get the result of the dialog
            CameraSelectorViewModel selector_result = CameraSelectorViewModel.GetInstance();
            if (selector_result.ResultOK)
            {
                //Start playing from the webcam
                string moniker_string = selector_result.AvailableCameras[selector_result.SelectedCameraIndex].ModelCamera.CameraInfo.MonikerString;
                var camera = new VideoCaptureDevice(moniker_string);
                CameraVideoSourcePlayer.VideoSource = camera;
                CameraVideoSourcePlayer.Start();

                //Set the data context of the main window
                DataContext = new MainWindowViewModel(moniker_string);
            }
            else
            {
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Stop the camera from playing
            CameraVideoSourcePlayer.Stop();

            //Get the camera object
            var camera = CameraVideoSourcePlayer.VideoSource as VideoCaptureDevice;

            //Stop saving frames to the file
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            if (vm != null)
            {
                vm.StopRecording(camera);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var camera = CameraVideoSourcePlayer.VideoSource as VideoCaptureDevice;

            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            if (vm != null)
            {
                if (vm.IsRecording)
                {
                    vm.StopRecording(camera);
                }
                else
                {
                    vm.StartRecording(camera);
                }
            }
        }
    }
}
