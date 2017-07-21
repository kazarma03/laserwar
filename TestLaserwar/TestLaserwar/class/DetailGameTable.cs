using System.ComponentModel;

namespace TestLaserwar
{
    /// <summary>
    /// Класс с полями для заполнения таблицы ИГР
    /// </summary>
    class DetailGameTable : INotifyPropertyChanged
    {

        public DetailGameTable(int Id, string CommandName, string GamerName, string Rating, string Accuracy, string Shots)
        {
            this.Id = Id;
            this.CommandName = CommandName;
            this.GamerName = GamerName;
            this.Rating = Rating;
            this.Accuracy = Accuracy;
            this.Shots = Shots;
        }
        // *************************** Таблица Детализация игры ***************************   \

        // ИД в таблице детализации игр
        public int _Id;
        /// <summary>
        /// ИД в таблице детализации игр
        /// </summary>
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        // Имя команды
        public string _CommandName;
        /// <summary>
        /// Имя команды
        /// </summary>
        public string CommandName
        {
            get
            {
                return _CommandName;
            }
            set
            {
                if (_CommandName != value)
                {
                    _CommandName = value;
                    OnPropertyChanged("CommandName");
                }
            }
        }

        // Имя игрока
        public string _GamerName;
        /// <summary>
        /// Имя игрока
        /// </summary>
        public string GamerName
        {
            get
            {
                return _GamerName;
            }
            set
            {
                if (_GamerName != value)
                {
                    _GamerName = value;
                    OnPropertyChanged("GamerName");
                }
            }
        }

        // Рейтинг
        public string _Rating;
        /// <summary>
        /// Рейтинг
        /// </summary>
        public string Rating
        {
            get
            {
                return _Rating;
            }
            set
            {
                if (_Rating != value)
                {
                    _Rating = value;
                    OnPropertyChanged("OnOfPercent");
                }
            }
        }

        // точность
        public string _Accuracy;
        /// <summary>
        /// точность
        /// </summary>
        public string Accuracy
        {
            get
            {
                return _Accuracy;
            }
            set
            {
                if (_Accuracy != value)
                {
                    _Accuracy = value;
                    OnPropertyChanged("Accuracy");
                }
            }
        }

        // количество выстрелов
        public string _Shots;
        /// <summary>
        /// количество выстрелов
        /// </summary>
        public string Shots
        {
            get
            {
                return _Shots;
            }
            set
            {
                if (_Shots != value)
                {
                    _Shots = value;
                    OnPropertyChanged("Shots");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // уведомления представления об изменениях свойств объекта.
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    /// <summary>
    /// Класс для привязки свойств лейбра в заголовке формы детализируемой игры
    /// </summary>
    class LabelDetailGameTable : INotifyPropertyChanged
    {
        // Имя детализируемой игры
        public string _DetailGameName;
        /// <summary>
        /// Имя детализируемой игры
        /// </summary>
        public string DetailGameName
        {
            get
            {
                return _DetailGameName;
            }
            set
            {
                if (_DetailGameName != value)
                {
                    _DetailGameName = value;
                    OnPropertyChanged("DetailGameName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // уведомления представления об изменениях свойств объекта.
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
