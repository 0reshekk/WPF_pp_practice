using System;
using System.Collections.Generic;
using System.IO;
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

    partial class Products
    {
        public string StringMaterials
        {
            get
            {
                // Проверяем сперва есть ли связь
                var list = ProductsMaterials.Select(p => p.Materials?.Name).ToList();
                return string.Join(", ", list);
            }
        }

        public bool MinPriceMoreThan10k
        {
            get
            {
                return MinPrice > 10000;
            }
        }
        public string PhotoPath
        {
            get
            {
                var baseDir = Path.Combine(Environment.CurrentDirectory, "images");
                string fileName = !string.IsNullOrEmpty(Photo) && File.Exists(Path.Combine(baseDir, Photo)) ? Photo : "picture.jpg";
                return Path.Combine(baseDir, fileName);
            }
        }
    }
}
