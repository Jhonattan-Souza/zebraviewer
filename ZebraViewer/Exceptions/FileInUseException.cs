using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraViewer.Helpers
{
    class FileInUseException : Exception
    {
        private static readonly string MESSAGE = $"Erro ao efetuar a leitura do Output" +
            $" da Impressora. Verifique se o seguinte arquivo está sendo utilizado por outra aplicação: " +
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}" +
            $"\\label.txt";

        public FileInUseException(Exception inner):base(MESSAGE, inner)
        {            
        }
    }
}
