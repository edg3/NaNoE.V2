using NaNoE.V2.Droid.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaNoE.V2.Droid
{
    class ViewModelLocator
    {
        public MainViewModel MainVM { get; private set; }

        public ViewModelLocator()
        {
            MainVM = new MainViewModel();
        }
    }
}
