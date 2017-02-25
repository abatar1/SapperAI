using System;

namespace Soliter.Views
{
    public class MessageReporter : IMessageReporter
    {
        public void ReportMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
