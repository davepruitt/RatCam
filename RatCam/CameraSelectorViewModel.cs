using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCam
{
    /// <summary>
    /// A view-model that handles the camera selector
    /// </summary>
    public class CameraSelectorViewModel : NotifyPropertyChangedObject
    {
        #region Private data members

        List<Camera> _camera_list = new List<Camera>();
        private int _selected_camera_index = 0;
        private bool _result_ok = false;

        #endregion

        #region Constructor

        #region Constructors - This is a singleton class

        private static CameraSelectorViewModel _instance = null;

        /// <summary>
        /// </summary>
        private CameraSelectorViewModel()
        {
            RefreshCameraList();
        }

        /// <summary>
        /// Gets the one and only instance of this class that is allowed to exist.
        /// </summary>
        /// <returns>Instance of ArdyMotorBoard class</returns>
        public static CameraSelectorViewModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CameraSelectorViewModel();
            }

            return _instance;
        }


        #endregion

        #endregion

        #region Public properties

        /// <summary>
        /// List of all available cameras connected to the system
        /// </summary>
        public List<CameraViewModel> AvailableCameras
        {
            get
            {
                List<CameraViewModel> k = _camera_list.Select(x => new CameraViewModel(x)).ToList();
                k.Sort((x, y) => x.BoothName.CompareTo(y.BoothName));
                return k;
            }
        }

        /// <summary>
        /// The camera that the user has selected
        /// </summary>
        public int SelectedCameraIndex
        {
            get
            {
                return _selected_camera_index;
            }
            set
            {
                _selected_camera_index = value;
                NotifyPropertyChanged("SelectedCameraIndex");
            }
        }

        /// <summary>
        /// The result of the camera selector window
        /// </summary>
        public bool ResultOK
        {
            get
            {
                return _result_ok;
            }
            set
            {
                _result_ok = value;
                NotifyPropertyChanged("ResultOK");
            }
        }

        #endregion

        #region Methods

        public void RefreshCameraList()
        {
            //Get the booth pairings
            RatCamConfiguration configuration = RatCamConfiguration.GetInstance();

            //Populate the list of cameras
            List<FilterInfo> c = CameraManager.GetInstance().RetrieveAvailableCameras();
            List<Camera> c2 = new List<Camera>();
            foreach (var k in c)
            {
                Camera k2 = new Camera();
                k2.CameraInfo = k;
                k2.BoothName = configuration.BoothPairings.Where(x => x.Value.Equals(k.MonikerString)).Select(x => x.Key).FirstOrDefault();
                c2.Add(k2);
            }

            _camera_list = c2;
        }

        #endregion
    }
}
