using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookTogether.Data.MyData
{
    public class SQLConnectionConfig
    {
        public SQLConnectionConfig(string value) => Value = value;

        public string Value { get; }
    }
}
