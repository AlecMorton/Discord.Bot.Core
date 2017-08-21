using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;

namespace INVO.Core.Common
{
    class Functions
    {
        public static string BotTextFormat(string message)
        {
            string x = string.Format("```css\r\n {0}```", message);
            return x;
        }
    }
}
