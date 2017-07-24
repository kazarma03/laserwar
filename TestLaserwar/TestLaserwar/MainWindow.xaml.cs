using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using System.Xml;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;


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

        Error Err = new Error();

        Interface VisabilityInterface = new Interface();
        Visibility vi = Visibility.Visible;
        Visibility hi = Visibility.Hidden;

        /// <summary>
        /// Загрузка главного окна
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefMainMenu(vi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
            
            dataGridSounds.ItemsSource = bindSoundTabel;
            dataGridGame.ItemsSource =bindGameTabel;
            DetailGame.DataContext = LabelDetail;
            TextBoxError.DataContext = Err;

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

            GridDownload.DataContext = VisabilityInterface;
            GridSound.DataContext = VisabilityInterface;
            GridGames.DataContext = VisabilityInterface;
            GridGamesDetail.DataContext = VisabilityInterface;
            GamerDetailGrid.DataContext = VisabilityInterface;
            VKGrid.DataContext = VisabilityInterface;
            AuthenticationGrid.DataContext = VisabilityInterface;
            CreatePostVK.DataContext = VisabilityInterface;
            Shadow.DataContext = VisabilityInterface;
            ShadowSub.DataContext = VisabilityInterface;
            AnimationGrid.DataContext = VisabilityInterface;
            MessageGrid.DataContext = VisabilityInterface;
        }

        /// <summary>
        /// поток в котором выполняется загрузка Json объекта
        /// </summary>
        Thread th;

        /// <summary>
        /// экземпляр класса с полями для управления повидением кнопок
        /// </summary>
        buttons but = new buttons();

        int PrevMainFormKey =1;

        /// <summary>
        /// Обновление форм при переходах
        /// </summary>
        /// <param name="dkey">Фрма загрузки JSON</param>
        /// <param name="skey">Форма звука</param>
        /// <param name="gkey">Форма игр</param>
        /// <param name="gdkey">Форма детализации игр</param>
        /// <param name="gdgkey">Форма детализации параметров игрока</param>
        /// <param name="VKkey">Форма ВК</param>
        /// <param name="VKAkey">Форма Аутентификации ВК на форме ВК</param>
        /// <param name="VKSkey">Форма отправки Поста в ВК на формме ВК</param>
        /// <param name="Skey">Затемнение главных форм</param>
        /// <param name="SSkey">Затемнение вспомогательных форм</param>
        /// <param name="Akey">Анимация</param>
        /// <param name="Mkey">Сообщение об ошибке</param>
        private void RefMainMenu(Visibility dkey, Visibility skey, Visibility gkey, Visibility gdkey, Visibility gdgkey, Visibility VKkey, Visibility VKAkey, Visibility VKSkey, Visibility Skey, Visibility SSkey, Visibility Akey, Visibility Mkey )
        {
            VisabilityInterface.dkey = dkey;
            if(dkey ==Visibility)but.ClickButtonDownLoad = true;
            else but.ClickButtonDownLoad = false;

            VisabilityInterface.skey = skey;
            if (skey == Visibility) but.ClickButtonSound = true;
            else but.ClickButtonSound = false;
            
            VisabilityInterface.gkey = gkey;
            if (gkey == Visibility) but.ClickButtonGame = true;
            else but.ClickButtonGame = false;

            VisabilityInterface.gdkey = gdkey;
            if (gdkey == Visibility) but.ClickButtonGame = true;

            VisabilityInterface.gdgkey = gdgkey;

            VisabilityInterface.VKkey = VKkey;
  
            VisabilityInterface.VKAkey = VKAkey;

            VisabilityInterface.VKSkey = VKSkey;

            VisabilityInterface.Skey = Skey;
            if (Skey == Visibility)
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    BlurEffect objBlur = new BlurEffect();
                    objBlur.Radius = 7;
                    MainGrid.Effect = objBlur;
                });
            else         
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MainGrid.Effect = null;
                });

            VisabilityInterface.SSkey = SSkey;
            if (SSkey == Visibility)
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    BlurEffect objBlur = new BlurEffect();
                    objBlur.Radius = 7;
                    MainGrid.Effect = objBlur;
                });
            else
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MainGrid.Effect = null;
                });

            VisabilityInterface.Akey = Akey;


            VisabilityInterface.Mkey = Mkey;        
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 1;
            RefMainMenu(vi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
        }

        private void sounds_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 2;
            RefMainMenu(hi, vi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
        }

        private void games_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 3;
            RefMainMenu(hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
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

            System.Net.WebClient client = new System.Net.WebClient();
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
                        LabelStateDownload.Visibility = Visibility.Visible;
                    });
                    but.OnOfButtonSound = true;
                    but.OnOfButtonGame = true;
                    RefMainMenu(vi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi);                   
                }
                else
                {
                    // LabelStateDownload.Content = "Файл не содержит необходимых данных";
                    //Отображаем сообщение об ошибке
                    /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                    RefMainMenu(vi, hi, hi, hi, hi, hi, hi, hi, vi, hi, hi, vi);
                    Err.ErrorMessage = "Файл не содержит необходимых данных.";
                }
            }
            else
            {             
                //LabelStateDownload.Content = "Файл не найден";
                //Отображаем сообщение об ошибке
                /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                RefMainMenu(vi, hi, hi, hi, hi, hi, hi, hi, vi, hi, hi, vi);
                Err.ErrorMessage = "Файл не найден. Проверьте корректность URL, начичие выхода в интернет и повторите попытку.";
            }            
        }

        /// <summary>
        /// Загрузка объекта JSON
        /// </summary>
        private void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(vi, hi, hi, hi, hi, hi, vi, vi, vi, hi, vi, hi);
           // but.OnOfDownloadJsonButtton = false;
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
            using ( System.Net.WebClient client = new System.Net.WebClient())
            {
                Stopwatch sw = new Stopwatch();
                sw.Reset();
                string FileName = bindSoundTabel[ind].URL.Substring(bindSoundTabel[ind].URL.LastIndexOf(@"/") + 1);
                string FilePath = bindSoundTabel[ind].URL;

                //создание и перегрузка события завершения загрузки асинхронного загрузчика
                client.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) => Completed(sender, e, ind, FileName));
                //создание и перегрузка события изменения состояния загрузки асинхронного загрузчика
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) => ProgressChanged(sender, e, ind, sw));

              
                //string FilePath = "http://ru.download.nvidia.com/Windows/384.76/384.76-desktop-win10-64bit-international-whql.exe";
                //string FilePath = "http://best-muzon.cc/dl/CZisIgYr9nmLLV9RcFvBRA/1500238531/songs12/2017/01/luis-fonsi-feat.-daddy-yankee-despacito-(best-muzon.cc).mp3";
                // Запуск Stopwatch для дальнейшего расчета скорости загрузки файла
                sw.Start();
                try
                {
                    // начинаем загрузку файла асинхронно
                    client.DownloadFileAsync(new Uri(FilePath), FileName);
                    //client.DownloadFileAsync(new Uri(" "), FileName);
                }
                catch (Exception ex)
                {

                    // MessageBox.Show(ex.Message);
                    //Отображаем сообщение об ошибке
                    /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                    RefMainMenu(hi, vi, hi, hi, hi, hi, hi, hi, vi, hi, hi, vi);
       
                    Err.ErrorMessage = "Возникли ошибки при загрузке файла. Проверьте корректность URL, начичие выхода в интернет и повторите попытку.";
                    bindSoundTabel[ind].OnOfDowlLoad = true;
                    bindSoundTabel[ind].OnOfDownloadProgress = Visibility.Hidden;
                    bindSoundTabel[ind].OnOfPercent = Visibility.Hidden;                
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
        private void Completed(object sender, AsyncCompletedEventArgs e, int ind, string FileName)
        {
            string[] findFiles = new string[1];
            if (e.Cancelled == true)
            {
                //tесли было принудительное завершение загрузки файла
                //MessageBox.Show("Download has been canceled.");
            }
            else
            {
                // Находим загруженный файл
                findFiles = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), FileName);
                if (findFiles.LongLength == 0)
                {

                    // MessageBox.Show(ex.Message);
                    //Отображаем сообщение об ошибке
                    /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                    RefMainMenu(hi, vi, hi, hi, hi, hi, hi, hi, vi, hi, hi, vi);
                    Err.ErrorMessage = "Возникли ошибки при загрузке файла.";
                    bindSoundTabel[ind].OnOfDowlLoad = true;
                    bindSoundTabel[ind].OnOfDownloadProgress = Visibility.Hidden;
                    bindSoundTabel[ind].OnOfPercent = Visibility.Hidden;
                }
                else
                if (findFiles.LongLength != 0)
                {
                    //Определяем размер скачанного файла
                    long length = new System.IO.FileInfo(findFiles[0]).Length;
                    if (length == 0)
                    {

                        // MessageBox.Show(ex.Message);
                        //Отображаем сообщение об ошибке
                        /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                        RefMainMenu(hi, vi, hi, hi, hi, hi, hi, hi, vi, hi, hi, vi);
                        Err.ErrorMessage = "Возникли ошибки при загрузке файла. Проверьте начичие выхода в интернет и повторите попытку.";
                        bindSoundTabel[ind].OnOfDowlLoad = true;
                        bindSoundTabel[ind].OnOfDownloadProgress = Visibility.Hidden;
                        bindSoundTabel[ind].OnOfPercent = Visibility.Hidden;
                        // удаляем скачанный файл если его размер 0
                        File.Delete(findFiles[0]);
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
                PrevMainFormKey = 4;
                //получаем уникальный идентификатор игры
                int id_game = bindGameTabel[dataGridGame.Items.IndexOf(dataGridGame.CurrentItem)].id_Game;
                int count = bindGameTabel.Count;
                RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
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
            RefMainMenu(hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi, hi);

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
            Thread PDF_CREATOR = new Thread(() => pdf.CreatePDF(ID, true));
            PDF_CREATOR.Start();
            
        }

     
        //****************************************************************
        //*****************   Редактирование игрока  *********************
        //****************************************************************


        private void DisAgree_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
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

            RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
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
                RefMainMenu(hi, hi, hi, vi, vi, hi, hi, hi, vi, hi, hi, hi);
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
            if (PrevMainFormKey == 1) RefMainMenu(vi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
            if (PrevMainFormKey == 2) RefMainMenu(hi, vi, hi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
            if (PrevMainFormKey == 3) RefMainMenu(hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi, hi);
            if (PrevMainFormKey == 4) RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
            if (PrevMainFormKey == 5) RefMainMenu(hi, hi, hi, vi, hi, vi, vi, hi, vi, hi, hi, hi);
        }


        //****************************************************************
        //***************************  VK  *******************************
        //****************************************************************




        /// <summary>
        /// коллекция групп ВК
        /// </summary>
        List<VKGroup> Groups = new List<VKGroup>();
        /// <summary>
        /// Экземпляр класса VkApi
        /// </summary>
        VkApi api;

        /// <summary>
        /// открытие формы отправки поста в ВК
        /// </summary>
        private void PublishVK_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 5;
            RefMainMenu(hi, hi, hi, vi, hi, vi, vi, hi, vi, hi, hi, hi);
        }

        /// <summary>
        /// Клик на вход в ВК
        /// </summary>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {            
            RefMainMenu(hi, hi, hi, vi, hi, vi, vi, hi, vi, vi, vi, hi);
            ComboBoxVK.ItemsSource = Groups;
    
            Thread VK_connector = new Thread(FormFillSenderVK);
            VK_connector.Priority = ThreadPriority.AboveNormal;
            VK_connector.Start();
        }

        /// <summary>
        /// Заполнение формы постинга в ВК
        /// </summary>
        private void FormFillSenderVK()
        {
            // проходим аутентификацию
            api = ConnectVK();
            if (api.Token != null)
            {
                // формируем список групп аутентифицированного пользователя   
                var groups = api.Groups.Get(new GroupsGetParams() { UserId = api.UserId.Value, Extended = true, Filter = null, Fields = GroupsFields.All }).ToList();
                foreach (var g in groups)
                {
                    // ComboBoxVK.Items.Add(new KeyValuePair<long, string>(g.Id, g.Name));
                    Groups.Add(new VKGroup(g.Id, g.Name));
                }
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    //Отображаем форму постинга
                    ComboBoxVK.SelectedIndex = 0;
                });
                RefMainMenu(hi, hi, hi, vi, hi, vi, hi, vi, vi, hi, hi, hi);

            }
            else
            {            
                //Отображаем сообщение об ошибке
                /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                RefMainMenu(hi, hi, hi, vi, hi, vi, vi, hi, vi, vi, hi, vi);
                Err.ErrorMessage = "Не удалось установить соединение. Проверьте правильности имени пользователя и пароля.";            
            }
        }

        /// <summary>
        /// Закрытие формы аутентификации
        /// </summary>
        private void CloseVK_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 4;
            RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
        }

        /// <summary>
        /// Аутентификация в ВК по паре логин пароль + ID приложения
        /// </summary>
        private VkApi ConnectVK()
        {
            VkApi api = new VkApi();
            string login="";
            string password="";
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                login = LoginVk.Text;
                password = PasswordVK.Password;
            });
            Thread.Sleep(1000);
            try
            {
                api.Authorize(new ApiAuthParams
                {
                    ApplicationId = 6120538,
                    Login = login,
                    Password = password,
                    Settings = Settings.All
                });
            }
            catch (Exception ex)
            {               
                return api;
            }
            
            return api;           
        }


        /// <summary>
        /// Постинг в ВК
        /// </summary>
        private void SendVKpost()
        {
            string PDFFileName = "";

            var wc = new WebClient();
            List<MediaAttachment> attachments = new List<MediaAttachment>();
            bool keyDoc = false;
            bool keyScreen = false;
            VKGroup Group = new VKGroup(0,"");
            string Mes = "";
            int ErrorKey = 0;

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Mes = TextBoxVK.Text;
                if (DocVk.IsChecked ?? false) keyDoc = true;       
                if (ScreenVK.IsChecked ?? false) keyScreen = true;
                Group = (VKGroup)ComboBoxVK.SelectedValue;
            });
            Thread.Sleep(1000);

            //Если необходимо добавидь документ, как вложение к посту
            if (keyDoc)
            {
                PDF pdf = new PDF();
                PDFFileName = pdf.CreatePDF(ID, false);

                // Загрузить файл.
                wc = new WebClient();
                try
                {
                    //// Получить адрес сервера для загрузки.
                    var uploadDocServer = api.Docs.GetUploadServer();
                    var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadDocServer.UploadUrl, PDFFileName));
                    // Сохранить загруженный файл
                    var doc = api.Docs.Save(responseFile, PDFFileName);
                    attachments.Add((MediaAttachment)doc[0]);
                }
                catch (Exception ex)
                {
                    ErrorKey = 1;
                }
            }  
            //Если необходимо добавить скриншот в пост
            if (keyScreen)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    // Переделываем указанный элемент в .png
                    ScreenShotElement.SaveToPNG(ParentGrid, "ScreenShot");
                });

                Thread.Sleep(1000);
                try
                {

                    var getAlbums = api.Photo.GetAlbums(new PhotoGetAlbumsParams { });

                    long AlbumId = 0;
                    bool albumFind = false;
                    //Ищем альбом у пользователя
                    for (int i = 0; i < getAlbums.Count; i++)
                    {
                        if (getAlbums[i].Title == "TestLaserwarGames")
                        {
                            albumFind = true;
                            AlbumId = getAlbums[i].Id;
                            break;
                        }
                    }
                    //если альбом не был найден, создаём новый
                    if (!albumFind)
                    {
                        var createAlbum = api.Photo.CreateAlbum(new PhotoCreateAlbumParams
                        {
                            Title = "TestLaserwarGames"
                        });
                        AlbumId = createAlbum.Id;
                    }

                    // Загрузить фото.
                    wc = new WebClient();

                    // Получить адрес сервера для загрузки.
                    var uploadPhotoServer = api.Photo.GetUploadServer(AlbumId);
                    // Загрузить файл.
                    var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadPhotoServer.UploadUrl, @"ScreenShot.png"));
                    // Сохранить загруженный файл
                    var photos = api.Photo.Save(new PhotoSaveParams
                    {
                        SaveFileResponse = responseFile,
                        AlbumId = AlbumId
                    });
                    attachments.Add((MediaAttachment)photos[0]);
                }
                catch (Exception ex)
                {
                    ErrorKey = 2;
                }
            }            

            double? la = null;
            double? lo = null;

            //GeoCoordinate location = new GeoCoordinate();

            //var immediate = new ImmediateLocation(x => location = x);
            //immediate.GetLocation();

            //if (location.IsUnknown == false)
            //{
            //    la = location.Latitude;
            //    lo = location.Longitude;

            //}
            //else
            //{
            //    IpLocation _IpLocation = new IpLocation();
            //    _IpLocation.GetCountryByIP();
            //    _IpLocation.Lat = _IpLocation.Lat.Replace(".", ",");
            //    _IpLocation.Lon = _IpLocation.Lon.Replace(".", ",");
            //    la = Convert.ToDouble(_IpLocation.Lat);
            //    lo = Convert.ToDouble(_IpLocation.Lon);
            //}

            try
            {
                // Постинг на стену
                var post = api.Wall.Post(new WallPostParams
                {
                    OwnerId = -Group.ID,
                    FriendsOnly = true,
                    FromGroup = false,
                    Message = Mes,
                    Attachments = attachments,
                    Services = null,
                    Signed = true,
                    PublishDate = null,
                    Lat = la,
                    Long = lo,
                    PlaceId = null,
                    PostId = null,
                    CaptchaSid = null,
                    CaptchaKey = null
                });
                // если создавались вложения и скрин, удаляем их
                if(keyDoc)File.Delete(PDFFileName);
                if(keyScreen)File.Delete(@"ScreenShot.png");
             
                RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
               
            }
            catch (Exception ex)
            {
                // если создавались вложения и скрин, удаляем их
                if (keyDoc) File.Delete(PDFFileName);
                if (keyScreen) File.Delete(@"ScreenShot.png");
              
                Err.ErrorMessage = "Не удалось опубликовать пост. Проверьте начичие выхода в интернет и повторите попытку.";
                if (ErrorKey == 1) Err.ErrorMessage += " Ошибки при загрузке вложения.";
                if (ErrorKey == 2) Err.ErrorMessage += " Ошибки при загрузке скриншота программы.";
                //Отображаем сообщение об ошибке
                /// RefMainMenu(false, false, false, false, false, true, true, false, true, false, false, false);
                RefMainMenu(hi, hi, hi, vi, hi, vi, hi, vi, vi, vi, hi, vi);              
            }
        }

        /// <summary>
        /// Опубликовать пост
        /// </summary>
        private void SendVK_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 4;
            RefMainMenu(hi, hi, hi, vi, hi, vi, hi, vi, vi, vi, vi, hi);
          
            ComboBoxVK.ItemsSource = Groups;

            Thread VK_Posting = new Thread(SendVKpost);
            VK_Posting.Start();
            //SendVKpost();          
        }

        /// <summary>
        /// Выйти из публикации ВК
        /// </summary>
        private void CloseSend_Click(object sender, RoutedEventArgs e)
        {
            PrevMainFormKey = 4;
            api.Dispose();
            RefMainMenu(hi, hi, hi, vi, hi, hi, hi, hi, hi, hi, hi, hi);
        }


    }
}
