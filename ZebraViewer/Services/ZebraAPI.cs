using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZebraViewer.Services
{
    public class ZebraAPI
    {
        private const string BASE_URL = "http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/";

        public static void CreateImageFileFromPrinter(string printerCode)
        {
            var request = (HttpWebRequest) WebRequest.Create(BASE_URL);

            WebProxy myProxy = new WebProxy();

            Uri newUri = new Uri("http://192.168.0.10:8080");

            // Associate the new Uri object to the myProxy object.
            myProxy.Address = newUri;

            // Create a NetworkCredential object and is assign to the Credentials property of the Proxy object.
            myProxy.Credentials = new NetworkCredential("username", "password");

            request.Proxy = myProxy;

            var zplCode = Encoding.UTF8.GetBytes(printerCode);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zplCode.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zplCode, 0, zplCode.Length);
            requestStream.Close();

            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                var responseStream = response.GetResponseStream();

                var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.png");

                var fileStream = File.Create(fileName);

                responseStream.CopyTo(fileStream);

                responseStream.Close();
                fileStream.Close();
            }
            catch (WebException e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
