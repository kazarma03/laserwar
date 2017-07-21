using System.ComponentModel;


namespace TestLaserwar
{
    /// <summary>
    /// Класс с полями для заполнения таблицы ИГР
    /// </summary>
    class GameTable : INotifyPropertyChanged
    {
        public GameTable(int id_Game, string GameName, string GameDate, int QuantityGamers)
        {
            this.id_Game = id_Game;
            this.GameName = GameName;
            this.GameDate = GameDate;
            this.QuantityGamers = QuantityGamers;
        }

        // *************************** Таблица игры ***************************      

        // id игры
        public int _id_Game;
        /// <summary>
        /// id игры
        /// </summary>
        public int id_Game
        {
            get
            {
                return _id_Game;
            }
            set
            {
                if (_id_Game != value)
                {
                    _id_Game = value;
                    OnPropertyChanged("id_Game");
                }
            }
        }

        // Имя игры
        public string _GameName;
        /// <summary>
        /// Имя игры
        /// </summary>
        public string GameName
        {
            get
            {
                return _GameName;
            }
            set
            {
                if (_GameName != value)
                {
                    _GameName = value;
                    OnPropertyChanged("GameName");
                }
            }
        }

        // Дата игры
        public string _GameDate;
        /// <summary>
        /// Дата игры
        /// </summary>
        public string GameDate
        {
            get
            {
                return _GameDate;
            }
            set
            {
                if (_GameDate != value)
                {
                    _GameDate = value;
                    OnPropertyChanged("GameDate");
                }
            }
        }

        // Количество игроков
        public int _QuantityGamers;
        /// <summary>
        /// Количество игроков
        /// </summary>
        public int QuantityGamers
        {
            get
            {
                return _QuantityGamers;
            }
            set
            {
                if (_QuantityGamers != value)
                {
                    _QuantityGamers = value;
                    OnPropertyChanged("URL");
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
