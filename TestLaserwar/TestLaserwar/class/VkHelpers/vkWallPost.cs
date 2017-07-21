using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace TestLaserwar
{
    class vkWallPost
    {
        string token;
        public vkWallPost(string token)
        {
            this.token = token;
        }

        private string Response(string request_path)    //на вход подаем URL API, например [url]https://api.vk.com/method/photos.getWallUploadServer?group_id=123[/url]
        {
            try
            {
                string response = string.Empty;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(request_path);   //отправление запроса к серверу API
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();      //получение ответа от сервера API
                Stream receiveStream = Response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(receiveStream, encode);

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);

                while (count > 0)
                {
                    String line = new String(read, 0, count);
                    response += line + "\r\n";
                    count = readStream.Read(read, 0, 256);
                }

                Response.Close();
                readStream.Close();

                return response;
            }
            catch (Exception) { return null; }
        }

        private string photosGetWallUploadServer(int group_id)    //получить сервер для загрузки фото на стену (возвращает upload_url)
        {
            try
            {
                string request_path = "https://api.vk.com/method/photos.getWallUploadServer?";    //формируем ссылку с нужными параметрами для запроса к API
                request_path += "group_id" + group_id;
                request_path += "&access_token=" + token;
                request_path += "&v=5.21";


                var json = JObject.Parse(Response(request_path));     //json парсер
                return json["response"]["upload_url"].ToString();       //возвращает upload_url
            }
            catch (Exception) { return null; }
        }

        private JObject photosUploadPhotoToURL(string URL, string file_path)    //загрузка фото на сервер
        {
            try
            {
                WebClient myWebClient = new WebClient();
                byte[] responseArray = myWebClient.UploadFile(URL, file_path);
                var json = JObject.Parse(System.Text.Encoding.ASCII.GetString(responseArray));

                return json;
            }
            catch (Exception) { return null; }
        }

        private JObject photosSaveWallPhoto(string server, string photo, string hash)
        {
            try
            {
                string request_path = "https://api.vk.com/method/photos.saveWallPhoto?";
                request_path += "server=" + server;
                request_path += "&photo=" + photo;
                request_path += "&hash=" + hash;
                request_path += "&v=5.21";
                request_path += "&access_token=" + token;

                var json = JObject.Parse((Response(request_path).Replace("[", String.Empty)).Replace("]", String.Empty));       //сначала убираем '[' и ']' из ответа сервера, а зачем парсим
                return json;  //возвращаем объект класса JObject
            }
            catch (Exception) { return null; }
        }

        private string wallPost(int owner_id, int friends_only = 0, int from_group = 1, string message = "", string attachments = "", long publish_date = 0)                    //пост на стенку
        {
            try
            {
                if (message == "" && attachments == "") return "Error: message and attachments is empty!";                //не вызывать API, если msg and attach пустые

                string request_path = "https://api.vk.com/method/wall.post?";     //путь обращения к vk.api
                request_path += "owner_id=" + owner_id;
                request_path += "&friends_only=" + friends_only;
                request_path += "&from_group=" + from_group;
                if (message != string.Empty) request_path += "&message=" + message;
                if (attachments != string.Empty) request_path += "&attachments=" + attachments;
                if (publish_date != 0) request_path += "&publish_date=" + publish_date;
                request_path += "&v=5.21";
                request_path += "&access_token=" + token;                                                          //токен (задается в конструкторе)

                return Response(request_path);
            }
            catch (Exception) { return null; }
        }

        public string AddWallPost(int gid, string message = "", string attachment = "")
        {
            try
            {
                if (message == "" && attachment == "") return "Error";

                string response = string.Empty;
                //int hour = int.Parse(publish_date.Split(':')[0]);
                //int min = int.Parse(publish_date.Split(':')[1]);
                /*
                if ((min % 5) != 0) min += 5 - (min % 5);
                if (min == 60)
                {
                    hour++;
                    min = 0;
                }
 
                DateTime data_time = new DateTime(2014, 5, 30, hour, min, 0);
                long time = (data_time.ToUniversalTime().Ticks - 621355968000000000) / 10000000; //перевод числа в unixtime
*/
                if (attachment != "")
                {
                    //System.Net.WebRequest req = new System.Net.WebRequest.Create("https://api.vkontakte.ru/method/photos.getWallUploadServer?gid=100669061&access_token=" + textBox3.Text.Trim());
                    string img_path = photosGetWallUploadServer(gid);
                    var resp = photosUploadPhotoToURL(img_path, attachment);
                    resp = photosSaveWallPhoto(resp["server"].ToString(), resp["photo"].ToString(), resp["hash"].ToString());
                    attachment = "photo" + resp["response"]["owner_id"] + "_" + resp["response"]["id"];
                }

                return wallPost(gid, message: message, attachments: attachment);
            }
            catch (Exception) { return null; }
        }
    }
}
