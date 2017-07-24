using System.IO;
using System.Net;

namespace TestLaserwar
{
    public class IpLocation
    {
        public string Status { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string TimeZone { get; set; }
        public string ISP { get; set; }
        public string ORG { get; set; }
        public string AS { get; set; }
        public string Query { get; set; }

        private string IPRequestHelper(string url)
        {
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }

        private string checkIp()
        {
            StreamReader reader;
            HttpWebRequest httpWebRequest;
            HttpWebResponse httpWebResponse;

            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://checkip.dyndns.org");
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                reader = new StreamReader(httpWebResponse.GetResponseStream());
                return System.Text.RegularExpressions.Regex.Match(reader.ReadToEnd(), @"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})").Groups[1].Value;
            }
            catch
            {
                return "error";
            }
        }


        public void GetCountryByIP()
        {
            string ipAddress =checkIp();
            if (ipAddress != "error")
            {
                string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + ipAddress);
                using (TextReader sr = new StringReader(ipResponse))
                {
                    using (System.Data.DataSet dataBase = new System.Data.DataSet())
                    {
                        dataBase.ReadXml(sr);
                        Status = dataBase.Tables[0].Rows[0][0].ToString();
                        Country = dataBase.Tables[0].Rows[0][1].ToString();
                        CountryCode = dataBase.Tables[0].Rows[0][2].ToString();
                        Region = dataBase.Tables[0].Rows[0][3].ToString();
                        RegionName = dataBase.Tables[0].Rows[0][4].ToString();
                        City = dataBase.Tables[0].Rows[0][5].ToString();
                        Zip = dataBase.Tables[0].Rows[0][6].ToString();
                        Lat = dataBase.Tables[0].Rows[0][7].ToString();
                        Lon = dataBase.Tables[0].Rows[0][8].ToString();
                        TimeZone = dataBase.Tables[0].Rows[0][9].ToString();
                        ISP = dataBase.Tables[0].Rows[0][10].ToString();
                        ORG = dataBase.Tables[0].Rows[0][11].ToString();
                        AS = dataBase.Tables[0].Rows[0][12].ToString();
                        Query = dataBase.Tables[0].Rows[0][13].ToString();

                    }
                }
            }
        }
    }


}
