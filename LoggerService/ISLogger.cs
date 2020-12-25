using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerService
{
    public interface ISLogger
    {
        public void Log(string message);
        public void ClearLog();
    }
}
