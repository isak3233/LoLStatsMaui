using System;
using System.Collections.Generic;
using System.Text;

namespace LoLStatsMaui.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message) { }
    }
}
