using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SQLite;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

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
            download.Background = new SolidColorBrush(Colors.Blue);
            dataGridSounds.ItemsSource = bindSoundTabel;
        }

        Thread th;

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
                download.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                GridDownload.Visibility = Visibility.Hidden;
                download.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            }
            if (skey)
            {
                GridSound.Visibility = Visibility.Visible;
                sounds.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                GridSound.Visibility = Visibility.Hidden;
                sounds.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            }
            if (gkey)
            {
                GridGames.Visibility = Visibility.Visible;
                games.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                GridGames.Visibility = Visibility.Hidden;
                games.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
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

        //public class DataItem
        //{
        //    public string name { get; set; }
        //    public int size { get; set; }
        //    public string URL { get; set; }
        //    public int DownloadProgress { get; set; }
        //}

        /// <summary>
        /// Считываем объект JSON по url парсим его и обрабатываем содержимое
        /// </summary>
        /// <param name="url"> ссылка по которой находиться объект JSON </param>
        public void DovnloadJSON( string url)
        {
            bindSoundTabel.Clear();
            
            //Скрываем интерфейс поиска данных
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                dataGridSounds.Items.Refresh();
                TextBoxJson.Visibility = Visibility.Hidden;
                LabelStateDownload.Visibility = Visibility.Hidden;
            });
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
                        bindSoundTabel.Add(new SoundTable((string)Data["name"], ((int)Data["size"]) / 1024, (string)Data["url"])); 
                        //собираем полную строку для вывода на форму
                        if (!string.IsNullOrWhiteSpace(SubVal)) MainVal += SubVal + " byte" + Environment.NewLine;
                    }
                    //Добавление композиций в таблицу
                    //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    //{
                    //    dataGridSounds.ItemsSource = result;
                    //});

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
                dataGridSounds.Items.Refresh();
                LabelStateDownload.Visibility = Visibility.Visible;
            });
        }

        /// <summary>
        /// Загрузка объекта JSON
        /// </summary>
        private void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            TextBoxJson.Visibility = Visibility.Hidden;
            LabelStateDownload.Visibility = Visibility.Hidden;
            TextBoxJson.Text = "";
            string url = TextBoxAddress.Text;
            th = new Thread(() =>DovnloadJSON(url));
            th.Start();
        }
        class SoundTable 
        {
            public SoundTable(string Name, int Size, string URL)
            {
                this.Name = Name;
                this.Size = Size;
                this.URL = URL;                
            }
            public string Name { get; set; }
            public int Size { get; set; }
            public string URL { get; set; }
            public int DownloadProgress { get; set; }
            public int PlayProgress { get; set; }
            public int Sel { get; set; }
            public bool OnOfDowlLoad { get; set; }           
            public Visibility OnOfPercent { get; set; }
 
            public Visibility OnOfDownloadProgress { get; set; }
            public bool OnOfPlay { get; set; }
            public Visibility OnOfTimePlay { get; set; }
            public Visibility OnOfPlayProgress { get; set; }
        }

        List<SoundTable> bindSoundTabel = new List<SoundTable>();


        private void PlaySound_Click(object sender, RoutedEventArgs e)
        {
            int ind = dataGridSounds.Items.IndexOf(dataGridSounds.CurrentItem);
            bindSoundTabel[ind].DownloadProgress = 70;
            bindSoundTabel[ind].PlayProgress = 40;
            bindSoundTabel[ind].OnOfPlay = true;
            bindSoundTabel[ind].Name = "ooooooooooooooooooooooo" ;

            bindSoundTabel[ind].OnOfDownloadProgress = Visibility.Hidden;
            bindSoundTabel[ind].OnOfPercent = Visibility.Hidden;
            bindSoundTabel[ind].OnOfTimePlay = Visibility.Hidden;
            bindSoundTabel[ind].OnOfPlayProgress = Visibility.Hidden;

            dataGridSounds.Items.Refresh();            
        }



        private void DownloadSound_Click(object sender, RoutedEventArgs e)
        {
            int ind = dataGridSounds.Items.IndexOf(dataGridSounds.CurrentItem);
            bindSoundTabel[ind].OnOfPlay = true;
            dataGridSounds.Items.Refresh();
        }
    }
}
