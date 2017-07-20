using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Globalization;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
            
            dataGridSounds.ItemsSource = bindSoundTabel;
            dataGridGame.ItemsSource =bindGameTabel;
            DetailGame.DataContext = LabelDetail;

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
                GridGamesDetail.Visibility = Visibility.Hidden;
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
                GridGamesDetail.Visibility = Visibility.Hidden;
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
                GridGamesDetail.Visibility = Visibility.Hidden;
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
            GetGameData();
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
            string fildEvents = "id INTEGER PRIMARY KEY AUTOINCREMENT, "
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
                    
                }
                reader.Dispose();
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
                    foreach (var Data in jObject["sounds"])
                    {
                        //собираем под строку для вывода на форму
                        SubVal = (string)Data["name"] + " " + (string)Data["url"] + " " + (string)Data["size"];
                        //собирамем набор строк для последующей загрузки данных в БД
                        val.Add("'" + (string)Data["name"] + "'" + ", " + "'" + (string)Data["url"] + "'" + ", " + "'" + (string)Data["size"] + "'");
                        //собираем данные о композициях для последующего добавления их в таблицу
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

        //****************************************************************
        //*******************        ЗВУКИ         ***********************
        //****************************************************************

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


        //****************************************************************
        //*******************        ИГРЫ          ***********************
        //****************************************************************


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

        /// <summary>
        /// Лист экземпляров класса GameTabel для удобного обращения к ячейкам таблицы игр
        /// </summary>
        List<GameTable> bindGameTabel = new List<GameTable>();        

        private void GetGameData()
        {
            DataTable GameTabel = new DataTable();
            SQLite SQL = new SQLite();
            bindGameTabel.Clear();
            dataGridGame.Items.Refresh();
            string SQLQuery = "Select Games.id_Game,Games.game_name, Games.game_date, count(Events.gamer_name) from Games inner join Events on Games.id_game=Events.game group by Games.game_name";
            GameTabel = SQL.SqlRead(SQLQuery);
            for (int i = 0; i <= GameTabel.Rows.Count-1; i++)
            {
                bindGameTabel.Add(new GameTable(Convert.ToInt32(GameTabel.Rows[i][0]), GameTabel.Rows[i][1].ToString(), UnixTimeStampToDateTime(Convert.ToDouble(GameTabel.Rows[i][2])).ToString(), Convert.ToInt32(GameTabel.Rows[i][3])));
            }
            dataGridGame.Items.Refresh();  
        }

        /// <summary>
        /// Конвертор UNIX-время в DateTime
        /// </summary>
        /// <param name="unixTimeStamp">количество секунд, прошедших с полуночи (00:00:00 UTC) 1 января 1970 года (четверг) до указанного момента</param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        //Уникальный идентификатор игры для построения ЗВА
        int ID;

        /// <summary>
        /// Двойной клик на строке в таблице игр, открывающий форму детализации игр
        /// </summary>
        private void dataGridGame_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridGame.CurrentItem != null)
            {
                //получаем уникальный идентификатор игры
                int id_game = bindGameTabel[dataGridGame.Items.IndexOf(dataGridGame.CurrentItem)].id_Game;
                int count = bindGameTabel.Count;
                GridGames.Visibility = Visibility.Hidden;
                GridGamesDetail.Visibility = Visibility.Visible;
                ID = id_game;
                FillGameDetail(id_game);
            }
         }


        //****************************************************************
        //*******************   Детализаци  ИГРЫ   ***********************
        //****************************************************************


        /// <summary>
        /// Класс с полями для заполнения таблицы ИГР
        /// </summary>
        class DetailGameTable : INotifyPropertyChanged
        {

            public DetailGameTable(int Id, string CommandName, string GamerName, int Rating, string Accuracy, int Shots)
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
            public int _Rating;
            /// <summary>
            /// Рейтинг
            /// </summary>
            public int Rating
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
            public int _Shots;
            /// <summary>
            /// количество выстрелов
            /// </summary>
            public int Shots
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

        LabelDetailGameTable LabelDetail = new LabelDetailGameTable();

        ObservableCollection<DetailGameTable> ObsCollectionDetailGameTabel = new ObservableCollection<DetailGameTable>();

        /// <summary>
        /// Заполнение таблицы детализации игр
        /// </summary>
        /// <param name="id_game"> id детализируемой игры</param>
        private void FillGameDetail(int id_game)
        {
            SQLite SQL = new SQLite();
            DataTable DetailGameTabel = new DataTable();
            string SQLQuery;          

            ObsCollectionDetailGameTabel.Clear();

            dataGridDetailGame.Items.Refresh();
            //получаем имя детализируемой игры
            SQLQuery = "select Games.game_name from Games where Games.id_game='" + id_game + "'";
            DetailGameTabel = SQL.SqlRead(SQLQuery);
            LabelDetail.DetailGameName = DetailGameTabel.Rows[0][0].ToString();
            //получаем набор команд и данные по игрокам
            SQLQuery = "select Events.Id,Teams.team_name, Events.gamer_name, Events.rating, Events.accuracy, Events.shots from Events inner join Teams on Teams.id_team = Events.team where game = '" + id_game + "'";

            DetailGameTabel = SQL.SqlRead(SQLQuery);
            for (int i = 0; i <= DetailGameTabel.Rows.Count - 1; i++)
            { 
                int Id = Convert.ToInt32(DetailGameTabel.Rows[i][0]);
                string CommandName = DetailGameTabel.Rows[i][1].ToString();
                string GamerName = DetailGameTabel.Rows[i][2].ToString();
                int Rating = Convert.ToInt32(DetailGameTabel.Rows[i][3]);
                string Accuracy = Convert.ToString(Convert.ToDouble(DetailGameTabel.Rows[i][4]) * 100) + " %";
                int Shots = Convert.ToInt32(DetailGameTabel.Rows[i][5]);

               ObsCollectionDetailGameTabel.Add( new DetailGameTable(Id, CommandName, GamerName, Rating, Accuracy, Shots));
            }


            ListCollectionView collection = new ListCollectionView(ObsCollectionDetailGameTabel);
            //Устанавливаем элемент по которому будет проводиться группировка
            collection.GroupDescriptions.Add(new PropertyGroupDescription("CommandName"));

            dataGridDetailGame.ItemsSource = collection;
            dataGridDetailGame.Items.Refresh();
        }

        /// <summary>
        /// Возврат на форму игр
        /// </summary>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            GridGames.Visibility = Visibility.Visible;
            GridGamesDetail.Visibility = Visibility.Hidden;
            GetGameData();
        }
  
        // поля для обновления данных по игроку через таблицу детализации игр
        // сохраняем значения полей до изменения
        int Prep_id;
        int Row_id;
        int Prep_Rating;
        string Prep_Accuracy;
        int Prep_Shots;

        /// <summary>
        /// Подготовка к изменению данных в ячейки таблицы детализации игр
        /// </summary>
        private void dataGridDetailGame_PreparingCellForEdit(object sender, System.Windows.Controls.DataGridPreparingCellForEditEventArgs e)
        {
            Row_id= dataGridDetailGame.Items.IndexOf(dataGridDetailGame.CurrentItem);
            Prep_id = ObsCollectionDetailGameTabel[Row_id].Id;
            Prep_Rating = ObsCollectionDetailGameTabel[Row_id].Rating;
            Prep_Accuracy = ObsCollectionDetailGameTabel[Row_id].Accuracy;
            Prep_Shots = ObsCollectionDetailGameTabel[Row_id].Shots;
        }

        /// <summary>
        /// Завершение изменений данных в ячейки таблицы детализации игр
        /// </summary>
        private void dataGridDetailGame_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
     
            //считываем значения полей после потери фокуса
            int Aft_Rating = ObsCollectionDetailGameTabel[Row_id].Rating;
            string Aft_Accuracy = ObsCollectionDetailGameTabel[Row_id].Accuracy;
            int Aft_Shots = ObsCollectionDetailGameTabel[Row_id].Shots;

            //сравниваем значение до изменений с текущем значением
            //если изменения различаются то вносим изменения в БД
            if (Aft_Rating != Prep_Rating)
            {
                //MessageBox.Show("изменён рейтинг");
                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "rating";
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, Aft_Rating.ToString(), "id ='" + Prep_id + "'");
            }
            if (Aft_Accuracy != Prep_Accuracy)
            {
                // MessageBox.Show("изменёна точность");
                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "accuracy";
                // Удаляем из строки все символы кроме цифр
                string prom_Accuracy = "";
                for (int i = 0; i < Aft_Accuracy.Length; i++)
                {
                    if (Char.IsDigit(Aft_Accuracy[i])) prom_Accuracy += Aft_Accuracy[i];
                }
                double new_Accuracy =0;
                if (prom_Accuracy != "") new_Accuracy = Convert.ToDouble(prom_Accuracy) / 100;
                else
                {
                    prom_Accuracy = "0";
                    new_Accuracy = 0;
                }
                ObsCollectionDetailGameTabel[Row_id].Accuracy = prom_Accuracy.ToString() + " %";
                string STRnew_Accuracy = new_Accuracy.ToString();
                //меняем точку на запятую,т.к. бд при вовторном запросе вернёт 0 если в числе будет запятая
                STRnew_Accuracy = STRnew_Accuracy.Replace(",", ".");
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, STRnew_Accuracy, "id ='" + Prep_id + "'");

            }
            if (Aft_Shots != Prep_Shots)
            {
                // MessageBox.Show("изменёны выстрелы");
                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "shots";
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, Aft_Shots.ToString(), "id ='" + Prep_id + "'");

            }
        }

        /// <summary>
        /// Редактирование информации по игроку при двойном нажатии на строку
        /// </summary>
        private void dataGridDetailGame_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridDetailGame.CurrentItem != null)
            {
                //получаем уникальный идентификатор игры
                int id_game = ObsCollectionDetailGameTabel[dataGridDetailGame.Items.IndexOf(dataGridDetailGame.CurrentItem)].Id;

            }
        }

        /// <summary>
        ///  Нажатие кнопки сохранения детализации игры в PDF
        /// </summary>
        private void SavePDF_Click(object sender, RoutedEventArgs e)
        {
            CreatePDF();
        }

        /// <summary>
        /// Сохраняем детализацию игры в PDF
        /// </summary>
        /// <returns>Имя файла</returns>
        private string CreatePDF()
        {
            SQLite SQL = new SQLite();
            DataTable SQLansw = new DataTable();
            string SQLQuery;

            SQLQuery = "select Games.game_name, Games.game_date from Games where Games.id_game='" + ID + "'";
            SQLansw = SQL.SqlRead(SQLQuery);

            string GameName = SQLansw.Rows[0][0].ToString();
            string Date = UnixTimeStampToDateTime(Convert.ToDouble(SQLansw.Rows[0][1])).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            string FileName = "Статистика " + GameName;
            BaseFont baseFont = RegisterFonts();
            string path = Directory.GetCurrentDirectory();

            string[] findFiles = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), FileName + ".pdf");
            FileStream fs;
            int j = 1;

            //Проверяем существование файла с указанным именем, если есть генерируем новое имя
            if (findFiles.LongLength == 0)
            {
                FileName = FileName + ".pdf";
                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            else
            {
                string NewName = "";
                while (findFiles.LongLength != 0)
                {
                    NewName = FileName + "_" + j;
                    findFiles = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), NewName + ".pdf");
                    j++;
                }
                FileName = NewName + ".pdf";
                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            }

            iTextSharp.text.Rectangle rec3 = new iTextSharp.text.Rectangle(PageSize.A4);
            rec3.BackgroundColor = new BaseColor(System.Drawing.Color.White);
            Document doc = new Document(rec3, 50, 30, 30, 30);

            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.AddTitle("Отчет по игре");
            doc.AddSubject(GameName + " " + Date);
            doc.AddAuthor("LazserWar");

            // Получаем данные для выгрузки PDF
            SQLQuery = "Select Teams.id_team, Teams.team_name, Events.gamer_name, Events.rating, Events.accuracy, Events.shots from Events inner join Teams on Teams.id_team = Events.team where game = '" + ID + "'";
            SQLansw = SQL.SqlRead(SQLQuery);

            iTextSharp.text.Font fontPDFHeader = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontPDFColumnName = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontPDFCommandName = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontPDFData = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fontPDFDataAll = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);

            PdfPTable table = new PdfPTable(4);

            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] widths = new float[] { 120f, 100f, 100f, 100f };
            table.SetWidths(widths);

            PdfPCell cell = new PdfPCell(new Phrase(GameName + "    " + Date, fontPDFHeader));
            cell.Colspan = 4;
            cell.FixedHeight = 40f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Игрок", fontPDFColumnName));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.FixedHeight = 30f;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Рейтинг", fontPDFColumnName));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.FixedHeight = 30f;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Точность", fontPDFColumnName));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.FixedHeight = 30f;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Выстрелы", fontPDFColumnName));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.FixedHeight = 30f;
            table.AddCell(cell);
            int id = -1;
            string CommandName;

            DataTable SQLanswCom = new DataTable();
            SQLQuery = " Select Events.game, Teams.team_name, count(DISTINCT Events.team)from Events inner join Teams on Teams.id_team = Events.team where Events.game = '" + ID + "' group by Events.game";
            SQLanswCom = SQL.SqlRead(SQLQuery);
            int indMAX = Convert.ToInt32(SQLanswCom.Rows[0][2]);
            PdfPCell[] _cellCommand = new PdfPCell[indMAX];

            float[] reting = new float[indMAX];
            float retingCommand = 0;
            float[] accuracy = new float[indMAX];
            float accuracyCommand = 0;
            float progress;

            int ind = 0;
            foreach (DataRow row in SQLansw.Rows)
            {
                if (id != Convert.ToInt32(row[0]))
                {
                    CommandName = row[1].ToString();
                    cell = new PdfPCell(new Phrase(CommandName, fontPDFCommandName));
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 50f;
                    table.AddCell(cell);

                    id = Convert.ToInt32(row[0]);

                    _cellCommand[ind] = new PdfPCell(new Phrase(CommandName, fontPDFCommandName));
                    _cellCommand[ind].Colspan = 4;
                    _cellCommand[ind].FixedHeight = 40f;
                    _cellCommand[ind].HorizontalAlignment = Element.ALIGN_LEFT;
                    _cellCommand[ind].VerticalAlignment = Element.ALIGN_MIDDLE;
                    _cellCommand[ind].Border = iTextSharp.text.Rectangle.NO_BORDER;

                    reting[ind] = ind + 7;
                    accuracy[ind] = ind + 9;

                    ind++;
                }
                cell = new PdfPCell(new Phrase(row[2].ToString(), fontPDFData));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.FixedHeight = 20f;
                cell.BorderColor = new BaseColor(197, 197, 197);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(row[3].ToString(), fontPDFData));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.FixedHeight = 20f;
                cell.BorderColor = new BaseColor(197, 197, 197);
                table.AddCell(cell);

                double _Accuracy = Convert.ToDouble(row[4]) * 100;
                string Accuracy = _Accuracy.ToString() + " %";
                cell = new PdfPCell(new Phrase(Accuracy, fontPDFData));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.FixedHeight = 20f;
                cell.BorderColor = new BaseColor(197, 197, 197);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(row[5].ToString(), fontPDFData));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.FixedHeight = 20f;
                cell.BorderColor = new BaseColor(197, 197, 197);
                table.AddCell(cell);
            }

            doc.Add(table);

            table = new PdfPTable(9);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] widthsDetail = new float[] { 3f, 117f, 117f, 3f, 20f, 3f, 117f, 117f, 3f };
            table.SetWidths(widthsDetail);

            cell = new PdfPCell(new Phrase("  "));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.FixedHeight = 40f;
            cell.Colspan = 9;
            table.AddCell(cell);

            // СЧЕТЧИК КОЛИЧЕСТВА ВЫВОДОВ КОМАНД В PDF ДЛЯ ОПРЕДЕЛЕНИЯ ЧЕТНОЙ ИЛИ НЕЧЕТНОЙ ДОБАВЛЯЕМОЙ КОМАНДЫ
            int tick = 1;
            // СЧЕТЧИК КОЛИЧЕСТВА ВЫВОДОВ КОМАНД
            int i = 0;
            // СЧЕТЧИК КОЛИЧЕСТВА ВЫВОДОВ ПАРАМЕТРОВ
            int iparam = 0;

            // ШАБЛОН ДЛЯ ДОБАВЛЕНИЯ ИММИТИЦИИ ПРОГРЕССОРА В ЯЧЕЙКУ
            PdfTemplate template;

            while (i < indMAX)
            {
                if (tick <= 2)
                {
                    if (tick % 2 != 0)
                    {
                        // НАЗВАНИЯ КОМАНД, НЕЧЕТНЫЙ ВЫВОД
                        table.AddCell(_cellCommand[i]);
                        tick++;
                        // ПУСТАЯ КОЛОНКА МЕЖДУ НИМИ
                        cell = new PdfPCell(new Phrase(" "));
                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(cell);
                    }
                    else
                    {
                        // НАЗВАНИЯ КОМАНД, ЧЕТНЫЙ ВЫВОД
                        table.AddCell(_cellCommand[i]);
                        tick++;
                    }
                }
                if (tick > 2)
                {
                    //##########################    НОВАЯ СТРОКА   ##########################

                    // РЕЙТИНГ НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase("Рейтинг", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЗНАЧЕНИЕ РЕЙТИНКА НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(reting[iparam].ToString(), fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЯЧЕЙКА КОЛОНКИ РАЗДЕЛИТЕЛЯ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    // РЕЙТИНГ ЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase("Рейтинг", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЗНАЧЕНИЕ РЕЙТИНГА ЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(reting[iparam + 1].ToString(), fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);
                    // ПРОГРЕССОР РЕЙТИНГА ЧЁТНОЙ КОМАНДЫ



                    template = PdfTemplate.CreateTemplate(writer, 234f, 10f);
                    template.SetColorFill(new BaseColor((System.Drawing.Color.Blue)));
                    template.Rectangle(0, 0, 234f, 10f);


                    template.Fill();
                    writer.ReleaseTemplate(template);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                    cell.Colspan = 2;
                    cell.FixedHeight = 10f;
                    cell.BorderColor = new BaseColor(System.Drawing.Color.Blue);
                    table.AddCell(cell);
                    // ЯЧЕЙКА КОЛОНКИ РАЗДЕЛИТЕЛЯ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    cell.Colspan = 3;
                    table.AddCell(cell);
                    // ПРОГРЕССОР РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ


                    template = PdfTemplate.CreateTemplate(writer, 234f, 10f);
                    template.SetColorFill(new BaseColor((System.Drawing.Color.Blue)));
                    template.Rectangle(0, 0, 234f, 10f);


                    template.Fill();
                    writer.ReleaseTemplate(template);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                    cell.Colspan = 2;
                    cell.FixedHeight = 10f;
                    cell.BorderColor = new BaseColor(System.Drawing.Color.Blue);
                    table.AddCell(cell);
                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ТОЧНОСТЬ НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase("Точночть", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЗНАЧЕНИЕ ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(accuracy[iparam].ToString() + " %", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЯЧЕЙКА КОЛОНКИ РАЗДЕЛИТЕЛЯ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    // ТОЧНОСТЬ ЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase("Точночть", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЗНАЧЕНИЕ ТОЧНОСТИ ЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(accuracy[iparam + 1].ToString() + " %", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################
                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);
                    // ПРОГРЕССОР ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ


                    progress = (234 * accuracy[iparam]) / 100;

                    template = PdfTemplate.CreateTemplate(writer, 234f, 10f);
                    template.SetColorFill(new BaseColor((System.Drawing.Color.Blue)));
                    template.Rectangle(0, 0, progress, 10f);


                    template.Fill();
                    writer.ReleaseTemplate(template);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                    cell.Colspan = 2;
                    cell.FixedHeight = 10f;
                    cell.BorderColor = new BaseColor(System.Drawing.Color.Blue);
                    table.AddCell(cell);
                    // ЯЧЕЙКА КОЛОНКИ РАЗДЕЛИТЕЛЯ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    cell.Colspan = 3;
                    table.AddCell(cell);
                    // ПРОГРЕССОР ТОЧНОСТИ ЧЁТНОЙ КОМАНДЫ

                    progress = (234 * accuracy[iparam + 1]) / 100;

                    template = PdfTemplate.CreateTemplate(writer, 234f, 10f);
                    template.SetColorFill(new BaseColor((System.Drawing.Color.Blue)));
                    template.Rectangle(0, 0, progress, 10f);


                    template.Fill();
                    writer.ReleaseTemplate(template);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                    cell.Colspan = 2;
                    cell.FixedHeight = 10f;
                    cell.BorderColor = new BaseColor(System.Drawing.Color.Blue);
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);

                    // ДОБАВИЛИ ДВА НАБОРА ПАРАМЕТРОВ
                    iparam = iparam + 2;
                    // СБРАСЫВАЕМ СЧЁТЧИК КОМАНД
                    tick = 1;
                }
                i++;
                if ((i == indMAX) && (i % 2 != 0))
                {
                    // ПУСТАЯ СТРОКА ЧЕТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // РЕЙТИНГ НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase("Рейтинг", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЗНАЧЕНИЕ РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(reting[iparam].ToString(), fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // РЕЙТИНГ ЧЁТНОЙ КОМАНДЫ, ЗНАЧЕНИЕ РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ С ПУСТОЙ КОЛОНКОЙ ДО 
                    cell = new PdfPCell(new Phrase(" ", fontPDFDataAll));
                    cell.Colspan = 6;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    // ПРОГРЕССОР РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ


                    template = PdfTemplate.CreateTemplate(writer, 234f, 10f);
                    template.SetColorFill(new BaseColor((System.Drawing.Color.Blue)));
                    template.Rectangle(0, 0, 234f, 10f);


                    template.Fill();
                    writer.ReleaseTemplate(template);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                    cell.Colspan = 2;
                    cell.FixedHeight = 10f;
                    cell.BorderColor = new BaseColor(System.Drawing.Color.Blue);
                    table.AddCell(cell);
                    // ПРОГРЕССОР РЕЙТИНГА ЧЁТНОЙ КОМАНДЫ, ЗНАЧЕНИЕ РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ С ПУСТОЙ КОЛОНКОЙ ДО И ПОСЛЕ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 6;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ТОЧНОСТЬ НЕЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase("Точночть", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ЗНАЧЕНИЕ ТОЧНОСТИ ЧЁТНОЙ КОМАНДЫ
                    cell = new PdfPCell(new Phrase(accuracy[iparam].ToString() + " %", fontPDFDataAll));
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    // ТОЧНОСТЬ ЧЁТНОЙ КОМАНДЫ, ЗНАЧЕНИЕ ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ С ПУСТОЙ КОЛОНКОЙ ДО 
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 6;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);
                    // ПРОГРЕССОР ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ

                    progress = (234 * accuracy[iparam]) / 100;

                    template = PdfTemplate.CreateTemplate(writer, 234f, 10f);
                    template.SetColorFill(new BaseColor((System.Drawing.Color.Blue)));
                    template.Rectangle(0, 0, progress, 10f);


                    template.Fill();
                    writer.ReleaseTemplate(template);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                    cell.Colspan = 2;
                    cell.FixedHeight = 10f;
                    cell.BorderColor = new BaseColor(System.Drawing.Color.Blue);
                    table.AddCell(cell);
                    // ПРОГРЕССОР ТОЧНОСТИ ЧЁТНОЙ КОМАНДЫ, ЗНАЧЕНИЕ ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ С ПУСТОЙ КОЛОНКОЙ ДО ИПОСЛЕ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 6;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);
                }
            }
            doc.Add(table);
            doc.Close();
            //Открываем сформированный файл      
            Process.Start(FileName);
            return FileName;
        }

        /// <summary>
        ///  Ищем шрифты      
        /// </summary>
        /// <returns></returns>
        private static BaseFont RegisterFonts()
        {
            string[] fontNames = { "Calibri.ttf", "Arial.ttf", "Segoe UI.ttf", "Tahoma.ttf" };
            string fontFile = null;

            foreach (string name in fontNames)
            {
                fontFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), name);
                if (!File.Exists(fontFile))
                {
                    Debug.WriteLine("Шрифт \"{0}\" не найден.");
                    fontFile = null;
                }
                else break;
            }
            if (fontFile == null)
            {
                throw new FileNotFoundException("Ни один шрифт не найден.");
            }

            FontFactory.Register(fontFile);
            return BaseFont.CreateFont(fontFile, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        }

        private void PublishVK_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
