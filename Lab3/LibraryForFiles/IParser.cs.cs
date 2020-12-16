using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForFiles
{
    interface IParser
    {
        T Parse<T>(string path) where T : new();
    }
}
