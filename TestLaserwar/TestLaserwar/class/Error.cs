using System.ComponentModel;

namespace TestLaserwar
{
    class Error : INotifyPropertyChanged
    {
        //Сообщение об ошибке
        public string _ErrorMessage;
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                if (_ErrorMessage != value)
                {
                    _ErrorMessage = value;
                    OnPropertyChanged("ErrorMessage");
                }
            }
        }

     
        public event PropertyChangedEventHandler PropertyChanged;

        // уведомления представления об изменениях свойств объекта
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
