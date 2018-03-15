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

namespace RatCam
{
    /// <summary>
    /// Interaction logic for CameraSelector.xaml
    /// </summary>
    public partial class CameraSelector : Window
    {
        public CameraSelector()
        {
            InitializeComponent();

            //Set the data context
            this.DataContext = CameraSelectorViewModel.GetInstance();
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            CameraSelectorViewModel vm = this.DataContext as CameraSelectorViewModel;
            if (vm != null)
            {
                vm.ResultOK = true;
            }

            this.Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            CameraSelectorViewModel vm = this.DataContext as CameraSelectorViewModel;
            if (vm != null)
            {
                vm.RefreshCameraList();
            }
        }
    }
}
