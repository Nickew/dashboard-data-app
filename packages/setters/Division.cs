using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intra_app.packages.setters
{
    class Division
    {
        public object item { get; set; }

        public string index { get; set; }

        public Division() { }

        public string setInfo(string i)
        {
            index = i;
            return i;
        }

        public string getInfo()
        {
            return index;
        }

        public void del()
        {

        }
    }
}
