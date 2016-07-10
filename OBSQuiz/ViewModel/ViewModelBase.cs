using System.ComponentModel;

namespace OBSQuiz.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        protected bool CheckPropertyChanged<T>(string propertyName, ref T oldValue, ref T newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if ((oldValue == null && newValue != null) || !oldValue.Equals((T)newValue))
            {
                oldValue = newValue;
                FirePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void FirePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
