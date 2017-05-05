using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZebraViewer.Services
{
    public class ZebraAPI
    {
        private const string BASE_URL = "http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/";

        public void GetPrinterFile(string printerCode)
        {
            var request = (HttpWebRequest) WebRequest.Create(BASE_URL);

            var zplCode = EncodeToByte(printerCode);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zplCode.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zplCode, 0, zplCode.Length);
            requestStream.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var fileStream = File.Create(@"E:\dev\label.png");
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();
            }
            catch (WebException e)
            {
                e.ToString();
            }
        }

        private byte[] EncodeToByte(string printerCode)
        {
            return Encoding.UTF8.GetBytes(printerCode);
        }
    }
}
