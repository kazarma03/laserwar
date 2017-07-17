using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace TestLaserwar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;          
        }

        /// <summary>
        /// Загрузка главного окна
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, false);
           // download.Background = new SolidColorBrush(Colors.Blue);
            
            dataGridSounds.ItemsSource = bindSoundTabel;

            buttonDownload.DataContext = but;
            download.DataContext = but;
            sounds.DataContext = but;
            games.DataContext = but;
            but.OnOfDownloadJsonButtton = true;
            but.OnOfButtonDownLoad = true;
            but.OnOfButtonSound = false;
            but.OnOfButtonGame = false;
            but.ClickButtonDownLoad = true;
            but.ClickButtonSound = false;
            but.ClickButtonGame = false;
        }

        /// <summary>
        /// поток в котором выполняется загрузка Json объекта
        /// </summary>
        Thread th;

        /// <summary>
        /// экземпляр класса с полями для управления повидением кнопок
        /// </summary>
        buttons but = new buttons();

        /// <summary>
        /// класс с полями для управления повидением кнопок
        /// </summary>
        class buttons: INotifyPropertyChanged
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

        /// <summary>
        /// Обновление форм при переходах
        /// </summary>
        /// <param name="dkey"> если true форма Загрузки отображается, если false форма скрыта </param>
        /// <param name="skey"> если true форма Звука отображается, если false форма скрыта</param>
        /// <param name="gkey"> если true форма Игра отображается, если false форма скрыта </param>
        private void RefMainMenu(bool dkey, bool skey, bool gkey)
        {
            if (dkey)
            {
                GridDownload.Visibility = Visibility.Visible;
                //  download.Background = new SolidColorBrush(Colors.Blue);
                but.ClickButtonDownLoad = true;
            }
            else
            {
                GridDownload.Visibility = Visibility.Hidden;
                //  download.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
                but.ClickButtonDownLoad = false;
            }
            if (skey)
            {
                GridSound.Visibility = Visibility.Visible;
                //   sounds.Background = new SolidColorBrush(Colors.Blue);
                but.ClickButtonSound = true;
            }
            else
            {
                GridSound.Visibility = Visibility.Hidden;
                //  sounds.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
                but.ClickButtonSound = false;
            }
            if (gkey)
            {
                GridGames.Visibility = Visibility.Visible;
                // games.Background = new SolidColorBrush(Colors.Blue);
                but.ClickButtonGame = true;
            }
            else
            {
                GridGames.Visibility = Visibility.Hidden;
                //games.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
                but.ClickButtonGame = false;
            }
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, false);
        }

        private void sounds_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, true, false);
        }

        private void games_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, false, true);
        }

        /// <summary>
        /// Создаём БД для игр
        /// </summary>
        private void CreateDB()
        {
            SQLite SQL = new SQLite();

            string tabelGames = "Games";
            SQL.TabelDROP(tabelGames);
            string fildGames = "id_game INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "game_name TEXT, "
            + "game_date INTEGER";
            SQL.TabelCreate(tabelGames, fildGames);

            string tabelTeams = "Teams";
            SQL.TabelDROP(tabelTeams);
            string fildTeams = "id_team INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "team_name TEXT";
            SQL.TabelCreate(tabelTeams, fildTeams);

            //string tabelGamers = "Gamers";
            //SQL.TabelDROP(tabelGamers);
            //string fildGamers = "id_gamer INTEGER PRIMARY KEY AUTOINCREMENT, "
            //+ "gamer_name TEXT";
            //SQL.TabelCreate(tabelGamers, fildGamers);
            //fildGamers = "gamer_name";

            string tabelEvents = "Events";
            SQL.TabelDROP(tabelEvents);
            string fildEvents = "id_event INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "game INTEGER, "
            + "team INTEGER, "
            + "gamer_name TEXT, "
            + "rating INTEGER, "
            + "accuracy REAL, "
            + "shots INTEGER, "
            + "FOREIGN KEY(game) REFERENCES Games(id_game), "
            + "FOREIGN KEY(team) REFERENCES Commands(id_team)";
            SQL.TabelCreate(tabelEvents, fildEvents);
        }

        /// <summary>
        /// Парсим XML по ссылкам url
        /// </summary>
        /// <param name="val"> набор строк с url ссылками где находятся XML файлы с данными по играм </param>
        private void XMLparce(List<string> val)
        {
            SQLite SQL = new SQLite();
            //Создаём таблицы в БД для игр
            CreateDB();
            string tabelGames = "Games";
            string fildGames = "game_name, game_date";
            string tabelTeams = "Teams";
            string fildTeams = "team_name";
            string tabelEvents = "Events";
            string fildEvents = "game, team, gamer_name, rating, accuracy, shots";

            // по каждой URL скачиваем XML и парсим его
            XmlReader reader;
            DataTable dat;
            string id_Game = "";
            string id_Team = "";
            List<string> valSQL = new List<string>();
            string values;

            foreach (string param in val)
            {
                reader = XmlReader.Create(param);
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "game"))
                    {
                        values = "";
                        while (reader.MoveToNextAttribute()) valSQL.Add(reader.Value);
                        values = "'" + valSQL[0] + "', '" + valSQL[1] + "'";
                        SQL.SqlInsertSingle(tabelGames, fildGames, values);
                        dat = SQL.SqlRead("SELECT id_game from Games where game_name = '" + valSQL[0] + "' and game_date = '" + valSQL[1] + "'");
                        id_Game = dat.Rows[0][0].ToString(); ;
                        valSQL.Clear();
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "team"))
                    {
                        values = "";
                        while (reader.MoveToNextAttribute()) valSQL.Add(reader.Value);
                        values = "'" + valSQL[0] + "'";
                        SQL.SqlInsertSingle(tabelTeams, fildTeams, values);
                        dat = SQL.SqlRead("SELECT id_team from Teams where team_name = '" + valSQL[0] + "'");
                        id_Team = dat.Rows[0][0].ToString();
                        valSQL.Clear();
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "player"))
                    {
                        while (reader.MoveToNextAttribute()) valSQL.Add(reader.Value);
                        values = "'" + id_Game + "', '" + id_Team + "', '" + valSQL[0] + "', '" + valSQL[1] + "', '" + valSQL[2] + "', '" + valSQL[3] + "'";
                        SQL.SqlInsertSingle(tabelEvents, fildEvents, values);
                        valSQL.Clear();
                    }
                    reader.Dispose();
                }
            }
        }

        /// <summary>
        /// Считываем объект JSON по url парсим его и обрабатываем содержимое
        /// </summary>
        /// <param name="url"> ссылка по которой находиться объект JSON </param>
        public void DovnloadJSON(string url)
        {
            // ------------------ ПОДГОТОВКА ИНТЕРФЕЙСА И ПЕРЕМЕННЫХ ------------------          

            but.OnOfButtonSound = false;
            but.OnOfButtonGame = false;
            //очищаем лист экземпляра классов SoundTabel на тот случай если это не первы запуск загрузки
            bindSoundTabel.Clear();
            //деинициализируем массив потоков для загрузки звуковых файлов
            thDownloadSound = null;            
            //Скрываем интерфейс поиска данных
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                dataGridSounds.Items.Refresh();
                TextBoxJson.Visibility = Visibility.Hidden;
                LabelStateDownload.Visibility = Visibility.Hidden;
            });

            // ------------------------------------------------------------------------

            WebClient client = new WebClient();
            JObject jObject;
            string data = null;
            // true- JSON объект содержит необходимые данные, false- данных для вывода нет
            bool keyRef = false;

            try
            {
                data = client.DownloadString(url);
            }
            catch (Exception ex)
            { }
            if (data != null)
            {
                jObject = JObject.Parse(data);

                SQLite SQL = new SQLite();
                List<string> val = new List<string>();

                string SubVal = "", MainVal = "";

                // Выводим Текст ошибки
                MainVal = (string)jObject["error"];
                if (!string.IsNullOrWhiteSpace(MainVal))
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        TextBoxJson.Text += "Error:" + Environment.NewLine + MainVal + Environment.NewLine;
                    });
                    keyRef = true;
                }

                // Выводим URL игр           
                if (jObject["games"] != null)
                {
                    MainVal = "";
                    foreach (var Data in jObject["games"])
                    {
                        //собираем под строку для вывода на форму
                        SubVal = (string)Data["url"];
                        //собирамем набор строк для последующей загрузки данных по URl
                        val.Add((string)Data["url"]);
                        //собираем полную строку для вывода на форму
                        if (!string.IsNullOrWhiteSpace(SubVal)) MainVal += SubVal + Environment.NewLine;
                    }
                    //Если в JSON объекте присутствуют записи об играх выводим их в нижней форме экрана и заносим в БД laserwar.db таблица Games
                    if (!string.IsNullOrWhiteSpace(MainVal))
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            TextBoxJson.Text += "Game url:" + Environment.NewLine + MainVal + Environment.NewLine;
                        });
                        keyRef = true;
                        XMLparce(val);
                    }
                    val.Clear();
                }

                // Выводим  Название, URL и размер звуковых файлов
                if (jObject["sounds"] != null)
                {
                    MainVal = "";
                    //List<MyTable> result = new List<MyTable>(3);
                    foreach (var Data in jObject["sounds"])
                    {
                        //собираем под строку для вывода на форму
                        SubVal = (string)Data["name"] + " " + (string)Data["url"] + " " + (string)Data["size"];
                        //собирамем набор строк для последующей загрузки данных в БД
                        val.Add("'" + (string)Data["name"] + "'" + ", " + "'" + (string)Data["url"] + "'" + ", " + "'" + (string)Data["size"] + "'");
                        //собираем данные о композициях для последующего добавления их в таблицу
                        //result.Add(new MyTable((string)Data["name"], ((int)Data["size"]) / 1024, (string)Data["url"]));
                        bindSoundTabel.Add(new SoundTable((string)Data["name"], ((int)Data["size"]) / 1024, (string)Data["url"], true, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, @"~\..\resources\downloading_sound.png", Visibility.Hidden, Visibility.Hidden, 0, @"~\..\resources\play_disabled.png"));
                        //собираем полную строку для вывода на форму
                        if (!string.IsNullOrWhiteSpace(SubVal)) MainVal += SubVal + " byte" + Environment.NewLine;
                    }

                    //Если в JSON объекте присутствуют записи о звуковых файлах выводим их в нижней форме экрана и заносис в БД laserwar.db таблица Sounds
                    if (!string.IsNullOrWhiteSpace(MainVal))
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            TextBoxJson.Text += "Sounds:" + Environment.NewLine + MainVal + Environment.NewLine;
                        });
                        keyRef = true;
                        string tabel = "Sounds";
                        SQL.TabelDROP(tabel);
                        string fild = "id_Sound INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "name TEXT, "
                        + "url TEXT, "
                        + "size INTEGER";
                        SQL.TabelCreate(tabel, fild);
                        fild = "name, url, size";
                        SQL.SqlInsertList(tabel, fild, val);

                    }
                    val.Clear();
                }
                if (keyRef)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        TextBoxJson.Visibility = Visibility.Visible;
                        LabelStateDownload.Content = "Файл успешно загружен";
                        but.OnOfButtonSound = true;
                        but.OnOfButtonGame = true;
                    });
                }
                else
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        LabelStateDownload.Content = "Файл не содержит необходимых данных";
                    });
                }
            }
            else
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    LabelStateDownload.Content = "Файл не найден";
                });
            }
            //Отображаем статус загрузки данных
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                LabelStateDownload.Visibility = Visibility.Visible;
                but.OnOfDownloadJsonButtton = true;
            });
        }


        /// <summary>
        /// Загрузка объекта JSON
        /// </summary>
        private void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            but.OnOfDownloadJsonButtton = false;
            TextBoxJson.Visibility = Visibility.Hidden;
            LabelStateDownload.Visibility = Visibility.Hidden;
            //до очисктки таблицы останавливаем воспроизведение если оно было активным
            if (StatePlaySound == true) StopSound();
            TextBoxJson.Text = "";
            string url = TextBoxAddress.Text;
            th = new Thread(() => DovnloadJSON(url));
            th.Start();
        }

        //-------------------------------------------------
        /// <summary>
        /// Класс с полями для заполнения таблицы
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

        /// <summary>
        /// Лист экземпляров класса SoundTable для удобного обращения к ячейкам таблицы звуков
        /// </summary>
        List<SoundTable> bindSoundTabel = new List<SoundTable>();
        /// <summary>
        /// Статус проигрывания звукового файла, false - не проигрывается, true - проигрывается
        /// </summary>
        bool StatePlaySound = false;
        /// <summary>
        /// индекс текущей проигрываемой мелодии
        /// </summary>
        int indPlaySound;

        /// <summary>
        /// событие - нажатие кнопки в столбце воспроизведение
        /// </summary>
        private void PlaySound_Click(object sender, RoutedEventArgs e)
        {

            int ind = dataGridSounds.Items.IndexOf(dataGridSounds.CurrentItem);
            // если в тукущий момент ничего не воспроизводиться
            if (!StatePlaySound)
            {
                PlaySound(ind);
            }
            else
            {               
                if (indPlaySound != ind)
                {
                    //если в текущий момент воспроизводится звуковой файл и мы пытаемся запустить другой файл
                    StopSound();
                    PlaySound(ind);
                }
                else
                {
                    //если в текущий момент воспроизводится звуковой файл и мы пытаемся остановить воспроизведение текущего файла
                    StopSound();
                }
            }
        }

        /// <summary>
        /// массив потоков загружающих звуковые композиции
        /// </summary>
        Thread[] thDownloadSound;

        /// <summary>
        /// событие - нажатие кнопки в столбце Загрузка файла
        /// </summary>
        private void DownloadSound_Click(object sender, RoutedEventArgs e)
        {
            //Определяем индекс текущего элемента с которым осуществляется взаимодействие
            int ind = dataGridSounds.Items.IndexOf(dataGridSounds.CurrentItem);
            //Отображаем интерфейс скачивания звукового файла
            bindSoundTabel[ind].OnOfDowlLoad = false;
            bindSoundTabel[ind].OnOfDownloadProgress = Visibility.Visible;
            bindSoundTabel[ind].OnOfPercent = Visibility.Visible;

            // при первом запуске первого скачиваемого файла инициализируем массив потоков, определяя размерность по колличеству строк в таблице звуковых файлов
            if (thDownloadSound == null) thDownloadSound = new Thread[dataGridSounds.Items.Count];

            // запускаем новый поток скачивания звукового файла
            thDownloadSound[ind] = new Thread(() => DownloadingSound(ind));
            thDownloadSound[ind].Name = "downloadSound " + bindSoundTabel[ind].URL.Substring(bindSoundTabel[ind].URL.LastIndexOf(@"/") + 1);
            thDownloadSound[ind].Priority = ThreadPriority.Normal;
            thDownloadSound[ind].Start();

        }

        /// <summary>
        /// метод загрузки звукового файла в отдельном потоке
        /// </summary>
        /// <param name="ind"> индекс текущего обрабатываемого элемента</param>
        public void DownloadingSound(int ind)
        {                       
            using (WebClient client = new WebClient())
            {
                Stopwatch sw = new Stopwatch();
                sw.Reset();
                //создание и перегрузка события завершения загрузки асинхронного загрузчика
                client.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) => Completed(sender, e, ind));
                //создание и перегрузка события изменения состояния загрузки асинхронного загрузчика
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) => ProgressChanged(sender, e, ind, sw));

                string FileName = bindSoundTabel[ind].URL.Substring(bindSoundTabel[ind].URL.LastIndexOf(@"/") + 1);
                string FilePath = bindSoundTabel[ind].URL;
                //string FilePath = "http://ru.download.nvidia.com/Windows/384.76/384.76-desktop-win10-64bit-international-whql.exe";
                //string FilePath = "http://best-muzon.cc/dl/CZisIgYr9nmLLV9RcFvBRA/1500238531/songs12/2017/01/luis-fonsi-feat.-daddy-yankee-despacito-(best-muzon.cc).mp3";
                // Запуск Stopwatch для дальнейшего расчета скорости загрузки файла
                sw.Start();
                try
                {
                    // начинаем загрузку файла асинхронно
                    client.DownloadFileAsync(new Uri(FilePath), FileName);               
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        // Событие изменения асинхронного загрузчика файлов
        /// <summary>
        /// Событие изменения асинхронного загрузчика файлов
        /// </summary>
        /// <param name="ind">Индекс текущего обрабатываемого элемента</param>
        /// <param name="sw">Stopwatch для расчета скорости загрузки файла</param>
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e, int ind, Stopwatch sw)
        {           
            // Обновляем прогресс бар загрузки файла.
            bindSoundTabel[ind].DownloadProgress = e.ProgressPercentage;
            // Считаем скорость загрузки файла
            double speed = e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds;            
            string STRspeed;
            if (speed > 1024)
            {
                speed = speed / 1024;
                STRspeed = string.Format("{0} Mb/s", (speed).ToString("0.00"));
            }
            else
            {
                STRspeed = string.Format("{0} kb/s", (speed).ToString("0.00"));
            }
            //выводим процент загрузки файла и скорость
            bindSoundTabel[ind].Percent = e.ProgressPercentage.ToString() + " %  " + STRspeed;
            // string.Format("{0} MB's / {1} MB's",(e.BytesReceived / 1024d / 1024d).ToString("0.00"),(e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // Событие завершения работы асинхронного загрузчика файлов
        /// <summary>
        /// Событие завершения работы асинхронного загрузчика файлов
        /// </summary>
        /// <param name="ind">Индекс текущего обрабатываемого элемента</param>
        private void Completed(object sender, AsyncCompletedEventArgs e, int ind)
        {
            if (e.Cancelled == true)
            {
                //tесли было принудительное завершение загрузки файла
                //MessageBox.Show("Download has been canceled.");
            }
            else
            {
                //отображаем лейбл "Файл загружен"
                bindSoundTabel[ind].OnOfStateProgress = Visibility.Visible;
                //Делаю кнопку загрузки неактивной
                bindSoundTabel[ind].OnOfDowlLoad = false;
                //Меняем иконку загрузки на неактивную
                bindSoundTabel[ind].DownloadPathIcon = @"~\..\resources\downloaded_sound.png";
                //Скрываем прогрессор загрузки
                bindSoundTabel[ind].OnOfDownloadProgress = Visibility.Hidden;
                //Скрываем проценты загрузки
                bindSoundTabel[ind].OnOfPercent = Visibility.Hidden;
                //Делаем кнопку проигрывания активной
                bindSoundTabel[ind].OnOfPlay = true;
                //Меняем иконку проигрывания на активную
                bindSoundTabel[ind].PlayPathIcon = @"~\..\resources\play.png";
            }
        }

        /// <summary>
        /// екземпляр MediaPlayer для проигрывания звукового файла
        /// </summary>
        private MediaPlayer player = new MediaPlayer();
        /// <summary>
        /// Таймер для визуализации процесса проигрывания звукового файла и завершения проигрывания
        /// </summary>
        DispatcherTimer timer;

        /// <summary>
        /// Проигрывание звукового файлв
        /// </summary>
        /// <param name="ind">Индекс текущего проигрываемого элемента</param>
        public void PlaySound(int ind)
        {
            // инициализируем интерфейс проигрывания звукового файла
            bindSoundTabel[ind].PlayProgress = 0;
            bindSoundTabel[ind].Time = "0:00";
            bindSoundTabel[ind].MaxPlayTime = 1000;

            timer = new DispatcherTimer();
            //задаем интервал срабатывания таймера для плавности отображения изменений интерфейса
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += new EventHandler(timer_Tick);
            // timer.Tick += new EventHandler((sender, e) => timer_Tick(sender, e, ind));
            timer.Start();

            StatePlaySound = true;
            //запоминием текущий индекс проигрываемого файла для обновления интерфейса в таймере
            indPlaySound = ind;

            // bindSoundTabel[ind].PlayProgress = 0;
            // bindSoundTabel[ind].Time = "0:00";            
            //Меняем иконку проигрывания на активную           
            bindSoundTabel[ind].PlayPathIcon = @"~\..\resources\stop.png";
            bindSoundTabel[ind].OnOfTimePlay = Visibility.Visible;
            bindSoundTabel[ind].OnOfPlayProgress = Visibility.Visible;
            
            string FileName = bindSoundTabel[ind].URL.Substring(bindSoundTabel[ind].URL.LastIndexOf(@"/") + 1);
            player.Open(new Uri(FileName, UriKind.Relative));
            //Проигрываем композицию
            player.Play();
        }

        /// <summary>
        /// Событие срабатывающее по таймеру каждые 0.01 сек для обновления интерфейса при проигрывании
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((player.Source != null) && (player.NaturalDuration.HasTimeSpan))
            {
                bindSoundTabel[indPlaySound].MaxPlayTime = player.NaturalDuration.TimeSpan.TotalSeconds;
                bindSoundTabel[indPlaySound].Time= TimeSpan.FromSeconds(player.Position.TotalSeconds).ToString(@"m\:ss"); ;
                bindSoundTabel[indPlaySound].PlayProgress = player.Position.TotalSeconds;

                if (bindSoundTabel[indPlaySound].MaxPlayTime == bindSoundTabel[indPlaySound].PlayProgress)
                {
                    //композиция полностью проиграна, производим остановку
                    StopSound();
                }
            }
        }

        /// <summary>
        /// Остановка проигрывания композиции
        /// </summary>
        /// <param name="ind">индекс текущей проигрываемой композиции</param>
        public void StopSound()
        {
            //Останавливаем проигрывание композиции
            StatePlaySound = false;
            bindSoundTabel[indPlaySound].PlayPathIcon = @"~\..\resources\play.png";
            bindSoundTabel[indPlaySound].OnOfTimePlay = Visibility.Hidden;
            bindSoundTabel[indPlaySound].OnOfPlayProgress = Visibility.Hidden;
            player.Stop();
            // останавливаем таймер
            timer.Stop();
        }

    }
}
