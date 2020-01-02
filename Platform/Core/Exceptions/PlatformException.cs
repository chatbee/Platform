using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Exceptions
{
    /// <summary>
    /// For application specific exceptions
    /// </summary>
    public class PlatformException : Exception
    {
        public PlatformException() : base(){}
        public PlatformException(string message) : base(message) { }
        public PlatformException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }

        public PlatformException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
