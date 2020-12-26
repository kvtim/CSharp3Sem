using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManager.Options
{
    public class Option
    {
        public string Target { get; set; }

        public string Source { get; set; }

        public Option()
        {
            Target = @"D:\Projects\TargetDirectory";

            Source = @"D:\Projects\SourceDirectory";
        }

    }
}
