using System.ComponentModel;
using System.Windows;

namespace TestLaserwar
{
    /// <summary>
    /// Класс с полями для заполнения таблицы Звуковых файлов
    /// </summary>
    class SoundTable : INotifyPropertyChanged
    {
        /// <summary>
        /// Метод для удобного первоначального заполнения таблицы
        /// </summary>
        /// <param name="Name">Имя звукового файла</param>
        /// <param name="Size">Размер звукового файла</param>
        /// <param name="URL">URL звукового файла (скрытый столбец, что бы не делать дополнительных запросов в бд)</param>
        /// <param name="OnOfDowlLoad">статус активности кнопки загрузки звуковых файлов</param>
        /// <param name="OnOfStateProgress">статус отображения(видимости) лейбла "Файл загружен"</param>
        /// <param name="OnOfDownloadProgress">статус отображения(видимости) прогрессора загрузки звуковых файлов</param>
        /// <param name="OnOfPercent">статус отображения(видимости) процентов и скорости загрузки звуковых файлов</param>
        /// <param name="DownloadPathIcon">путь иконки отображающейся на кнопке загрузки звугового файла</param>
        /// <param name="OnOfTimePlay">статус отображения(видимости) времени проигрывания звукового файла</param>
        /// <param name="OnOfPlayProgress"статус отображения(видимости) прогрессора проигрывания звукового файла></param>
        /// <param name="PlayProgress">значение прогрессора проигрывания звукового файла</param>
        /// <param name="PlayPathIcon">путь иконки отображающейся на кнопке проигрывания звукового файла</param>
        public SoundTable(string Name, int Size, string URL, bool OnOfDowlLoad, Visibility OnOfStateProgress, Visibility OnOfDownloadProgress, Visibility OnOfPercent, string DownloadPathIcon, Visibility OnOfTimePlay, Visibility OnOfPlayProgress, double PlayProgress, string PlayPathIcon)
        {
            this.Name = Name;
            this.Size = Size;
            this.URL = URL;
            this.OnOfDowlLoad = OnOfDowlLoad;

            this.DownloadPathIcon = DownloadPathIcon;

            this.OnOfStateProgress = OnOfStateProgress;
            this.OnOfPercent = OnOfPercent;
            this.OnOfDownloadProgress = OnOfDownloadProgress;

            this.PlayPathIcon = PlayPathIcon;

            this.OnOfTimePlay = OnOfTimePlay;
            this.OnOfPlayProgress = OnOfPlayProgress;
            this.PlayProgress = PlayProgress;
            this.PlayPathIcon = PlayPathIcon;
        }
        //@"~\..\resources\downloaded_sound.png"
        //@"~\..\resources\downloading_sound.png"
        //@"~\..\resources\play_disabled.png"
        //@"~\..\resources\play.png"
        //@"~\..\resources\stop.png"

        //Столбец -------------------- ИМЯ ФАЙЛА --------------------

        //Имя звукового файла
        public string _Name;
        /// <summary>
        /// Имя звукового файла
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        //Столбец -------------------- Размер файла --------------------

        //Размер звукового файла
        public int _Size;
        /// <summary>
        /// Размер звукового файла
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    OnPropertyChanged("Size");
                }
            }
        }

        //Столбец -------------------- Скрытый URL --------------------

        //URL звукового файла (скрытый столбец, что бы не делать дополнительных запросов в бд)
        public string _URL;
        /// <summary>
        /// URL звукового файла (скрытый столбец, что бы не делать дополнительных запросов в бд)
        /// </summary>
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                if (_URL != value)
                {
                    _URL = value;
                    OnPropertyChanged("URL");
                }
            }
        }

        //Столбец -------------------- Загрузка файла --------------------

        //статус активности кнопки загрузки звуковых файлов
        public bool _OnOfDowlLoad;
        /// <summary>
        /// статус активности кнопки загрузки звуковых файлов
        /// </summary>
        public bool OnOfDowlLoad
        {
            get
            {
                return _OnOfDowlLoad;
            }
            set
            {
                if (_OnOfDowlLoad != value)
                {
                    _OnOfDowlLoad = value;
                    OnPropertyChanged("OnOfDowlLoad");
                }
            }
        }

        //------- Лейблы -------

        //статус отображения(видимости) процентов и скорости загрузки звуковых файлов
        public Visibility _OnOfPercent;
        /// <summary>
        /// статус отображения(видимости) процентов и скорости загрузки звуковых файлов
        /// </summary>
        public Visibility OnOfPercent
        {
            get
            {
                return _OnOfPercent;
            }
            set
            {
                if (_OnOfPercent != value)
                {
                    _OnOfPercent = value;
                    OnPropertyChanged("OnOfPercent");
                }
            }
        }

        //статус отображения(видимости) лейбла "Файл загружен"
        public Visibility _OnOfStateProgress;
        /// <summary>
        /// статус отображения(видимости) лейбла "Файл загружен"
        /// </summary>
        public Visibility OnOfStateProgress
        {
            get
            {
                return _OnOfStateProgress;
            }
            set
            {
                if (_OnOfStateProgress != value)
                {
                    _OnOfStateProgress = value;
                    OnPropertyChanged("OnOfStateProgress");
                }
            }
        }

        //процент загрузки звукового файла и скорость загрузки
        public string _Percent;
        /// <summary>
        /// процент загрузки звукового файла и скорость загрузки
        /// </summary>
        public string Percent
        {
            get
            {
                return _Percent;
            }
            set
            {
                if (_Percent != value)
                {
                    _Percent = value;
                    OnPropertyChanged("Percent");
                }
            }
        }

        //------- Прогрессор-------

        //статус отображения(видимости) прогрессора загрузки звуковых файлов
        public Visibility _OnOfDownloadProgress;
        /// <summary>
        /// статус отображения(видимости) прогрессора загрузки звуковых файлов
        /// </summary>
        public Visibility OnOfDownloadProgress
        {
            get
            {
                return _OnOfDownloadProgress;
            }
            set
            {
                if (_OnOfDownloadProgress != value)
                {
                    _OnOfDownloadProgress = value;
                    OnPropertyChanged("OnOfDownloadProgress");
                }
            }
        }

        //значение прогрессора загрузки звуковых файлов
        public int _DownloadProgress;
        /// <summary>
        /// значение прогрессора загрузки звуковых файлов
        /// </summary>
        public int DownloadProgress
        {
            get
            {
                return _DownloadProgress;
            }
            set
            {
                if (_DownloadProgress != value)
                {
                    _DownloadProgress = value;
                    OnPropertyChanged("DownloadProgress");
                }
            }
        }

        //путь иконки отображающейся на кнопке загрузки звугового файла
        public string _DownloadPathIcon;
        /// <summary>
        /// путь иконки отображающейся на кнопке загрузки звугового файла
        /// </summary>
        public string DownloadPathIcon
        {
            get
            {
                return _DownloadPathIcon;
            }
            set
            {
                if (_DownloadPathIcon != value)
                {
                    _DownloadPathIcon = value;
                    OnPropertyChanged("DownloadPathIcon");
                }
            }
        }

        //Столбец -------------------- Воспроизвести --------------------            
        //статус активности кнопки проигрывания звуковых файлов
        public bool _OnOfPlay;
        /// <summary>
        /// статус активности кнопки проигрывания звуковых файлов
        /// </summary>
        public bool OnOfPlay
        {
            get
            {
                return _OnOfPlay;
            }
            set
            {
                if (_OnOfPlay != value)
                {
                    _OnOfPlay = value;
                    OnPropertyChanged("OnOfPlay");
                }
            }
        }

        //------- Лейблы -------

        //статус отображения(видимости) времени проигрывания звукового файла
        public Visibility _OnOfTimePlay;
        /// <summary>
        /// статус отображения(видимости) времени проигрывания звукового файла
        /// </summary>
        public Visibility OnOfTimePlay
        {
            get
            {
                return _OnOfTimePlay;
            }
            set
            {
                if (_OnOfTimePlay != value)
                {
                    _OnOfTimePlay = value;
                    OnPropertyChanged("OnOfTimePlay");
                }
            }
        }

        //время проигрывания звукового файла
        public string _Time;
        /// <summary>
        /// время проигрывания звукового файла
        /// </summary>
        public string Time
        {
            get
            {
                return _Time;
            }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        //------- Прогрессор-------

        //статус отображения(видимости) прогрессора проигрывания звукового файла
        public Visibility _OnOfPlayProgress;
        /// <summary>
        /// статус отображения(видимости) прогрессора проигрывания звукового файла
        /// </summary>
        public Visibility OnOfPlayProgress
        {
            get
            {
                return _OnOfPlayProgress;
            }
            set
            {
                if (_OnOfPlayProgress != value)
                {
                    _OnOfPlayProgress = value;
                    OnPropertyChanged("OnOfPlayProgress");
                }
            }
        }

        //значение прогрессора проигрывания звукового файла
        public double _PlayProgress;
        /// <summary>
        /// значение прогрессора проигрывания звукового файла
        /// </summary>
        public double PlayProgress
        {
            get
            {
                return _PlayProgress;
            }
            set
            {
                if (_PlayProgress != value)
                {
                    _PlayProgress = value;
                    OnPropertyChanged("PlayProgress");
                }
            }
        }

        //максимальное значение прогрессора проигрывания звукового файла
        public double _MaxPlayTime;
        /// <summary>
        /// максимальное значение прогрессора проигрывания звукового файла
        /// </summary>
        public double MaxPlayTime
        {
            get
            {
                return _MaxPlayTime;
            }
            set
            {
                if (_MaxPlayTime != value)
                {
                    _MaxPlayTime = value;
                    OnPropertyChanged("MaxPlayTime");
                }
            }
        }

        //путь иконки отображающейся на кнопке проигрывания звукового файла
        public string _PlayPathIcon;
        /// <summary>
        /// путь иконки отображающейся на кнопке проигрывания звукового файла
        /// </summary>
        public string PlayPathIcon
        {
            get
            {
                return _PlayPathIcon;
            }
            set
            {
                if (_PlayPathIcon != value)
                {
                    _PlayPathIcon = value;
                    OnPropertyChanged("PlayPathIcon");
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
