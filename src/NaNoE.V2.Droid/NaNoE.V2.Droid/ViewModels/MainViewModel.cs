using System;
using System.Collections.Generic;
using System.Text;

namespace NaNoE.V2.Droid.ViewModels
{
    class MainViewModel
    {
        public string IPAddress { get; set; }
        public string Password { get; set; }

        public CommandBase GoIP { get; private set; }

        public MainViewModel()
        {
            IPAddress = "";
            Password = "";

            GoIP = new CommandBase(() =>
            {
                // Try connect
            });
        }
    }
}
