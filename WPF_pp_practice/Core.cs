using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace WPF_pp_practice
{
    static internal class Core
    {
        static private Entities1 _context = new Entities1();
        static public Entities1 Context
        {
            get
            {
                return _context;
            }
        }
    }
}
