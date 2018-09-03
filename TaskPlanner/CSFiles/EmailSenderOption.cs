using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.CSFiles
{
    public class EmailSenderOption
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string SenderEmail { get; set; }
    }
}
