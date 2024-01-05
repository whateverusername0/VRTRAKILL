using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRTRAKILL.UIManager.Model.Logging
{
    internal class ConsoleLogger : StringLogger
    {
        private readonly bool UseColors = false;

        public ConsoleLogger(bool UseColors = false)
        {
            this.UseColors = UseColors;
        }

        public new void Debug(string Message)
        {
            if (UseColors) Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(base.Debug(Message));
        }
        public new void Info(string Message)
        {
            if (UseColors) Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(base.Info(Message));
        }
        public new void Warn(string Message)
        {
            if (UseColors) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(base.Warn(Message));
        }
        public new void Error(string Message)
        {
            if (UseColors) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(base.Error(Message));
        }
        public new void Fatal(string Message)
        {
            if (UseColors) Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(base.Fatal(Message));
        }
    }
}
