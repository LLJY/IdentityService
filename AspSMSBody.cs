using System;
using System.Collections.Generic;

namespace IdentityService
{
    public class AspSmsBody
    {
        public AspSmsBody(string userName, string password, string originator, string[] recipients, string messageText)
        {
            UserName = userName;
            Password = password;
            Originator = originator;
            Recipients = recipients;
            MessageText = messageText;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Originator { get; set; }
        public string[] Recipients { get; set; }
        public string MessageText { get; set; }
    }
}