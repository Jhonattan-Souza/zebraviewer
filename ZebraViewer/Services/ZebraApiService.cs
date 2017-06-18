using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZebraViewer.Services.Interfaces;

namespace ZebraViewer.Services
{
    public class ZebraApiService : IZebraApiService
    {
        private const string BaseUrl = "http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/";

        public async Task<string> GetLabelAsync(string zplCode)
        {
            var httpClient = new HttpClient();

            var content = new StringContent(zplCode,
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync(BaseUrl, content);

            if (!response.IsSuccessStatusCode) return null;

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                string fileName;

                using (var file = File.Create("ZPL_Label.png"))
                {
                    fileName = file.Name;
                    await responseStream.CopyToAsync(file);
                }

                return fileName;
            }
        }
    }
}
