using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCam
{
    /// <summary>
    /// Class for config stuff
    /// </summary>
    public class RatCamConfiguration
    {
        #region Singleton

        private static RatCamConfiguration _instance = null;
        private static Object _instance_lock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        private RatCamConfiguration()
        {
            this.ReadConfigurationFile();
            this.ReadBoothPairings();
        }

        /// <summary>
        /// Gets the one and only instance of this class that is allowed to exist.
        /// </summary>
        /// <returns>Instance of Microcontroller class</returns>
        public static RatCamConfiguration GetInstance()
        {
            if (_instance == null)
            {
                lock (_instance_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RatCamConfiguration();
                    }
                }
            }

            return _instance;
        }

        #endregion

        #region Privata data members

        private string ConfigurationFileName = "ratcam.config";
        private string BoothPairingsFileName = "ratcam_booths.config";

        #endregion

        #region Public data members

        public Dictionary<string, string> BoothPairings = new Dictionary<string, string>();
        public string SavePath = string.Empty;
        public int RecordingDuration = 180;
        public bool AdministratorMode = false;
        public string RatName = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// Reads the MotoTrak configuration file and populates properties of this class accordingly.
        /// </summary>
        public void ReadConfigurationFile ()
        {
            //Check to see if the desired configuration file exists
            FileInfo file_info = new FileInfo(ConfigurationFileName);
            if (file_info.Exists)
            {
                //Create a stream reader object
                StreamReader reader = null;

                try
                {
                    //Try to open a handle to the configuration file
                    reader = new StreamReader(ConfigurationFileName);
                }
                catch (Exception e)
                {
                    //Log the exception that occurred
                    System.Console.WriteLine("Unable to read configuration file!");

                    //Exit the function
                    return;
                }

                //Create a variable that indicates whether we have discovered the file version yet
                bool version_found = false;
                int file_version = 0;

                //Continue loading the configuration file
                while (!reader.EndOfStream)
                {
                    //Read in a line from the configuration file
                    string input_string = reader.ReadLine();

                    //Parse away any comments that are at the end of the line
                    string[] whole_line_parts = input_string.Split(new char[] { '%' }, 2);
                    string line_without_comments = whole_line_parts[0].Trim();

                    //If this line was just an empty line, or a line full of comments, skip it
                    if (string.IsNullOrEmpty(line_without_comments))
                    {
                        continue;
                    }

                    //Check to see if the file version has been found
                    if (!version_found)
                    {
                        //The file version MUST be the first thing in the file (other than comments).
                        //If anything comes before the file version, it will be ignored.
                        string[] parameter_string_parts = line_without_comments.Split(new char[] { ':' }, 2);
                        string parameter = parameter_string_parts[0].Trim();
                        if (parameter.Equals("Version"))
                        {
                            bool success = Int32.TryParse(parameter_string_parts[1].Trim(), out file_version);
                            if (!success)
                            {
                                System.Console.WriteLine("Unable to read config file version!");
                                return;
                            }
                            else
                            {
                                version_found = true;
                                if (file_version != 1)
                                {
                                    System.Console.WriteLine("Configuration file has incompatible version!");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        //At this point, we have found the file version, so we can read in parameters
                        string[] parameter_string_parts = line_without_comments.Split(new char[] { ':' }, 2);
                        string parameter = parameter_string_parts[0].Trim();

                        //Check the parameter and read it in
                        if (parameter.Equals("Save Path"))
                        {
                            SavePath = parameter_string_parts[1].Trim();

                            //Make sure the save path ends with a slash
                            if (!SavePath.EndsWith(@"\"))
                            {
                                SavePath += @"\";
                            }

                            //Make the directory if it doesn't exist
                            DirectoryInfo dir_info = new DirectoryInfo(SavePath);
                            if (!dir_info.Exists)
                            {
                                dir_info.Create();
                            }
                        }
                        else if (parameter.Equals("Recording Duration"))
                        {
                            int dur = 0;
                            bool success = Int32.TryParse(parameter_string_parts[1].Trim(), out dur);
                            if (success)
                            {
                                RecordingDuration = dur;
                            }
                        }
                        else if (parameter.Equals("Administrator Mode"))
                        {
                            if (parameter_string_parts[1].Trim().Equals("True", StringComparison.InvariantCultureIgnoreCase))
                            {
                                AdministratorMode = true;
                            }
                            else
                            {
                                AdministratorMode = false;
                            }
                        }
                    }
                }
            }
            else
            {
                System.Console.WriteLine("Unable to find configuration file!");
            }
        }

        /// <summary>
        /// Saves the booth pairings to a file
        /// </summary>
        public void SaveBoothPairings()
        {
            try
            {
                //Open a stream to write to the file
                StreamWriter writer = new StreamWriter(BoothPairingsFileName);

                //Write each booth pairing to the file
                foreach (var kvp in BoothPairings)
                {
                    if (!string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                    {
                        writer.WriteLine(kvp.Key + ", " + kvp.Value);
                    }
                }

                //Close the file handle
                writer.Close();
            }
            catch
            {
                System.Console.WriteLine("Unable to save booth pairings!");
            }
        }

        /// <summary>
        /// Reads the booth pairings file
        /// </summary>
        public void ReadBoothPairings()
        {
            //Open a stream to read the booth pairings configuration file
            try
            {
                StreamReader reader = new StreamReader(BoothPairingsFileName);

                //Read all the lines from the file
                List<string> lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }

                //Close the stream
                reader.Close();

                //Now parse the input
                for (int i = 0; i < lines.Count; i++)
                {
                    string thisLine = lines[i];
                    string[] splitString = thisLine.Split(new char[] { ',' }, 2);

                    string booth_name = splitString[0].Trim();
                    string com_port = splitString[1].Trim();

                    //Add the booth pairing to our dictionary
                    BoothPairings.Add(booth_name, com_port);
                }
            }
            catch
            {
                System.Console.WriteLine("Unable to read booth pairings!");
            }

        }

        #endregion
    }
}
