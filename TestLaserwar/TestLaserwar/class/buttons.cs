using System.ComponentModel;

namespace TestLaserwar
{
    /// <summary>
    /// класс с полями для управления повидением кнопок
    /// </summary>
    class buttons : INotifyPropertyChanged
    {
        //активность кнопки загрузки JSON объекта
        public bool _OnOfDownloadJsonButtton;
        /// <summary>
        /// активность кнопки загрузки JSON объекта
        /// </summary>
        public bool OnOfDownloadJsonButtton
        {
            get
            {
                return _OnOfDownloadJsonButtton;
            }
            set
            {
                if (_OnOfDownloadJsonButtton != value)
                {
                    _OnOfDownloadJsonButtton = value;
                    OnPropertyChanged("OnOfDownloadJsonButtton");
                }
            }
        }

        //активность кнопки перехода на форму загрузки
        public bool _OnOfButtonDownLoad;
        /// <summary>
        /// активность кнопки перехода на форму загрузки
        /// </summary>
        public bool OnOfButtonDownLoad
        {
            get
            {
                return _OnOfButtonDownLoad;
            }
            set
            {
                if (_OnOfButtonDownLoad != value)
                {
                    _OnOfButtonDownLoad = value;
                    OnPropertyChanged("OnOfButtonDownLoad");
                }
            }
        }

        //фон кнопки перехода на форму загрузки
        public bool _ClickButtonDownLoad;
        /// <summary>
        /// фон кнопки перехода на форму загрузки
        /// </summary>
        public bool ClickButtonDownLoad
        {
            get
            {
                return _ClickButtonDownLoad;
            }
            set
            {
                if (_ClickButtonDownLoad != value)
                {
                    _ClickButtonDownLoad = value;
                    OnPropertyChanged("ClickButtonDownLoad");
                }
            }
        }

        //активность кнопки перехода на форму звуки
        public bool _OnOfButtonSound;
        /// <summary>
        /// активность кнопки перехода на форму звуки
        /// </summary>
        public bool OnOfButtonSound
        {
            get
            {
                return _OnOfButtonSound;
            }
            set
            {
                if (_OnOfButtonSound != value)
                {
                    _OnOfButtonSound = value;
                    OnPropertyChanged("OnOfButtonSound");
                }
            }
        }

        //фон кнопки перехода на форму звуки
        public bool _ClickButtonSound;
        /// <summary>
        /// фон кнопки перехода на форму звуки
        /// </summary>
        public bool ClickButtonSound
        {
            get
            {
                return _ClickButtonSound;
            }
            set
            {
                if (_ClickButtonSound != value)
                {
                    _ClickButtonSound = value;
                    OnPropertyChanged("ClickButtonSound");
                }
            }
        }

        // активность кнопки перехода на форму игры
        public bool _OnOfButtonGame;
        /// <summary>
        /// активность кнопки перехода на форму игры
        /// </summary>
        public bool OnOfButtonGame
        {
            get
            {
                return _OnOfButtonGame;
            }
            set
            {
                if (_OnOfButtonGame != value)
                {
                    _OnOfButtonGame = value;
                    OnPropertyChanged("OnOfButtonGame");
                }
            }
        }

        // фон кнопки перехода на форму игры
        public bool _ClickButtonGame;
        /// <summary>
        /// фон кнопки перехода на форму игры
        /// </summary>
        public bool ClickButtonGame
        {
            get
            {
                return _ClickButtonGame;
            }
            set
            {
                if (_ClickButtonGame != value)
                {
                    _ClickButtonGame = value;
                    OnPropertyChanged("ClickButtonGame");
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
