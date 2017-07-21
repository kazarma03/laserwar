using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using System.Configuration;
using System.Windows.Media.Effects;


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
            RefMainMenu(true, false, false,false,false, false, false, false);
            
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
        /// Обновление форм при переходах
        /// </summary>
        /// <param name="dkey"> если true форма Загрузки отображается, если false форма скрыта </param>
        /// <param name="skey"> если true форма Звука отображается, если false форма скрыта</param>
        /// <param name="gkey"> если true форма Игра отображается, если false форма скрыта </param>
        private void RefMainMenu(bool dkey, bool skey, bool gkey, bool gdkey, bool gdgkey, bool VKkey, bool Skey, bool Akey)
        {
            if (dkey)
            {
                GridDownload.Visibility = Visibility.Visible;
                GridGamesDetail.Visibility = Visibility.Hidden;              
                but.ClickButtonDownLoad = true;
            }
            else
            {
                GridDownload.Visibility = Visibility.Hidden;           
                but.ClickButtonDownLoad = false;
            }
            if (skey)
            {
                GridSound.Visibility = Visibility.Visible;  
                but.ClickButtonSound = true;
            }
            else
            {
                GridSound.Visibility = Visibility.Hidden;             
                but.ClickButtonSound = false;
            }
            if (gkey)
            {
                GridGames.Visibility = Visibility.Visible;            
                but.ClickButtonGame = true;
            }
            else
            {
                GridGames.Visibility = Visibility.Hidden;            
                but.ClickButtonGame = false;
            }
            if (gdkey)
            {
                GridGamesDetail.Visibility = Visibility.Visible;
                but.ClickButtonGame = true;
            }
            else
            {
                GridGamesDetail.Visibility = Visibility.Hidden;
            }
            if (gdgkey)
            {
                
                GamerDetailGrid.Visibility = Visibility.Visible;
            }
            else
            {
                GamerDetailGrid.Visibility = Visibility.Hidden;
            }
            if (VKkey)
            {
                VKGrid.Visibility = Visibility.Visible;
            }
            else
            {
                VKGrid.Visibility = Visibility.Hidden;
            }
            if (Skey)
            {
                Shadow.Visibility = Visibility.Visible;

                BlurEffect objBlur = new BlurEffect();
                objBlur.Radius = 12;
                MainGrid.Effect = objBlur;
            }
            else
            {
                Shadow.Visibility = Visibility.Hidden;
                MainGrid.Effect = null;
            }
            if (Akey)
            {
                AnimationGrid.Visibility = Visibility.Visible;
            }
            else
            {
                AnimationGrid.Visibility = Visibility.Hidden;
            }
           

        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, false,false, false, false, false, false);
        }

        private void sounds_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, true, false,false, false, false, false, false);
        }

        private void games_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, false, true,false, false, false, false, false);
            GetGameData();
        }

        /// <summary>
        /// Парсим XML по ссылкам url
        /// </summary>
        /// <param name="val"> набор строк с url ссылками где находятся XML файлы с данными по играм </param>
        private void XMLparce(List<string> val)
        {
            SQLite SQL = new SQLite();
            //Создаём таблицы в БД для игр
            SQL.CreateDB();
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
                RefMainMenu(true, false, false, false, false, false, false, false);
            });
            
        }


        /// <summary>
        /// Загрузка объекта JSON
        /// </summary>
        private void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, false, false, false, false, true, true);
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

        /// <summary>
        /// Уникальный идентификатор игры для построения Таблицы детализации игры
        /// </summary>
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
                RefMainMenu(false, false, false, true, false, false, false, false);
                ID = id_game;
                FillGameDetail(id_game);
            }
         }


        //****************************************************************
        //*******************   Детализаци  ИГРЫ   ***********************
        //****************************************************************

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
                string Rating = DetailGameTabel.Rows[i][3].ToString(); ;
                string Accuracy = Convert.ToString(Convert.ToDouble(DetailGameTabel.Rows[i][4]) * 100) + " %";
                string Shots =DetailGameTabel.Rows[i][5].ToString(); ;

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
            RefMainMenu(false, false, true, false, false, false, false, false);

            GetGameData();
        }

        // поля для обновления данных по игроку через таблицу детализации игр
        // сохраняем значения полей до изменения        
        
        /// <summary>
        /// Уникальный идентификаторв таблице Events однозначно задающий редактируемого игрока
        /// </summary>
        int Prep_id;

        int Row_id;

        string Prep_Name;
        string Prep_Rating;
        string Prep_Accuracy;
        string Prep_Shots;

        private string convertTexttoInttoText(string newText, string prevText, bool percent)
        {
            string prom_Text = "";
            // Убираем из текста все символы кроме цифр
            for (int i = 0; i < newText.Length; i++)
            {
                if (Char.IsDigit(newText[i])) prom_Text += newText[i];
            }

            int new_IntText = 0;
            if (prom_Text != "")
            {
                try
                {
                    new_IntText = Convert.ToInt32(prom_Text);
                    newText = prom_Text;
                    if (percent)
                    {
                        if (new_IntText > 100) newText = "100";
                        newText += " %";
                    }
                }
                catch
                {
                    newText = prevText;
                }
            }
            else
            {
                newText = prevText;
            }

            return newText;
        }

        /// <summary>
        /// Подготовка к изменению данных в ячейки таблицы детализации игр
        /// </summary>
        private void dataGridDetailGame_PreparingCellForEdit(object sender, System.Windows.Controls.DataGridPreparingCellForEditEventArgs e)
       {
            Row_id= dataGridDetailGame.Items.IndexOf(dataGridDetailGame.CurrentItem);
            Prep_id = ObsCollectionDetailGameTabel[Row_id].Id;

            //получаем уникальный идентификатор игры
            SQLite SQL = new SQLite();
            DataTable GamerProperty = new DataTable();
            //получаем имя детализируемой игры
            string SQLQuery = "select Events.gamer_name, Events.rating, Events.accuracy, Events.shots from Events where Events.id ='" + Prep_id + "'";
            GamerProperty = SQL.SqlRead(SQLQuery);

            Prep_Name= GamerProperty.Rows[0][0].ToString();

            Prep_Rating = GamerProperty.Rows[0][1].ToString();

            Prep_Accuracy = Convert.ToString(Convert.ToDouble(GamerProperty.Rows[0][2]) * 100) + " %";

            Prep_Shots = GamerProperty.Rows[0][3].ToString();
        }

        /// <summary>
        /// Завершение изменений данных в ячейки таблицы детализации игр
        /// </summary>
        private void dataGridDetailGame_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {

            //считываем значения полей после потери фокуса
            string Aft_GamerName = ObsCollectionDetailGameTabel[Row_id].GamerName;
            string Aft_Rating = ObsCollectionDetailGameTabel[Row_id].Rating;
            string Aft_Accuracy = ObsCollectionDetailGameTabel[Row_id].Accuracy;
            string Aft_Shots = ObsCollectionDetailGameTabel[Row_id].Shots;

            if (Aft_GamerName != Prep_Name)
            {
                //MessageBox.Show("изменено имя");

                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "gamer_name";
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, Aft_GamerName, "id ='" + Prep_id + "'");
                ObsCollectionDetailGameTabel[Row_id].GamerName = Aft_GamerName;
            }

            //сравниваем значение до изменений с текущем значением
            //если изменения различаются то вносим изменения в БД
            if (Aft_Rating != Prep_Rating)
            {
                //MessageBox.Show("изменён рейтинг");
                Aft_Rating = convertTexttoInttoText(Aft_Rating, Prep_Rating, false);

                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "rating";
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, Aft_Rating, "id ='" + Prep_id + "'");
                ObsCollectionDetailGameTabel[Row_id].Rating = Aft_Rating;
            }

            if (Aft_Accuracy != Prep_Accuracy)
            {
                // MessageBox.Show("изменёна точность");
                Aft_Accuracy = convertTexttoInttoText(Aft_Accuracy, Prep_Accuracy, true);

                ObsCollectionDetailGameTabel[Row_id].Accuracy = Aft_Accuracy;

                Aft_Accuracy = Aft_Accuracy.Substring(0, Aft_Accuracy.Length - 2);
                double new_Accuracy = 0;
                new_Accuracy = Convert.ToDouble(Aft_Accuracy) / 100;
                // после нормализации значение содержит "," а не "." а в БД разделитель "."                
                Aft_Accuracy = new_Accuracy.ToString();
                Aft_Accuracy = Aft_Accuracy.Replace(",", ".");

                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "accuracy";
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, Aft_Accuracy, "id ='" + Prep_id + "'");
            }

            if (Aft_Shots != Prep_Shots)
            {
                Aft_Shots = convertTexttoInttoText(Aft_Shots, Prep_Shots, false);

                // MessageBox.Show("изменёны выстрелы");
                SQLite SQL = new SQLite();
                string tabelEvents = "Events";
                string fildEvents = "shots";
                SQL.SqlUpdateSinglefield(tabelEvents, fildEvents, Aft_Shots, "id ='" + Prep_id + "'");
                ObsCollectionDetailGameTabel[Row_id].Shots = Aft_Shots;
            }
        }




        /// <summary>
        ///  Нажатие кнопки сохранения детализации игры в PDF
        /// </summary>
        private void SavePDF_Click(object sender, RoutedEventArgs e)
        {
            PDF pdf = new TestLaserwar.PDF();
            Thread PDF_CREATOR = new Thread(() => pdf.CreatePDF(ID));
            PDF_CREATOR.Start();
            
        }

        private void PublishVK_Click(object sender, RoutedEventArgs e)
        {

            RefMainMenu(false, false, false, false, false, true, true, false);
            webBrowser.Navigate(String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=page&response_type=token", ConfigurationSettings.AppSettings["VKAppId"], ConfigurationSettings.AppSettings["VKScope"], ConfigurationSettings.AppSettings["VKRedirectUri"]));

        }

        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            // Удаляем #
            var clearUriFragment = e.Uri.Fragment.Replace("#", "").Trim();
            // разбиваем строку на фрагменты
            var parameters = HttpUtility.ParseQueryString(clearUriFragment);
            // Пытаемся получить токен
            Vk.AccessToken = parameters.Get("access_token");
            // Пытаемся получить идентификатор авторизованного пользователя
            Vk.UserId = parameters.Get("user_id");

            if (Vk.AccessToken != null && Vk.UserId != null)
            {
                webBrowser.Visibility = Visibility.Hidden;
            }

            if (Vk.AccessToken != "")
            {
                try
                {


                    vkWallPost vk = new vkWallPost(Vk.AccessToken);   //создается класс с методами для вконтакте, ему передается токен

                    //Помещаем метод в поток, т.к. в противном случае будет тормозить
                    var thread = new Thread(() =>
                    {
                    //Отправляем фотку в Вконтакте
                    string s=vk.AddWallPost(2549171, "Т7777777777777777" + DateTime.Now.ToString("dd MMMM yyyy | HH:mm: ss"), @"~\..\resources\downloaded_sound.png");
                    });
                    thread.Start();

                }
                catch (Exception) { }
                finally
                {
                }
            }
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            webBrowser.OpacityMask = null;
            webBrowser.Opacity = 1;
        }
        private void CloseVK_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, false, false, true, false, false, false, false);
            FillGameDetail(ID);
        }

        //****************************************************************
        //*****************   Редактирование игрока  *********************
        //****************************************************************

        private void DisAgree_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, false, false, true, false, false, false, false);
            FillGameDetail(ID);
        }

        private void Agree_Click(object sender, RoutedEventArgs e)
        {           
            SQLite SQL = new SQLite();
            string tabelEvents = "Events";
            TextBoxGamerAccuracy.Text = convertTexttoInttoText(TextBoxGamerAccuracy.Text, Convert.ToString(Convert.ToDouble(GamerProperty.Rows[0][3]) * 100) + " %", true);
            TextBoxGamerRating.Text = convertTexttoInttoText(TextBoxGamerRating.Text, GamerProperty.Rows[0][2].ToString(), false);
            TextBoxGamerShots.Text = convertTexttoInttoText(TextBoxGamerShots.Text, GamerProperty.Rows[0][4].ToString(), false);
            //Нормализуем точность
            double Accuracy = Convert.ToDouble(TextBoxGamerAccuracy.Text.Substring(0,TextBoxGamerAccuracy.Text.Length-2));
            Accuracy = Accuracy / 100;
            string STRnew_Accuracy = Accuracy.ToString();
            //меняем точку на запятую,т.к. бд при вовторном запросе вернёт 0 если в числе будет запятая
            STRnew_Accuracy = STRnew_Accuracy.Replace(",", ".");

            SQL.SqlUpdateSinglefield(tabelEvents, "gamer_name", TextBoxGamerName.Text, "id ='" + Prep_id + "'");

            SQL.SqlUpdateSinglefield(tabelEvents, "rating", TextBoxGamerRating.Text, "id ='" + Prep_id + "'");
         
            SQL.SqlUpdateSinglefield(tabelEvents, "accuracy", STRnew_Accuracy, "id ='" + Prep_id + "'");
       
            SQL.SqlUpdateSinglefield(tabelEvents, "shots", TextBoxGamerShots.Text, "id ='" + Prep_id + "'");

            RefMainMenu(false, false, false, true, false, false, false, false);
            FillGameDetail(ID);

        }

        /// <summary>
        /// Сохраняем текущие атрибуты пользователя при редактировании через отдельную форму
        /// </summary>
        DataTable GamerProperty = new DataTable();

        /// <summary>
        /// Редактирование информации по игроку при двойном нажатии на строку
        /// </summary>
        private void dataGridDetailGame_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridDetailGame.CurrentItem != null)
            {
                Prep_id = ObsCollectionDetailGameTabel[dataGridDetailGame.Items.IndexOf(dataGridDetailGame.CurrentItem)].Id;
                //получаем уникальный идентификатор игры
                SQLite SQL = new SQLite();
                //получаем имя детализируемой игры
                string SQLQuery = "select Teams.team_name, Events.gamer_name, Events.rating,Events.accuracy,Events.shots from Events inner join Teams on Teams.id_team=Events.team where Events.id ='" + Prep_id + "'";
                GamerProperty = SQL.SqlRead(SQLQuery);
                CommandName.Content = GamerProperty.Rows[0][0].ToString();
                TextBoxGamerName.Text = GamerProperty.Rows[0][1].ToString();
                TextBoxGamerRating.Text = GamerProperty.Rows[0][2].ToString();
                TextBoxGamerAccuracy.Text = Convert.ToString(Convert.ToDouble(GamerProperty.Rows[0][3])*100) +" %";
                TextBoxGamerShots.Text = GamerProperty.Rows[0][4].ToString();
                RefMainMenu(false, false, false, false, true, false, true, false);
            }
        }

        private void TextBoxGamerAccuracy_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBoxGamerAccuracy.Text = convertTexttoInttoText(TextBoxGamerAccuracy.Text, Convert.ToString(Convert.ToDouble(GamerProperty.Rows[0][3]) * 100) + " %", true);
        }

        private void TextBoxGamerRating_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBoxGamerRating.Text = convertTexttoInttoText(TextBoxGamerRating.Text, GamerProperty.Rows[0][2].ToString(), false);
        }

        private void TextBoxGamerShots_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBoxGamerShots.Text = convertTexttoInttoText(TextBoxGamerShots.Text, GamerProperty.Rows[0][4].ToString(), false);
        }


        //****************************************************************
        //*************************   Ошибки  ****************************
        //****************************************************************

        private void CloseMessage_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, false, false, false, false, false, false);
        }

    }
}
