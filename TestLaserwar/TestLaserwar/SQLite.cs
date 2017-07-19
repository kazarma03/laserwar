using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace TestLaserwar
{
    class SQLite
    {
        /// <summary>
        /// Строка соединения с бд
        /// </summary>
        private string SqlConStr = "Data Source=laserwar.db; Version=3;";

        /// <summary>
        /// Открытие соединения с БД
        /// </summary>
        /// <param name="STRcon"> Строка соединения с БД </param>
        /// <returns> Реализованное подключение к БД </returns>
        private SQLiteConnection SqlConnect(string STRcon)
        {
            SQLiteConnection conn = new SQLiteConnection(STRcon);
            try
            {
                conn.Open();
                return conn;
            }
            catch (SQLiteException ex)
            {
                return conn;
            }
        }

        /// <summary>
        /// Закрытие соединения с БД
        /// </summary>
        /// <param name="conn"> Реализованное подключение к БД </param>
        /// <param name="state"> если true соединение закрывается, если false закрытие соединения и снятие блокировки с файла данных</param>
        private bool SqlCloseConnect(SQLiteConnection conn, bool state)
        {
            
            try
            {
                if(state)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Dispose();
                    return true;
                }
               
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Выполнение запроса SQL на редактирование БД
        /// </summary>        
        /// <param name="SqlCommand"> SQL запрос </param>
        /// /// <param name="conn"> Реализованное подключение к БД </param>
        /// <returns> Количество измененных строк </returns>
        public int ExecNonQuery(string SqlCommand, SQLiteConnection conn)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = SqlCommand;
            try
            {             
                return cmd.ExecuteNonQuery(); ;
            }
            catch (SQLiteException ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Конструктор SQL запросов NSERT
        /// </summary>
        /// <param name="tabel"> Имя таблицы БД </param>
        /// <param name="fild"> Строка содержащая набор полей через "," </param>
        /// <param name="val"> Набор значений полей через "," </param>
        /// <returns> true строки изменены, false зафиксировано 0 изменений </returns>
        public bool SqlInsertSingle(string tabel,string fild, string val)
        {
            SQLiteConnection conn = SqlConnect(SqlConStr);
            string SqlCommand = "INSERT INTO " + tabel + "(" + fild + ") VALUES (" + val + ");";
            int key = ExecNonQuery(SqlCommand, conn);
            SqlCloseConnect(conn, false);
            if (key > 0) return true;
            else return false;
        }

        /// <summary>
        /// Конструктор SQL запросов update
        /// </summary>
        /// <param name="tabel"> Имя таблицы БД </param>
        /// <param name="fild"> Строка содержащая поле "," </param>
        /// <param name="val"> значение "," </param>
        /// <param name="condition"> условие "," </param>
        /// <returns> true строки изменены, false зафиксировано 0 изменений </returns>
        public bool SqlUpdateSinglefield(string tabel, string fild, string val, string condition)
        {
            SQLiteConnection conn = SqlConnect(SqlConStr);
            string SqlCommand = "Update " + tabel + " set " + fild + "= '" + val + "' Where " + condition;

            int key = ExecNonQuery(SqlCommand, conn);
            SqlCloseConnect(conn, false);
            if (key > 0) return true;
            else return false;
        }

        /// <summary>
        /// Конструктор SQL запросов NSERT
        /// </summary>
        /// <param name="tabel"> Имя таблицы БД </param>
        /// <param name="fild"> Строка содержащая набор полей через "," </param>
        /// <param name="val"> Набор значений полей через "," </param>
        /// <returns> true строки изменены, false зафиксировано 0 изменений </returns>
        public int SqlInsertList(string tabel, string fild, List<string> val)
        {
            SQLiteConnection conn = SqlConnect(SqlConStr);
            string SqlCommand;
            int key =0;
            foreach (string param in val)
            {
                SqlCommand = "INSERT INTO " + tabel + "(" + fild + ") VALUES (" + param + ");";
                key += ExecNonQuery(SqlCommand, conn);
            }

            SqlCloseConnect(conn, false);
            return key;
        }

        /// <summary>
        /// Удаление таблицы из БД
        /// </summary>
        /// <param name="tabelName"> Имя таблицы БД </param>
        public void TabelDROP(string tabelName)
        {
            SQLiteConnection conn = SqlConnect(SqlConStr);
            string SqlCommand = "DROP TABLE IF EXISTS "+tabelName+";";
            ExecNonQuery(SqlCommand, conn);
            SqlCloseConnect(conn, false);
        }

        /// <summary>
        /// Созание таблицы в БД
        /// </summary>
        /// <param name="tabelName"> Имя таблицы БД </param>
        /// <param name="fild"> Строка содержащая набор полей вместе с типоми доп. параметрами через "," </param>
        public void TabelCreate(string tabelName, string fild)
        {
            SQLiteConnection conn = SqlConnect(SqlConStr);
            string SqlCommand = "CREATE TABLE " + tabelName + "(" + fild + ");";
            ExecNonQuery(SqlCommand, conn);
            SqlCloseConnect(conn, false);
        }

        /// <summary>
        /// Считыванем набор данных из БД
        /// </summary>
        /// <param name="SqlCommand"> SQL запрос </param>
        /// <returns> Набор данных содержащих выборку из БД </returns>
        public DataTable SqlRead(string SqlCommand)
        {
            SQLiteConnection conn = SqlConnect(SqlConStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = SqlCommand;
            DataTable data = new DataTable();
            data.Reset();
            SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
            ad.Fill(data);
            SqlCloseConnect(conn, false);
            return data;
        }

    }
}
