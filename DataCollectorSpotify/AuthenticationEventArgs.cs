using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollectorSpotify
{
    public class AuthenticationEventArgs : EventArgs
    {
        public string AccessToken { get; set; }
    }
}
