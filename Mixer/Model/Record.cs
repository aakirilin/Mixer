using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixer.Model
{
    internal class Record
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public Record Copy()
        {
            return new Record { Key = Key, Value = Value };
        }
    }
}
