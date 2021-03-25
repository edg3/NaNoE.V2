using NaNoE.V2.Droid.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NaNoE.V2.Droid
{
    class Navigator
    {
        private MainPage _main;

        public static Navigator Instance { get; private set; }

        public Navigator(MainPage mainWindow)
        {
            _main = mainWindow;
            Instance = this;
        }

        public void GoTo(string name)
        {
            ContentView w = null;
            switch (name)
            {
                case "start": w = new MainView(); break;
            }

            if (null != w)
            {
                (_main.Content.FindByName("frmContent") as Frame).Content = w.Content;
            }    
        }
    }
}
