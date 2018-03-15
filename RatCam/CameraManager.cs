using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCam
{
    public class CameraManager
    {
        #region Singleton

        private static CameraManager _instance = null;
        private static Object _instance_lock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        private CameraManager()
        {
            //constructor is private
        }

        /// <summary>
        /// Gets the one and only instance of this class that is allowed to exist.
        /// </summary>
        /// <returns>Instance of Microcontroller class</returns>
        public static CameraManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_instance_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CameraManager();
                    }
                }
            }

            return _instance;
        }

        #endregion

        #region Public methods

        public List<FilterInfo> RetrieveAvailableCameras ()
        {
            //Retrieve all video capture devices
            FilterInfoCollection j = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            List<FilterInfo> result = j.Cast<FilterInfo>().ToList();

            return result;
        }

        #endregion
    }
}
