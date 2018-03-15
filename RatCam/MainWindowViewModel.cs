using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RatCam
{
    /// <summary>
    /// View model class for the main window
    /// </summary>
    public class MainWindowViewModel : NotifyPropertyChangedObject
    {
        #region Private data members

        private RatCamConfiguration configuration = RatCamConfiguration.GetInstance();
        private string camera_moniker_string = string.Empty;
        private bool is_recording = false;
        private bool recording_started = false;
        private VideoFileWriter writer = new VideoFileWriter();
        private DateTime time_of_video_start = DateTime.MinValue;
        private string save_file_name = string.Empty;
        private bool rat_name_requires_editing = true;

        #endregion

        #region Constructors

        public MainWindowViewModel (string cms)
        {
            camera_moniker_string = cms;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The booth associated with this camera
        /// </summary>
        public string BoothName
        {
            get
            {
                return configuration.BoothPairings.Where(x => x.Value.Equals(camera_moniker_string)).Select(x => x.Key).FirstOrDefault();
            }
            set
            {
                string new_name = value;
                string old_name = configuration.BoothPairings.Where(x => x.Value.Equals(camera_moniker_string)).Select(x => x.Key).FirstOrDefault();

                new_name = ViewHelperMethods.CleanInput(new_name).ToUpper();

                if (!string.IsNullOrEmpty(old_name))
                {
                    if (configuration.BoothPairings.ContainsKey(old_name))
                    {
                        configuration.BoothPairings.Remove(old_name);
                    }
                }
                
                configuration.BoothPairings.Add(new_name, camera_moniker_string);
                configuration.SaveBoothPairings();

                NotifyPropertyChanged("BoothName");
            }
        }

        /// <summary>
        /// The rat name being used
        /// </summary>
        public string RatName
        {
            get
            {
                return configuration.RatName;
            }
            set
            {
                configuration.RatName = value;
                configuration.RatName = ViewHelperMethods.CleanInput(configuration.RatName).ToUpper();

                if (!string.IsNullOrEmpty(configuration.RatName))
                    rat_name_requires_editing = false;

                NotifyPropertyChanged("RatName");
                NotifyPropertyChanged("StartButtonEnabled");
                NotifyPropertyChanged("StartButtonColor");
            }
        }

        /// <summary>
        /// Whether to display certain controls only available in admin mode
        /// </summary>
        public Visibility AdminModeVisibility
        {
            get
            {
                if (configuration.AdministratorMode)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// What to display on the start button
        /// </summary>
        public string StartButtonContent
        {
            get
            {
                if (is_recording)
                {
                    return "Stop";
                }
                else
                {
                    return "Start";
                }
            }
        }

        /// <summary>
        /// The color of the text on the start button
        /// </summary>
        public SolidColorBrush StartButtonColor
        {
            get
            {
                if (is_recording)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    if (StartButtonEnabled)
                    {
                        return new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Gray);
                    }
                }
            }
        }

        /// <summary>
        /// The visibility of the camera
        /// </summary>
        public Visibility CameraVisibility
        {
            get
            {
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Whether or not we are currently recording video
        /// </summary>
        public bool IsRecording
        {
            get
            {
                return is_recording;
            }
        }

        /// <summary>
        /// Whether or not to enable the start button
        /// </summary>
        public bool StartButtonEnabled
        {
            get
            {
                if (rat_name_requires_editing || string.IsNullOrEmpty(configuration.RatName))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion

        #region Methods

        public void StartRecording (VideoCaptureDevice camera)
        {
            //Set the recording started flag to false
            recording_started = false;

            //Determine the file name of the video file to save
            save_file_name = configuration.RatName + "_" + DateTime.Now.ToString("YYYYmmDD_HHMMSS") + ".mp4";

            //Join the path and file, and put the file in a sub-folder for the specific rat
            save_file_name = configuration.SavePath + configuration.RatName + @"\" + save_file_name;

            //Make sure the full directory exists if it doesn't
            FileInfo file_info = new FileInfo(save_file_name);
            if (!file_info.Directory.Exists)
            {
                file_info.Directory.Create();
            }

            //Set the is_recording flag
            is_recording = true;

            //Subscribe to new frame events
            camera.NewFrame += Camera_NewFrame;
            
            //Notify changes on some properties
            NotifyPropertyChanged("IsRecording");
            NotifyPropertyChanged("StartButtonContent");
            NotifyPropertyChanged("StartButtonColor");
        }

        public void StopRecording (VideoCaptureDevice camera)
        {
            //Unsubscribe from the event
            camera.NewFrame -= Camera_NewFrame;

            //Close the video writer
            if (writer.IsOpen)
            {
                writer.Close();
            }

            //Set the is recording flag
            is_recording = false;

            //Indicate that the rat name now needs editing
            rat_name_requires_editing = true;

            //Notify changes on some properties
            NotifyPropertyChanged("IsRecording");
            NotifyPropertyChanged("StartButtonContent");
            NotifyPropertyChanged("StartButtonColor");
            NotifyPropertyChanged("StartButtonEnabled");
        }

        private void Camera_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap new_frame = (Bitmap)eventArgs.Frame.Clone();

            if (!recording_started)
            {
                writer.Open(save_file_name, new_frame.Width, new_frame.Height);
                recording_started = true;
                time_of_video_start = DateTime.Now;
            }
            else if (writer.IsOpen)
            {
                DateTime current_time = DateTime.Now;
                TimeSpan time_since_video_started = current_time.Subtract(time_of_video_start);

                if (time_since_video_started.TotalSeconds >= configuration.RecordingDuration)
                {
                    //Close down the writer
                    writer.Close();

                    //Unsubscribe from new frame events
                    VideoCaptureDevice camera = sender as VideoCaptureDevice;
                    if (camera != null)
                    {
                        camera.NewFrame -= Camera_NewFrame;
                    }

                    //Set the is recording flag
                    is_recording = false;

                    //Indicate that the rat name now needs editing
                    rat_name_requires_editing = true;
                    
                    //Notify changes on some properties
                    NotifyPropertyChanged("IsRecording");
                    NotifyPropertyChanged("StartButtonContent");
                    NotifyPropertyChanged("StartButtonColor");
                    NotifyPropertyChanged("StartButtonEnabled");
                }
                else
                {
                    try
                    {
                        writer.WriteVideoFrame(new_frame, time_since_video_started);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Unable to save video frame!");
                    }
                }
            }
        }

        #endregion
    }
}
