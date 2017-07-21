using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace TestLaserwar
{
    class PDF
    {

        /// <summary>
        /// Сохраняем детализацию игры в PDF
        /// </summary>
        /// <param name="ID"> Уникальный монер детилизируемой игры</param>
        /// <returns>Имя файла</returns>
        public string CreatePDF(int ID)
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
            rec3.BackgroundColor = new BaseColor(System.Drawing.Color.WhiteSmoke);
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

            table.AddCell(CreateHeader("Игрок", fontPDFColumnName));
            table.AddCell(CreateHeader("Рейтинг", fontPDFColumnName));
            table.AddCell(CreateHeader("Точность", fontPDFColumnName));
            table.AddCell(CreateHeader("Выстрелы", fontPDFColumnName));
    
            int id = -1;
            string CommandName;

            DataTable SQLanswCom = new DataTable();
            SQLQuery = " Select Events.game, Teams.team_name, count(DISTINCT Events.team)from Events inner join Teams on Teams.id_team = Events.team where Events.game = '" + ID + "' group by Events.game";
            SQLanswCom = SQL.SqlRead(SQLQuery);
            int indMAX = Convert.ToInt32(SQLanswCom.Rows[0][2]);
            PdfPCell[] _cellCommand = new PdfPCell[indMAX];

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

                    ind++;
                }
                // добавляем имя игрока
                table.AddCell(CreateGamersProperty(row[2].ToString(), fontPDFData));
                // добавляем рейтинг игрока
                table.AddCell(CreateGamersProperty(row[3].ToString(), fontPDFData));
                // добавляем точность игрока
                double _Accuracy = Convert.ToDouble(row[4]) * 100;
                string Accuracy = _Accuracy.ToString() + " %";
                table.AddCell(CreateGamersProperty(Accuracy, fontPDFData));    
                // добавляем количество выстрелов игрока
                table.AddCell(CreateGamersProperty(row[5].ToString(), fontPDFData));

            }

            double[] reting = new double[indMAX];
            double maxReting = 0;
            double[] accuracy = new double[indMAX];
            double progress;

            SQLQuery = "select Events.game,  Teams.team_name,Events.gamer_name, SUM(Events.rating), "
            + " SUM(Events.accuracy * 100), Count(Events.gamer_name) from Events "
            + "inner join Teams on Teams.id_team = Events.team inner join Games on Games.id_game = Events.game "
            + "where Events.game = '" + ID + "' Group by Teams.team_name";
            SQLansw = SQL.SqlRead(SQLQuery);

            //Считаем командную точность (средняя точность всех игроков)
            //Считаем командный рейтинг (суммарный рейтинг всех игроков)
            //Находим максимальный командный рейтинг
            for (int k = 0; k < indMAX; k++)
            {
                reting[k] = Convert.ToDouble(SQLansw.Rows[k][3]);
                if (maxReting < reting[k]) maxReting = reting[k];
                accuracy[k] = Convert.ToDouble(SQLansw.Rows[k][4]) / Convert.ToDouble(SQLansw.Rows[k][5]);
                accuracy[k] = Math.Round(accuracy[k], 1);
                if (accuracy[k] > 100) accuracy[k] = 100;
            }

            doc.Add(table);
            for (int h = 0; h < reting.Length; h++)
            {
                if (maxReting < reting[h]) maxReting = reting[h];
            }
            for (int h = 0; h < reting.Length; h++)
            {
                if (maxReting < reting[h]) maxReting = reting[h];
            }
            double m = maxReting;
            double[] l = reting;
            double[] u = accuracy;

            // Создаём новую таблицу для вывода общекомандной статистики
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
                    table.AddCell(CreateCommandProperty("Рейтинг", fontPDFDataAll, Element.ALIGN_LEFT));
                    // ЗНАЧЕНИЕ РЕЙТИНКА НЕЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty(reting[iparam].ToString(), fontPDFDataAll, Element.ALIGN_RIGHT));
                    // ЯЧЕЙКА КОЛОНКИ РАЗДЕЛИТЕЛЯ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    // РЕЙТИНГ ЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty("Рейтинг", fontPDFDataAll, Element.ALIGN_LEFT));
                    // ЗНАЧЕНИЕ РЕЙТИНГА ЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty(reting[iparam + 1].ToString(), fontPDFDataAll, Element.ALIGN_RIGHT));

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);
                    // ПРОГРЕССОР РЕЙТИНГА ЧЁТНОЙ КОМАНДЫ

                    progress = reting[iparam] / (maxReting / 234);

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
                    // ПРОГРЕССОР РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ

                    progress = reting[iparam + 1] / (maxReting / 234);

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
                    // ОТСТУП ПРОГРЕССОРА
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ТОЧНОСТЬ НЕЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty("Точночть", fontPDFDataAll, Element.ALIGN_LEFT));
                    // ЗНАЧЕНИЕ ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty(accuracy[iparam].ToString() + " %", fontPDFDataAll, Element.ALIGN_RIGHT));
                    // ЯЧЕЙКА КОЛОНКИ РАЗДЕЛИТЕЛЯ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    // ТОЧНОСТЬ ЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty("Точночть", fontPDFDataAll, Element.ALIGN_LEFT));
                    // ЗНАЧЕНИЕ ТОЧНОСТИ ЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty(accuracy[iparam + 1].ToString() + " %", fontPDFDataAll, Element.ALIGN_RIGHT));

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
                    table.AddCell(CreateCommandProperty("Рейтинг", fontPDFDataAll, Element.ALIGN_LEFT));
                    // ЗНАЧЕНИЕ РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty(reting[iparam].ToString(), fontPDFDataAll, Element.ALIGN_RIGHT));
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

                    progress = reting[iparam] / (maxReting / 234);

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
                    // ПРОГРЕССОР РЕЙТИНГА ЧЁТНОЙ КОМАНДЫ, ЗНАЧЕНИЕ РЕЙТИНГА НЕЧЁТНОЙ КОМАНДЫ С ПУСТОЙ КОЛОНКОЙ ДО И ПОСЛЕ
                    cell = new PdfPCell(new Phrase(" "));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.Colspan = 6;
                    cell.FixedHeight = 10f;
                    table.AddCell(cell);

                    //##########################    НОВАЯ СТРОКА   ##########################

                    // ТОЧНОСТЬ НЕЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty("Точночть", fontPDFDataAll, Element.ALIGN_LEFT));
                    // ЗНАЧЕНИЕ ТОЧНОСТИ НЕЧЁТНОЙ КОМАНДЫ
                    table.AddCell(CreateCommandProperty(accuracy[iparam].ToString() + " %", fontPDFDataAll, Element.ALIGN_RIGHT));
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
        /// Формируемя ячейки для заголовков таблицы
        /// </summary>
        /// <param name="Header"> содержимое ячейки</param>
        /// <param name="font">шрифт</param>
        /// <returns>ячейка с настроенными свойствами</returns>
        private PdfPCell CreateHeader(string Header, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(Header, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.FixedHeight = 30f;
            return cell;
        }

        /// <summary>
        /// Создаем ячейки с атрибутами игроков
        /// </summary>
        /// <param name="Property"> содержимое ячейки</param>
        /// <param name="font">шрифт</param>
        /// <returns>ячейка с настроенными свойствами</returns>
        private PdfPCell CreateGamersProperty(string Property, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(Property, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.BOTTOM_BORDER;
            cell.FixedHeight = 20f;
            cell.BorderColor = new BaseColor(197, 197, 197);
            return cell;
        }

        /// <summary>
        /// Создаем ячейки с атрибутами команд
        /// </summary>
        /// <param name="Property">содержимое ячейки</param>
        /// <param name="font">шрифт</param>
        /// <param name="elign">Выравние в горизонтальной плоскости</param>
        /// <returns></returns>
        private PdfPCell CreateCommandProperty(string Property, iTextSharp.text.Font font, int elign)
        {
            PdfPCell cell = new PdfPCell(new Phrase(Property, font));
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = elign;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.Colspan = 2;
            return cell;
        }

             
    }
}
