using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCam
{
    /// <summary>
    /// Class that handles camera view stuff
    /// </summary>
    public class CameraViewModel : NotifyPropertyChangedObject
    {
        #region Private data members

        private Camera _model = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CameraViewModel(Camera model)
        {
            _model = model;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The model camera
        /// </summary>
        public Camera ModelCamera
        {
            get
            {
                return _model;
            }
            private set
            {
                _model = value;
            }
        }

        /// <summary>
        /// The booth for this camera
        /// </summary>
        public string BoothName
        {
            get
            {
                if (!string.IsNullOrEmpty(_model.BoothName))
                    return "Booth " + _model.BoothName;
                return "Unknown booth";
            }
            set
            {
                _model.BoothName = value;
                NotifyPropertyChanged("BoothName");
            }
        }

        /// <summary>
        /// The device information for this camera
        /// </summary>
        public string DeviceInformation
        {
            get
            {
                if (_model != null && _model.CameraInfo != null)
                {
                    return _model.CameraInfo.Name;
                }

                return string.Empty;
            }
        }

        #endregion
    }
}
