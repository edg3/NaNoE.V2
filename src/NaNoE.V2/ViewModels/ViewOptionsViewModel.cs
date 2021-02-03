using System.Collections.Generic;

namespace NaNoE.V2.ViewModels
{
    class ViewOptionsViewModel
    {
        private List<string> _options = new List<string>() { "Dark", "Light", "Large Dark", "Large Light" };
        public List<string> Options
        {
            get { return _options; }
        }


    }
}
