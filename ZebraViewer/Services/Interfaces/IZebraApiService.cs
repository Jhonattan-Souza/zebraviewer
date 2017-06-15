using System.Threading.Tasks;

namespace ZebraViewer.Services.Interfaces
{
    public interface IZebraApiService
    {
        Task<string> GetLabelAsync(string zplCode);
    }
}
