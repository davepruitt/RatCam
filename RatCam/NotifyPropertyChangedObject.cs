using System.ComponentModel;

namespace RatCam
{
    /// <summary>
    /// Base class for objects that want to use INotifyPropertyChanged
    /// </summary>
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}