using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraViewer.Exceptions
{
    class ZPLException : Exception
    {
        private static readonly string MESSAGE = $"Erro ao efetuar o download" +
            $" da imagem. Verifique se o computador tem acesso a internet.";

        public ZPLException(Exception inner):base(MESSAGE, inner)
        {
        }
    }
}
