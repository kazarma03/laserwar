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

        class MyTable
        {
            public MyTable(string Name, int Size, string URL)
            {
                this.Name = Name;
                this.Size = Size;
                this.URL = URL;
            }
            public string Name { get; set; }
            public int Size { get; set; }
            public string URL { get; set; }
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
        /// Загрузка объекта JSON
        /// </summary>
        private void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            TextBoxJson.Visibility = Visibility.Hidden;
            LabelStateDownload.Visibility = Visibility.Hidden;
            TextBoxJson.Text = "";
            string url = TextBoxAddress.Text;
            WebClient client = new WebClient();
            JObject jObject;
            string data = null;

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
                if (!string.IsNullOrWhiteSpace(MainVal)) TextBoxJson.Text += "Error:" + Environment.NewLine + MainVal + Environment.NewLine;

                // Выводим URL игр           
                if (jObject["games"] != null)
                {
                    MainVal = "";
                    foreach (var Data in jObject["games"])
                    {
                        SubVal = (string)Data["url"];
                        val.Add((string)Data["url"]);
                        if (!string.IsNullOrWhiteSpace(SubVal)) MainVal += SubVal + Environment.NewLine;
                    }
                    //Если в JSON объекте присутствуют записи об играх выводим их в нижней форме экрана и заносим в БД laserwar.db таблица Games
                    if (!string.IsNullOrWhiteSpace(MainVal))
                    {
                        TextBoxJson.Text += "Game url:" + Environment.NewLine + MainVal + Environment.NewLine;

                        //Создаём БД
                        string tabelGames = "Games";
                        SQL.TabelDROP(tabelGames);
                        string fildGames = "id_game INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "game_name TEXT, "
                        + "game_date INTEGER";
                        SQL.TabelCreate(tabelGames, fildGames);
                        fildGames = "game_name, game_date";

                        string tabelTeams = "Teams";
                        SQL.TabelDROP(tabelTeams);
                        string fildTeams = "id_team INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "team_name TEXT";
                        SQL.TabelCreate(tabelTeams, fildTeams);
                        fildTeams = "team_name";

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
                        fildEvents = "game, team, gamer_name, rating, accuracy, shots";

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
                        val.Clear();                     

                        // Выводим  Название URL и размер звуковых файлов
                        if (jObject["sounds"] != null)
                        {
                            MainVal = "";
                            List<MyTable> result = new List<MyTable>(3);
                            foreach (var Data in jObject["sounds"])
                            {
                                SubVal = (string)Data["name"] + " " + (string)Data["url"] + " " + (string)Data["size"];
                                val.Add("'" + (string)Data["name"] + "'" + ", " + "'" + (string)Data["url"] + "'" + ", " + "'" + (string)Data["size"] + "'");                                
                               
                                //Добавление композиций в таблицу
                                result.Add(new MyTable((string)Data["name"], ((int)Data["size"])/1024, (string)Data["url"]));                                

                                if (!string.IsNullOrWhiteSpace(SubVal)) MainVal += SubVal + " byte" + Environment.NewLine;
                            }
                            dataGridSounds.ItemsSource = result;

                            //Если в JSON объекте присутствуют записи о звуковых файлах игр выводим их в нижней форме экрана и заносис в БД laserwar.db таблица Sounds
                            if (!string.IsNullOrWhiteSpace(MainVal))
                            {
                                TextBoxJson.Text += "Sounds:" + Environment.NewLine + MainVal + Environment.NewLine;

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

                        if (!string.IsNullOrWhiteSpace(TextBoxJson.Text))
                        {
                            TextBoxJson.Visibility = Visibility.Visible;
                            LabelStateDownload.Content = "Файл успешно загружен";
                        }
                        else
                        {
                            TextBoxJson.Visibility = Visibility.Hidden;
                            LabelStateDownload.Content = "Файл не содержит необходимых данных";
                        }
                        LabelStateDownload.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        TextBoxJson.Visibility = Visibility.Hidden;
                        LabelStateDownload.Content = "Файл не найден";
                        LabelStateDownload.Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private void dataGridSounds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
